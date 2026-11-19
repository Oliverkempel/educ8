using System.Text;
using Educ8.Documents;
using Educ8.Itslearning.Models;

namespace Educ8.Mcp.Formatting;

/// <summary>Formats the "what's new" feeds: personal notifications (bell) and the latest-changes stream.</summary>
internal static class UpdatesFormatter
{
    private const int MaxGroupItems = 8;

    public static string Format(
        IReadOnlyList<Notification>? personal, IReadOnlyList<StreamItem> stream, IReadOnlyDictionary<int, List<ElementLink>> groups,
        int days, string? courseTitle, TimeZoneInfo zone, IReadOnlyList<string> notes)
    {
        var sb = new StringBuilder();
        sb.Append(courseTitle is null ? $"## What's new in the last {days} days\n\n" : $"## What's new in {courseTitle} in the last {days} days\n\n");
        sb.Append($"Times in {zone.Id}. Use the element ids with the tool named after each item.\n");

        if (personal is not null)
        {
            sb.Append("\n### For you\n\n");
            if (personal.Count == 0) sb.Append("No personal notifications.\n");
            foreach (var n in personal)
            {
                var when = n.PublishedDate is { } d ? TimeFormat.Local(d, zone).ToString("ddd yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture) : "?";
                sb.Append($"- {when} · {Markdown.Cell(n.Text)}");
                if (!n.IsRead) sb.Append(" **(unread)**");
                if (n.ElementId is { } id) sb.Append($" · element {id} · read_material");
                sb.Append('\n');
            }
        }

        sb.Append("\n### Course updates\n");
        if (stream.Count == 0) sb.Append("\nNo course updates.\n");
        foreach (var day in stream.GroupBy(s => s.PublishedDate is { } d ? TimeFormat.Local(d, zone).Date : DateTime.MinValue))
        {
            sb.Append($"\n#### {(day.Key == DateTime.MinValue ? "Undated" : TimeFormat.Day(day.Key))}\n\n");
            foreach (var item in day)
            {
                sb.Append(StreamLine(item, zone, withCourse: courseTitle is null, withDate: false));
                if (groups.TryGetValue(item.NotificationId, out var elements))
                {
                    foreach (var e in elements.Take(MaxGroupItems))
                        sb.Append($"  - {ResourceFormatter.KindLabel(e.Kind, e.LearningToolId)}: {Markdown.Cell(e.Title)}{(e.Homework ? " (homework)" : "")} · element {e.ElementId} · {ResourceFormatter.NextStep(e.Kind)}\n");
                    if (elements.Count > MaxGroupItems) sb.Append($"  - … {elements.Count - MaxGroupItems} more\n");
                }
            }
        }

        foreach (var note in notes) sb.Append($"\nNote: {note}");
        return sb.ToString().TrimEnd();
    }

    /// <summary>One stream item as a Markdown list line (announcements include their text).</summary>
    public static string StreamLine(StreamItem item, TimeZoneInfo zone, bool withCourse, bool withDate)
    {
        var sb = new StringBuilder("- ");
        if (item.PublishedDate is { } d)
            sb.Append(TimeFormat.Local(d, zone).ToString(withDate ? "ddd yyyy-MM-dd HH:mm" : "HH:mm", System.Globalization.CultureInfo.InvariantCulture)).Append(" · ");
        if (withCourse && !string.IsNullOrWhiteSpace(item.LocationTitle)) sb.Append($"{Markdown.Cell(item.LocationTitle)} (course {item.LocationId}) · ");

        if (item.LightBulletin is { } bulletin)
        {
            sb.Append($"**Announcement** by {Markdown.Cell(bulletin.PublishedBy?.FullName ?? item.PublishedBy?.FullName)}: ");
            sb.Append(HtmlToMarkdown.Truncate(string.Join(' ', bulletin.Text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)), 400));
            if (bulletin.CommentsCount > 0) sb.Append($" ({bulletin.CommentsCount} comments)");
        }
        else
        {
            sb.Append(Markdown.Cell(item.Text));
            if (item.ElementId > 0)
                sb.Append($" · {ResourceFormatter.KindLabel(item.Kind, item.LearningToolId)} · element {item.ElementId} · {ResourceFormatter.NextStep(item.Kind)}");
            else if (item.IsGroup)
                sb.Append($" ({item.ElementsCount} items)");
        }
        return sb.Append('\n').ToString();
    }
}
