using Educ8.Itslearning.Models;

namespace Educ8.Itslearning.Tests;

public class CourseCodesTests
{
    [Theory]
    [InlineData("2026-11-19", "E26")]
    [InlineData("2026-08-01", "E26")]
    [InlineData("2027-01-15", "E26")] // January still counts to the autumn semester
    [InlineData("2027-02-01", "F27")]
    [InlineData("2026-07-31", "F26")]
    public void Current_semester(string date, string expected) =>
        Assert.Equal(expected, CourseCodes.CurrentSemester(DateOnly.Parse(date)));

    [Theory]
    [InlineData("T510046101-1-E26", "E26")]
    [InlineData("IA-TEK-00000011", null)]
    public void Semester_from_code(string code, string? expected) => Assert.Equal(expected, CourseCodes.Semester(code));
}
