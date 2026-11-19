using System.Text.RegularExpressions;
using Educ8.Itslearning.Http;

namespace Educ8.Itslearning.Models;

/// <summary>
/// A lesson plan's material. <see cref="FolderId"/> is set only when it was found through the plan-named folder (fallback),
/// so callers can point at the folder; null when it came from the plan itself.
/// </summary>
public sealed record PlanMaterials(int? FolderId, List<CourseResource> Items);

/// <summary>A lesson plan linked from a calendar event, with what's known about it (description, material).</summary>
public sealed record LinkedPlan(CalendarPlanLink Link, PlanPreview? Plan, PlanMaterials? Materials);

/// <summary>A planner topic's plans (<see cref="Topic"/> null: plans without a topic).</summary>
public sealed record PlanGroup(PlanTopic? Topic, IReadOnlyList<CoursePlan> Plans);

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
    private const int MaxPlans = 15;

    /// <summary>
    /// The material attached to each plan, read from the plan itself (<see cref="ItslearningClient.GetPlanAsync"/>).
    /// Plans that can't be read that way fall back to <see cref="FromTopicFoldersAsync"/>. Best effort: plans with neither are left out.
    /// </summary>
    public static async Task<IReadOnlyDictionary<int, PlanMaterials>> GetPlanMaterialsAsync(
        this ItslearningClient client, int courseId, IReadOnlyList<CourseTopic>? topics, IReadOnlyList<PlanPreview>? plans, CancellationToken ct = default)
    {
        if (plans is null || plans.Count == 0) return new Dictionary<int, PlanMaterials>();
        var wanted = plans.DistinctBy(p => p.PlanId).Take(MaxPlans).ToList();

        async Task<CoursePlan?> TryPlan(int planId)
        {
            try { return await client.GetPlanAsync(courseId, planId, ct).ConfigureAwait(false); }
            catch (Exception e) when (e is ItslearningApiException or HttpRequestException or System.Text.Json.JsonException) { return null; }
        }

        var fetched = await Task.WhenAll(wanted.Select(async p => (p.PlanId, Plan: await TryPlan(p.PlanId).ConfigureAwait(false)))).ConfigureAwait(false);
        var result = fetched.Where(f => f.Plan is not null)
            .ToDictionary(f => f.PlanId, f => new PlanMaterials(null, ToResources(client, courseId, f.Plan!)));

        var missing = wanted.Where(p => !result.ContainsKey(p.PlanId)).ToList();
        if (missing.Count > 0)
            foreach (var (planId, materials) in await client.FromTopicFoldersAsync(courseId, topics, missing, ct).ConfigureAwait(false))
                result[planId] = materials;
        return result;
    }

    /// <summary>
    /// Every plan in the course, grouped by planner topic in itslearning's order (plans by <see cref="CoursePlan.OrderNumber"/>;
    /// groups by their first plan). Plans without a topic form a group with no topic. Untitled empty placeholder plans are left out.
    /// </summary>
    public static async Task<IReadOnlyList<PlanGroup>> GetAllPlansAsync(this ItslearningClient client, int courseId, CancellationToken ct = default)
    {
        var topics = await client.GetPlanTopicsAsync(courseId, ct).ConfigureAwait(false);
        var lists = await Task.WhenAll(new PlanTopic?[] { null }.Concat(topics.Where(t => t.PlanCount > 0)).Select(async t =>
            new PlanGroup(t, (await client.GetTopicPlansAsync(courseId, t?.Id ?? 0, ct).ConfigureAwait(false))
                .Where(p => p.IsActive && (p.Title.Trim().Length > 0 || p.Elements.Count > 0))
                .OrderBy(p => p.OrderNumber).ToList()))).ConfigureAwait(false);
        return lists.Where(g => g.Plans.Count > 0).OrderBy(g => g.Plans[0].OrderNumber).ToList();
    }

    /// <summary>A plan's active elements as course resources (absolute links), as shown under the plan.</summary>
    public static List<CourseResource> MaterialsOf(this ItslearningClient client, int courseId, CoursePlan plan) => ToResources(client, courseId, plan);

    private static List<CourseResource> ToResources(ItslearningClient client, int courseId, CoursePlan plan) =>
        plan.Elements.Where(e => e.IsActive).Select(e => new CourseResource
        {
            ElementId = e.Id,
            ElementType = e.IsFolder ? "Folder" : e.Type,
            LearningToolId = e.LearningToolId,
            Title = e.Title,
            CourseId = courseId,
            Url = e.Url is null ? null : client.BaseAddress is { } b && Uri.TryCreate(b, e.Url, out var u) ? u.ToString() : e.Url,
        }).ToList();

    /// <summary>
    /// Fallback: a plan's material often lives in a subfolder of its topic's folder named like the plan (e.g. topic "02: OS" → "Lecture").
    /// Best effort: folders that can't be listed are skipped.
    /// </summary>
    private static async Task<Dictionary<int, PlanMaterials>> FromTopicFoldersAsync(
        this ItslearningClient client, int courseId, IReadOnlyList<CourseTopic>? topics, IReadOnlyList<PlanPreview> plans, CancellationToken ct)
    {
        if (topics is null) return [];
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

        var folderIds = planFolder.Values.Distinct().Take(MaxPlans).ToList();
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
