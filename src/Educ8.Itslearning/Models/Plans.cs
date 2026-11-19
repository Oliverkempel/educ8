using System.Text.RegularExpressions;
using Educ8.Itslearning.Http;

namespace Educ8.Itslearning.Models;

/// <summary>The folder holding a lesson plan's material, and what's in it.</summary>
public sealed record PlanMaterials(int FolderId, List<CourseResource> Items);

/// <summary>A lesson plan linked from a calendar event, with what's known about it (description, material).</summary>
public sealed record LinkedPlan(CalendarPlanLink Link, PlanPreview? Plan, PlanMaterials? Materials);

/// <summary>Course code helpers.</summary>
public static partial class CourseCodes
{
    /// <summary>Semester from an SDU-style course code suffix, e.g. "T510046101-1-E26" → "E26" (E = autumn, F = spring).</summary>
    public static string? Semester(string? courseCode) =>
        courseCode is not null && SemesterSuffix().Match(courseCode) is { Success: true } m ? m.Groups[1].Value : null;

    /// <summary>The semester running on <paramref name="date"/>: Aug–Jan is autumn (E, January counts to the previous year), Feb–Jul is spring (F).</summary>
    public static string CurrentSemester(DateOnly date) => date.Month switch
    {
        >= 8 => $"E{date.Year % 100:00}",
        1 => $"E{(date.Year - 1) % 100:00}",
        _ => $"F{date.Year % 100:00}",
    };

    [GeneratedRegex(@"-([EF]\d{2})$")]
    private static partial Regex SemesterSuffix();
}

/// <summary>Lesson-plan lookups shared by the MCP tools and the desktop app.</summary>
public static class PlanLookups
{
    /// <summary>
    /// A plan's material lives in a subfolder of its topic's folder named like the plan (e.g. topic "02: OS" → "Lecture").
    /// The plan-info endpoint that would say this directly returns nothing for students (see docs/api/live-findings.md).
    /// Best effort: folders that can't be listed are skipped.
    /// </summary>
    public static async Task<IReadOnlyDictionary<int, PlanMaterials>> GetPlanMaterialsAsync(
        this ItslearningClient client, int courseId, IReadOnlyList<CourseTopic>? topics, IReadOnlyList<PlanPreview>? plans, CancellationToken ct = default)
    {
        if (topics is null || plans is null || plans.Count == 0) return new Dictionary<int, PlanMaterials>();
        var topicFolders = topics.Where(t => t.FolderId is not null).ToDictionary(t => t.TopicId, t => t.FolderId!.Value);
        var topicIds = plans.Select(p => p.TopicId).Where(topicFolders.ContainsKey).Distinct().Take(8).ToList();

        async Task<List<CourseResource>?> TryList(int folderId)
        {
            try { return await client.GetFolderResourcesAsync(courseId, folderId, ct).ConfigureAwait(false); }
            catch (Exception e) when (e is ItslearningApiException or HttpRequestException) { return null; }
        }

        var subfolders = await Task.WhenAll(topicIds.Select(async id => (TopicId: id, Items: await TryList(topicFolders[id]).ConfigureAwait(false)))).ConfigureAwait(false);
        var planFolder = new Dictionary<int, int>();
        foreach (var plan in plans)
        {
            var match = subfolders.FirstOrDefault(s => s.TopicId == plan.TopicId).Items?
                .FirstOrDefault(r => r.Kind == ResourceKind.Folder && r.Title.Trim().Equals(plan.PlanName.Trim(), StringComparison.OrdinalIgnoreCase));
            if (match is not null) planFolder[plan.PlanId] = match.ElementId;
        }

        var folderIds = planFolder.Values.Distinct().Take(15).ToList();
        var contents = (await Task.WhenAll(folderIds.Select(async id => (Id: id, Items: await TryList(id).ConfigureAwait(false)))).ConfigureAwait(false))
            .Where(f => f.Items is not null).ToDictionary(f => f.Id, f => f.Items!);
        return planFolder.Where(p => contents.ContainsKey(p.Value))
            .ToDictionary(p => p.Key, p => new PlanMaterials(p.Value, contents[p.Value]));
    }

    /// <summary>
    /// The lesson plans linked from an event, with their description (from the course planner) and material.
    /// Best effort: if the planner can't be read, plans are returned with only their link.
    /// </summary>
    public static async Task<IReadOnlyList<LinkedPlan>> GetEventPlansAsync(this ItslearningClient client, CalendarEventDetails e, CancellationToken ct = default)
    {
        if (e.PlanLinks.Count == 0) return [];
        if (e.EventType != "Course" || e.LocationId <= 0) return e.PlanLinks.Select(l => new LinkedPlan(l, null, null)).ToList();
        try
        {
            var day = DateOnly.FromDateTime(e.FromDate.UtcDateTime);
            var plansTask = client.GetCoursePlansAsync(e.LocationId, day.AddDays(-1), day.AddDays(1), ct);
            var topicsTask = client.GetCourseTopicsAsync(e.LocationId, ct);
            await Task.WhenAll(plansTask, topicsTask).ConfigureAwait(false);
            var linked = plansTask.Result.Where(p => e.PlanLinks.Any(l => l.PlanId == p.PlanId)).DistinctBy(p => p.PlanId).ToList();
            var materials = await client.GetPlanMaterialsAsync(e.LocationId, topicsTask.Result, linked, ct).ConfigureAwait(false);
            var previews = linked.ToDictionary(p => p.PlanId);
            return e.PlanLinks.Select(l => new LinkedPlan(l, previews.GetValueOrDefault(l.PlanId), materials.GetValueOrDefault(l.PlanId))).ToList();
        }
        catch (Exception ex) when (ex is ItslearningApiException or HttpRequestException)
        {
            return e.PlanLinks.Select(l => new LinkedPlan(l, null, null)).ToList();
        }
    }
}
