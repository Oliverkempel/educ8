using System.Text;
using System.Text.RegularExpressions;
using Educ8.Itslearning.Models;

namespace Educ8.Mcp.Formatting;

internal static partial class CourseFormatter
{
    /// <summary>Semester from a course code suffix, e.g. "T510046101-1-E26" → "E26" (E = autumn, F = spring).</summary>
    public static string? Semester(string? courseCode) => CourseCodes.Semester(courseCode);

    /// <summary>Courses sorted by most recent activity, optionally filtered by a case-insensitive text match.</summary>
    public static IReadOnlyList<PersonCourse> Select(IEnumerable<PersonCourse> courses, string? search) =>
        courses
            .Where(c => string.IsNullOrWhiteSpace(search)
                        || c.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || (c.CourseCode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                        || (c.FriendlyName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false))
            .OrderByDescending(c => c.LastUpdatedUtc ?? DateTimeOffset.MinValue)
            .ThenBy(c => c.Title, StringComparer.OrdinalIgnoreCase)
            .ToList();

    public static string FormatList(IReadOnlyList<PersonCourse> courses, int totalCount, string? search, int maxResults)
    {
        var sb = new StringBuilder();
        if (courses.Count == 0)
            return string.IsNullOrWhiteSpace(search)
                ? "You are not a member of any itslearning courses."
                : $"No courses match \"{search}\" (out of {totalCount}). Try a shorter search or omit it.";

        sb.Append(string.IsNullOrWhiteSpace(search)
            ? $"## Your itslearning courses ({courses.Count})\n\n"
            : $"## Courses matching \"{search}\" ({courses.Count} of {totalCount})\n\n");
        sb.Append("Sorted by most recent activity. Pass the course id to other tools. ");
        sb.Append("Semester: E = autumn, F = spring, plus the year (E26 = autumn 2026).\n\n");
        sb.Append("| Course id | Title | Code | Semester | Last activity | Teachers |\n|---|---|---|---|---|---|\n");
        foreach (var c in courses.Take(maxResults))
        {
            var teachers = c.TeachersInCourse.Select(t => t.DisplayName).Where(n => n.Length > 0).ToList();
            var teacherText = teachers.Count <= 3 ? string.Join(", ", teachers) : string.Join(", ", teachers.Take(3)) + $" +{teachers.Count - 3}";
            sb.Append($"| {c.CourseId} | {Markdown.Cell(c.Title)} | {Markdown.Cell(c.CourseCode)} | {Semester(c.CourseCode)} | {Markdown.Date(c.LastUpdatedUtc)} | {Markdown.Cell(teacherText)} |\n");
        }
        if (courses.Count > maxResults)
            sb.Append($"\n{courses.Count - maxResults} more not shown. Use `search` to narrow down, or raise `maxResults`.\n");
        return sb.ToString();
    }
}
