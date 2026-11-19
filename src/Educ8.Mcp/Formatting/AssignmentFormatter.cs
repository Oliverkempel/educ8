using System.Text;
using Educ8.Documents;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Models;

namespace Educ8.Mcp.Formatting;

/// <summary>Everything known about one assignment, gathered from REST + the assignment web view.</summary>
internal sealed record AssignmentInfo(
    CourseResourceWithCourse? Element,
    AssignmentDetails Details,
    AssignmentSettings? Settings,
    AssignmentAnswer? Answer,
    IReadOnlyList<AssignmentFile> FeedbackFiles,
    AssignmentView? View,
    IReadOnlyList<string> Problems);

internal static class AssignmentFormatter
{
    public static string Format(AssignmentInfo a, TimeZoneInfo zone, DateTimeOffset now)
    {
        var d = a.Details;
        var sb = new StringBuilder();
        sb.Append($"# {d.Title}\n\n");
        sb.Append($"Assignment · element {d.ElementId}");
        if (a.Element is { } e)
        {
            sb.Append($" · {e.CourseTitle} (course {e.CourseId})");
            var path = ResourceFormatter.CleanPath(e.Path);
            if (path.Length > 0) sb.Append($" · location: {path}");
            if (!string.IsNullOrEmpty(e.Url)) sb.Append($"\nOpen in itslearning: {e.Url}");
        }
        sb.Append("\n\n");

        // Key facts
        var facts = new List<string>();
        if (d.Deadline is { } deadline)
        {
            var local = TimeZoneInfo.ConvertTime(deadline, zone);
            var left = deadline - now;
            var relative = left > TimeSpan.Zero
                ? $"in {Human(left)}"
                : $"passed {Human(-left)} ago";
            facts.Add($"**Deadline:** {local:ddd yyyy-MM-dd HH:mm} ({zone.Id}, {relative}){(d.IsSubmissionAfterDeadlineAllowed ? "; late submissions allowed" : "; no late submissions")}");
        }
        else facts.Add("**Deadline:** none set");
        if (d.ActiveFrom is { } from && from > now) facts.Add($"**Opens:** {TimeZoneInfo.ConvertTime(from, zone):yyyy-MM-dd HH:mm}");
        if (!string.IsNullOrEmpty(d.SubmissionType)) facts.Add($"**Submission:** {d.SubmissionType}");
        if (d.MaxScore is { } max) facts.Add($"**Max score:** {max:0.##}");
        if (d.IsMandatory) facts.Add("**Mandatory**");
        if (d.IsAnonymous) facts.Add("**Anonymous**");
        if (d.IsSelfAssessmentRequired) facts.Add("**Self-assessment required**");
        if (d.PeersToAssessCount is > 0) facts.Add($"**Peer assessment:** assess {d.PeersToAssessCount} peers");
        if (a.View?.Status is { } status) facts.Add($"**Your status:** {status}");
        sb.Append(string.Join("  \n", facts)).Append("\n\n");

        // Description
        sb.Append("## Description\n\n");
        var description = HtmlToMarkdown.Convert(d.RawText);
        sb.Append(description.Length > 0 ? description : "_No description._").Append("\n\n");

        // Teacher's files
        var teacherFiles = d.Files.ToList();
        if (d.TemplateFile is { } template) teacherFiles.Add(template);
        if (teacherFiles.Count > 0)
        {
            sb.Append("## Files from the teacher\n\n");
            foreach (var f in d.Files) sb.Append(FileLine(f, null));
            if (d.TemplateFile is { } t) sb.Append(FileLine(t, "template to fill in"));
            sb.Append('\n');
        }

        // Grading scale
        if (a.Settings?.AssessmentScale is { } scale && scale.AssessmentItems.Count > 0)
        {
            sb.Append($"## Grading scale{(string.IsNullOrWhiteSpace(scale.Title) ? "" : $": {scale.Title}")}\n\n");
            foreach (var item in scale.AssessmentItems)
            {
                sb.Append($"- {item.Title}");
                if (item.PercentFromAndIncl is not null || item.PercentTo is not null) sb.Append($" ({item.PercentFromAndIncl:0.##}–{item.PercentTo:0.##}%)");
                if (!string.IsNullOrWhiteSpace(item.Description)) sb.Append($": {item.Description}");
                sb.Append('\n');
            }
            sb.Append('\n');
        }

        // The student's submission
        sb.Append("## Your submission\n\n");
        if (a.Answer is { } answer)
        {
            sb.Append(a.View?.SubmittedLine is { } line ? line : answer.Submitted is { } s ? $"Submitted {TimeZoneInfo.ConvertTime(s, zone):yyyy-MM-dd HH:mm}" : "Not submitted yet");
            if (answer.IsDraft) sb.Append(" (draft, not handed in)");
            sb.Append("\n\n");
            var text = HtmlToMarkdown.Convert(answer.Text);
            if (text.Length > 0) sb.Append(text).Append("\n\n");
            foreach (var f in answer.Files) sb.Append(FileLine(f, "your file"));
            if (answer.Files.Count > 0) sb.Append('\n');
        }
        else sb.Append(a.View?.Status is { } st ? $"{st}.\n\n" : "Nothing submitted yet.\n\n");

        // Assessment and feedback
        if (a.View is { HasAssessment: true } view || a.FeedbackFiles.Count > 0)
        {
            sb.Append("## Assessment and feedback\n\n");
            if (a.View?.Score is { } score) sb.Append($"**Score:** {score}\n\n");
            if (a.View?.AssessedBy is { } by) sb.Append(by).Append("\n\n");
            var feedback = HtmlToMarkdown.Convert(a.View?.FeedbackHtml);
            if (feedback.Length > 0) sb.Append(feedback).Append("\n\n");
            foreach (var f in a.FeedbackFiles) sb.Append(FileLine(f, "feedback file"));
            if (a.FeedbackFiles.Count > 0) sb.Append('\n');
        }
        else if (a.Answer is not null && d.IsAssessmentVisibleForStudents)
            sb.Append("## Assessment and feedback\n\nNot assessed yet (or the assessment isn't visible yet).\n\n");

        if (teacherFiles.Count > 0 || a.Answer?.Files.Count > 0 || a.FeedbackFiles.Count > 0)
            sb.Append("Read any file listed above with `read_assignment_file` (elementId and fileId).\n");
        foreach (var problem in a.Problems) sb.Append($"\nNote: {problem}");
        return sb.ToString().TrimEnd();
    }

