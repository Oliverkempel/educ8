using Educ8.Itslearning.Models;
using Educ8.Mcp.Formatting;

namespace Educ8.Mcp.Tests;

public class CourseFormatterTests
{
    private static PersonCourse Course(int id, string title, string? code, string? updated) => new()
    {
        CourseId = id, Title = title, CourseCode = code,
        LastUpdatedUtc = updated is null ? null : DateTimeOffset.Parse(updated),
    };

    [Theory]
    [InlineData("T510046101-1-E26", "E26")]
    [InlineData("T000207101-M-F26", "F26")]
    [InlineData("IA-TEK-00000011", null)]
    [InlineData(null, null)]
    public void Extracts_semester(string? code, string? expected) => Assert.Equal(expected, CourseFormatter.Semester(code));

    [Fact]
    public void Sorts_by_recent_activity_and_filters()
    {
        var courses = new[]
        {
            Course(1, "Old course", "A-1-E24", "2024-10-01T00:00:00Z"),
            Course(2, "Operating systems and distributed systems", "T510046101-1-E26", "2026-11-18T11:00:00Z"),
            Course(3, "Web Development", "T510069101-1-E26", "2026-11-18T17:00:00Z"),
            Course(4, "No date", null, null),
        };

        Assert.Equal([3, 2, 1, 4], CourseFormatter.Select(courses, null).Select(c => c.CourseId));
        Assert.Equal([2], CourseFormatter.Select(courses, "DISTRIBUTED").Select(c => c.CourseId));
        Assert.Equal([2], CourseFormatter.Select(courses, "T510046").Select(c => c.CourseId));
    }

    [Fact]
    public void Formats_table_with_escaping_and_truncation()
    {
        var courses = new List<PersonCourse>
        {
            Course(10, "A | B", "X-1-E26", "2026-11-18T00:00:00Z") with
            {
                TeachersInCourse = [new() { FirstName = "T1" }, new() { FirstName = "T2" }, new() { FirstName = "T3" }, new() { FirstName = "T4" }],
            },
            Course(11, "Second", null, null),
        };

        var text = CourseFormatter.FormatList(courses, totalCount: 2, search: null, maxResults: 1);

        Assert.Contains("| 10 | A \\| B | X-1-E26 | E26 | 2026-11-18 | T1, T2, T3 +1 |", text);
        Assert.DoesNotContain("| 11 |", text);
        Assert.Contains("1 more not shown", text);
    }

    [Fact]
    public void Explains_empty_search() =>
        Assert.Contains("No courses match \"zzz\"", CourseFormatter.FormatList([], 5, "zzz", 50));
}
