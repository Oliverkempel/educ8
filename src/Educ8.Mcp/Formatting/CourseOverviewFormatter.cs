using System.Text;
using Educ8.Documents;
using Educ8.Itslearning.Models;

namespace Educ8.Mcp.Formatting;

/// <summary>A section that may have failed to load without failing the whole overview.</summary>
internal sealed record Section<T>(T? Value, string? Error)
{
    public static async Task<Section<T>> LoadAsync(Func<Task<T>> load)
    {
        try { return new(await load(), null); }
        catch (Educ8.Itslearning.Auth.NotAuthenticatedException) { throw; }
        catch (Exception e) when (e is Educ8.Itslearning.Http.ItslearningApiException or HttpRequestException or TaskCanceledException)
        {
            return new(default, "couldn't load this part right now");
        }
    }
}

internal sealed record CourseOverview(
    CourseInfo Course,
    Section<List<CourseTopic>> Topics,
    Section<List<CourseResource>> RootResources,
    Section<List<PlanPreview>> Plans,
    Section<List<CourseTask>> Tasks,
    Section<EntityList<LightBulletin>> Bulletins,
    DateOnly PlansFrom,
    DateOnly PlansTo,
    IReadOnlyDictionary<int, PlanMaterials> PlanMaterials,
    Section<List<StreamItem>>? RecentChanges = null);

/// <summary>The folder holding a lesson plan's material, and what's in it.</summary>
internal sealed record PlanMaterials(int FolderId, List<CourseResource> Items);

internal static class CourseOverviewFormatter
{
    private const int MaxPlanItems = 12;
    internal const int RecentChangesDays = 14;
    private const int MaxRecentChanges = 5;