    private static string FileLine(AssignmentFile f, string? role) =>
        $"- {Markdown.Cell(f.Name)} ({DocumentExtractor.Size(f.Size)}{(role is null ? "" : $", {role}")}) · fileId {f.FileId}\n";

    private static string Human(TimeSpan t) => t.TotalDays >= 2 ? $"{(int)t.TotalDays} days"
        : t.TotalHours >= 2 ? $"{(int)t.TotalHours} hours"
        : $"{Math.Max(1, (int)t.TotalMinutes)} minutes";

    /// <summary>Task list grouped by course, current courses first.</summary>
    public static string FormatTasks(IReadOnlyList<CourseTask> tasks, IReadOnlyList<PersonCourse> courses, string status, int maxResults, TimeZoneInfo zone)
    {
        if (tasks.Count == 0) return status == "Completed" ? "No completed tasks." : "No open tasks. 🎉";
        var courseOrder = courses.Select((c, i) => (c.CourseId, i)).ToDictionary(x => x.CourseId, x => x.i);
        var groups = tasks
            .GroupBy(t => (t.LocationId, t.LocationTitle))
            .OrderBy(g => courseOrder.GetValueOrDefault(g.Key.LocationId, int.MaxValue))
            .ToList();

        var sb = new StringBuilder($"## {(status == "Completed" ? "Completed" : "Open")} tasks ({tasks.Count})\n\n");
        sb.Append("Grouped by course, most recently active course first. Assignments: `get_assignment`; other items: `read_material`.\n");
        var shown = 0;
        foreach (var g in groups)
        {
            if (shown >= maxResults) break;
            var title = Markdown.Cell(g.Key.LocationTitle);
            sb.Append($"\n### {(title.Length > 0 ? title : "Unknown course")} (course {g.Key.LocationId})\n\n");
            sb.Append("| Element id | Type | Title | Deadline | Status |\n|---|---|---|---|---|\n");
            // Graded work first (assignments, tests), then surveys and teacher activities.
            var byKind = g.OrderBy(t => t.Kind switch { ResourceKind.Assignment => 0, ResourceKind.Test => 1, ResourceKind.Survey => 2, _ => 3 });
            var ordered = status == "Completed"
                ? byKind.ThenByDescending(t => t.Deadline ?? DateTimeOffset.MinValue)
                : byKind.ThenBy(t => t.Deadline ?? DateTimeOffset.MaxValue);
            foreach (var t in ordered)
            {
                if (shown++ >= maxResults) break;
                var deadline = t.Deadline is { } dl ? TimeZoneInfo.ConvertTime(dl, zone).ToString("yyyy-MM-dd HH:mm") : "";
                sb.Append($"| {t.ElementId} | {ResourceFormatter.KindLabel(t.Kind, t.LearningToolId)} | {Markdown.Cell(t.Title)}{(t.Homework ? " (homework)" : "")} | {deadline} | {t.Status} |\n");
            }
        }
        if (tasks.Count > maxResults) sb.Append($"\n{tasks.Count - maxResults} more not shown: pass a courseId, or raise maxResults.\n");
        return sb.ToString();
    }
}
