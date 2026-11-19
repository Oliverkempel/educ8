using System.Text;
using Educ8.Documents;
using Educ8.Itslearning.Models;

namespace Educ8.Mcp.Formatting;

/// <summary>A lesson plan linked from an event, with what's known about it.</summary>
internal sealed record LinkedPlan(CalendarPlanLink Link, PlanPreview? Plan, PlanMaterials? Materials);

internal static class ScheduleFormatter
{
    public static string FormatSchedule(IReadOnlyList<CalendarEvent> events, DateTime fromLocal, DateTime toLocal, string? courseTitle, int maxResults, TimeZoneInfo zone)
    {
        var sb = new StringBuilder();
        var range = $"{TimeFormat.Day(fromLocal)} – {TimeFormat.Day(toLocal.AddDays(-1))}";
        sb.Append(courseTitle is null ? $"## Schedule {range}\n\n" : $"## Schedule for {courseTitle}, {range}\n\n");
        sb.Append($"Times in {zone.Id}. Event ids work with `get_event` (linked lesson plan and material).\n");
        if (events.Count == 0) return sb.Append("\nNo events in this period.").ToString();

        var shown = 0;
        foreach (var day in events.GroupBy(e => TimeFormat.Local(e.FromDate, zone).Date))
        {
            if (shown >= maxResults) break;
            sb.Append($"\n### {TimeFormat.Day(day.Key)}\n\n");
            foreach (var e in day)
            {
                if (shown++ >= maxResults) break;
                sb.Append($"- {TimeFormat.TimeSpan(e.FromDate, e.ToDate, zone)} · **{Markdown.Cell(e.EventTitle)}**");
                if (courseTitle is null && !string.IsNullOrWhiteSpace(e.LocationTitle))
                    sb.Append($" · {Markdown.Cell(e.LocationTitle)}{(e.EventType == "Course" ? $" (course {e.LocationId})" : "")}");
                var details = Details(e);
                if (details.Length > 0) sb.Append($" · {details}");
                sb.Append($" · event {e.EventId}\n");
            }
        }
        if (events.Count > maxResults) sb.Append($"\n{events.Count - maxResults} more events not shown: narrow the period or pass a courseId.\n");
        return sb.ToString().TrimEnd();
    }

    public static string FormatEvent(CalendarEventDetails e, IReadOnlyList<LinkedPlan> plans, TimeZoneInfo zone)
    {
        var sb = new StringBuilder();
        sb.Append($"# {e.EventTitle}\n\n");
        sb.Append($"**When:** {TimeFormat.When(e.FromDate, e.ToDate, zone)} ({zone.Id})  \n");
        if (!string.IsNullOrWhiteSpace(e.LocationTitle))
            sb.Append($"**Where it belongs:** {e.LocationTitle}{(e.EventType == "Course" ? $" (course {e.LocationId})" : $" ({e.EventType})")}  \n");
        if (!string.IsNullOrWhiteSpace(e.ImportDescription)) sb.Append($"**Timetable info:** {Markdown.Cell(e.ImportDescription)}  \n");
        if (e.Bookings.Count > 0) sb.Append($"**Booked:** {string.Join(", ", e.Bookings.Select(b => b.Name))}  \n");
        sb.Append($"Event {e.EventId}");
        if (!string.IsNullOrEmpty(e.EventUrl)) sb.Append($" · open in itslearning: {e.EventUrl}");
        sb.Append("\n\n");

        var description = HtmlToMarkdown.Convert(e.Description);
        if (description.Length > 0) sb.Append("## Description\n\n").Append(description).Append("\n\n");
        if (e.HasMoreContent && description.Length > 0)
            sb.Append("_itslearning reports more content than the API returns (formatting or images); open the event in itslearning for the full version._\n\n");

        if (plans.Count > 0)
        {
            sb.Append("## Lesson plan\n\n");
            foreach (var (link, plan, materials) in plans)
            {
                sb.Append($"- **{Markdown.Cell(link.PlanName)}**{(string.IsNullOrWhiteSpace(link.TopicName) ? "" : $" (topic: {link.TopicName})")}");
                if (!string.IsNullOrEmpty(link.Url)) sb.Append($" · {link.Url}");
                sb.Append('\n');
                var planText = HtmlToMarkdown.ConvertExcerpt(plan?.PlanDescription, 1500);
                if (planText.Length > 0) sb.Append(string.Join('\n', planText.Split('\n').Select(l => "  " + l))).Append('\n');
                if (materials is null) continue;
                if (materials.Items.Count == 0) sb.Append($"  - (material folder {materials.FolderId} is empty)\n");
                foreach (var r in materials.Items.Take(15))
                    sb.Append($"  - {ResourceFormatter.KindLabel(r.Kind, r.LearningToolId)}: {Markdown.Cell(r.Title)} · element {r.ElementId} · {ResourceFormatter.NextStep(r.Kind)}\n");
                if (materials.Items.Count > 15) sb.Append($"  - … {materials.Items.Count - 15} more: browse folder {materials.FolderId}\n");
            }
            sb.Append('\n');
        }

        if (e.ElementLinks.Count > 0)
        {
            sb.Append("## Linked material\n\n");
            foreach (var l in e.ElementLinks)
                sb.Append($"- {ResourceFormatter.KindLabel(l.Kind, l.LearningToolId)}: {Markdown.Cell(l.Title)}{(l.Homework ? " (homework)" : "")} · element {l.ElementId} · {ResourceFormatter.NextStep(l.Kind)}\n");
        }
        return sb.ToString().TrimEnd();
    }

    /// <summary>Extra info for a list line: timetable text (room, teachers) or a short description.</summary>
    private static string Details(CalendarEvent e)
    {
        var import = e.ImportDescription?.Trim();
        if (!string.IsNullOrEmpty(import)) return Markdown.Cell(import);
        var description = HtmlToMarkdown.ConvertExcerpt(e.Description, 150).Replace("\n", " ");
        return Markdown.Cell(description);
    }
}