    public static string Format(CourseOverview o, TimeZoneInfo zone)
    {
        var sb = new StringBuilder();
        var semester = CourseFormatter.Semester(o.Course.Code);
        sb.Append($"# {o.Course.Title}\n\n");
        sb.Append($"Course id {o.Course.CourseId}");
        if (!string.IsNullOrEmpty(o.Course.Code)) sb.Append($" · code {o.Course.Code}");
        if (semester is not null) sb.Append($" · semester {semester}");
        sb.Append($"\n\nTimes are shown in {zone.Id} time.\n");

        // Topics
        var topicNames = o.Topics.Value?.ToDictionary(t => t.TopicId, t => t.TopicName) ?? [];
        sb.Append("\n## Topics\n\n");
        if (o.Topics.Error is { } te) sb.Append($"_({te})_\n");
        else if (o.Topics.Value!.Count == 0) sb.Append("No topics.\n");
        else
        {
            sb.Append("Each topic has a folder of material. Browse it with `browse_course_materials` and the folder id.\n\n| Topic | Folder id |\n|---|---|\n");
            foreach (var t in o.Topics.Value) sb.Append($"| {Markdown.Cell(t.TopicName)} | {t.FolderId?.ToString() ?? ""} |\n");
        }

        // Lesson plans
        sb.Append($"\n## Lesson plans {o.PlansFrom:yyyy-MM-dd} – {o.PlansTo:yyyy-MM-dd}\n\n");
        if (o.Plans.Error is { } pe) sb.Append($"_({pe})_\n");
        else if (o.Plans.Value!.Count == 0) sb.Append("No lesson plans in this period.\n");
        else
            foreach (var p in o.Plans.Value)
            {
                sb.Append($"- {When(p.Start, p.Stop, zone)} · **{Markdown.Cell(p.PlanName)}**");
                if (topicNames.TryGetValue(p.TopicId, out var topic)) sb.Append($" (topic: {topic})");
                var description = HtmlToMarkdown.ConvertExcerpt(p.PlanDescription, 300).Replace("\n", " ");
                if (description.Length > 0) sb.Append($": {description}");
                sb.Append('\n');
                if (o.PlanMaterials.TryGetValue(p.PlanId, out var materials))
                {
                    if (materials.Items.Count == 0) sb.Append($"  - (material folder {materials.FolderId} is empty)\n");
                    foreach (var r in materials.Items.Take(MaxPlanItems))
                        sb.Append($"  - {ResourceFormatter.KindLabel(r.Kind, r.LearningToolId)}: {Markdown.Cell(r.Title)} · element {r.ElementId} · {ResourceFormatter.NextStep(r.Kind)}\n");
                    if (materials.Items.Count > MaxPlanItems)
                        sb.Append($"  - … {materials.Items.Count - MaxPlanItems} more: browse folder {materials.FolderId}\n");
                }
            }

        // Tasks
        sb.Append("\n## Open tasks\n\n");
        if (o.Tasks.Error is { } ke) sb.Append($"_({ke})_\n");
        else if (o.Tasks.Value!.Count == 0) sb.Append("No open tasks.\n");
        else
            foreach (var t in o.Tasks.Value.OrderBy(t => t.Deadline ?? DateTimeOffset.MaxValue))
            {
                sb.Append($"- **{Markdown.Cell(t.Title)}** · {ResourceFormatter.KindLabel(t.Kind, t.LearningToolId)} · element {t.ElementId}");
                sb.Append(t.Deadline is { } d ? $" · deadline {Local(d, zone):ddd yyyy-MM-dd HH:mm}" : " · no deadline set");
                if (!string.IsNullOrWhiteSpace(t.Status)) sb.Append($" · {t.Status}");
                var description = HtmlToMarkdown.ConvertExcerpt(t.Description, 200).Replace("\n", " ");
                if (description.Length > 0) sb.Append($": {description}");
                sb.Append('\n');
            }

        // Announcements
        sb.Append("\n## Latest announcements\n\n");
        if (o.Bulletins.Error is { } be) sb.Append($"_({be})_\n");
        else if (o.Bulletins.Value!.EntityArray.Count == 0) sb.Append("No announcements.\n");
        else
            foreach (var b in o.Bulletins.Value.EntityArray)
            {
                var date = b.PublishedDate is { } pd ? Local(pd, zone).ToString("yyyy-MM-dd") : "?";
                sb.Append($"- {date}{(b.Pinned ? " (pinned)" : "")} · {Markdown.Cell(b.PublishedBy?.FullName)}: ");
                sb.Append(HtmlToMarkdown.Truncate(string.Join(' ', b.Text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)), 500));
                if (!string.IsNullOrEmpty(b.EmbedUrl) && !b.Text.Contains(b.EmbedUrl, StringComparison.Ordinal)) sb.Append($" (link: {b.EmbedUrl})");
                sb.Append('\n');
            }

        // Recent changes
        if (o.RecentChanges is { } changes)
        {
            sb.Append($"\n## Recent changes (last {RecentChangesDays} days)\n\n");
            if (changes.Error is { } ce) sb.Append($"_({ce})_\n");
            else if (changes.Value!.Count == 0) sb.Append("Nothing new.\n");
            else
            {
                foreach (var item in changes.Value.Take(MaxRecentChanges)) sb.Append(UpdatesFormatter.StreamLine(item, zone, withCourse: false, withDate: true));
                sb.Append($"For details and grouped items, use `whats_new` with courseId {o.Course.CourseId}.\n");
            }
        }

        // Root material
        sb.Append("\n## Top-level course material\n\n");
        if (o.RootResources.Error is { } re) sb.Append($"_({re})_\n");
        else if (o.RootResources.Value!.Count == 0) sb.Append("The course has no material at the top level.\n");
        else sb.Append(ResourceFormatter.Table(o.RootResources.Value, includePath: false)).Append('\n').Append(ResourceFormatter.UseLegend).Append('\n');

        return sb.ToString();
    }

    private static DateTimeOffset Local(DateTimeOffset value, TimeZoneInfo zone) => TimeFormat.Local(value, zone);

    private static string When(DateTimeOffset? start, DateTimeOffset? stop, TimeZoneInfo zone) => TimeFormat.When(start, stop, zone);
}
