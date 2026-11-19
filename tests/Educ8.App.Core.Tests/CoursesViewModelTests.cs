using System.Net;
using Educ8.Itslearning.Tests;

namespace Educ8.App.Core.Tests;

public class CoursesViewModelTests
{
    private static AppTestHost Host(string coursesJson)
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath == "/restapi/personal/courses/v4"
            ? FakeItslearning.Json(coursesJson)
            : new HttpResponseMessage(HttpStatusCode.NotFound);
        return h;
    }

    private static readonly string Data = AppTestHost.CoursesJson(
        AppTestHost.Course(1, "Web Development, (E26)", "T510069101-1-E26", "2026-11-18T08:00:00Z", notifications: 3, bulletins: 1,
            teachers: string.Join(',', AppTestHost.Teacher("Ann", "A"), AppTestHost.Teacher("Bo", "B"), AppTestHost.Teacher("Cy", "C"), AppTestHost.Teacher("Di", "D"))),
        AppTestHost.Course(2, "Operating systems, (E26)", "T510046101-1-E26", "2026-11-17T08:00:00Z", teachers: AppTestHost.Teacher("Leo", "L")),
        AppTestHost.Course(3, "Data Management, (F26)", "T510051101-1-F26", "2026-05-04T08:00:00Z"),
        AppTestHost.Course(4, "Software Uddannelsen Odense", "IA-TEK-00000011", "2026-11-13T08:00:00Z"),
        AppTestHost.Course(5, "SDU Bibliotek", "L000001M01-M-F21x", "2025-01-02T08:00:00Z"));

    [Fact]
    public async Task Current_semester_by_default_sorted_by_activity()
    {
        using var h = Host(Data);
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();

        // E26 courses + a recently active course without semester code; not F26, not the old one.
        Assert.Equal([1, 2, 4], vm.Courses.Select(c => c.CourseId));
        Assert.Equal("3 courses", vm.CountText);
        Assert.False(vm.IsEmpty);

        vm.ShowAll = true;
        Assert.Equal([1, 2, 4, 3, 5], vm.Courses.Select(c => c.CourseId));
    }

    [Fact]
    public async Task Course_with_null_lists_from_the_api_still_gets_a_card()
    {
        // itslearning sends "TeachersInCourse": null for some (older) courses; showing all courses used to crash on it.
        var old = AppTestHost.Course(6, "Old course", "X-F20", "2020-03-01T08:00:00Z").Replace("\"TeachersInCourse\":[]", "\"TeachersInCourse\":null");
        using var h = Host(AppTestHost.CoursesJson(old));
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();
        vm.ShowAll = true;

        var card = Assert.Single(vm.Courses);
        Assert.Equal("", card.TeachersText);
    }

    [Fact]
    public async Task Card_texts_are_ready_to_show()
    {
        using var h = Host(Data);
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();
        vm.ShowAll = true;

        var web = vm.Courses[0];
        Assert.Equal("Semester E26", web.SemesterText);
        Assert.Equal("Teachers: Ann A, Bo B and 2 more", web.TeachersText);
        Assert.Equal("Updated today", web.LastActivityText);
        Assert.Equal("3 new · 1 announcement", web.BadgeText);
        Assert.True(web.HasBadges);
        Assert.Equal("#408AB4", web.AccentColor);

        Assert.Equal("Updated yesterday", vm.Courses[1].LastActivityText);
        Assert.Equal("Teachers: Leo L", vm.Courses[1].TeachersText);
        Assert.Equal("Updated 5 days ago", vm.Courses[2].LastActivityText);
        Assert.Equal("Updated 4 May 2026", vm.Courses[3].LastActivityText);
        Assert.Equal("", vm.Courses[2].SemesterText);

        web.OpenInItslearningCommand.Execute(null);
        Assert.Equal("https://sdu.itslearning.com/main.aspx?CourseID=1", h.OpenUrl.Opened.Single().ToString());
    }

    [Fact]
    public async Task Search_filters_and_explains_no_match()
    {
        using var h = Host(Data);
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();

        vm.SearchText = "operating";
        Assert.Equal([2], vm.Courses.Select(c => c.CourseId));

        vm.SearchText = "T510069";
        Assert.Equal([1], vm.Courses.Select(c => c.CourseId));

        vm.SearchText = "chemistry";
        Assert.Empty(vm.Courses);
        Assert.Equal("No courses match \"chemistry\".", vm.EmptyText);
        Assert.True(vm.IsEmpty);
    }

    [Fact]
    public async Task No_current_courses_suggests_showing_all()
    {
        using var h = Host(AppTestHost.CoursesJson(AppTestHost.Course(3, "Data Management, (F26)", "T1-1-F26", "2026-05-04T08:00:00Z")));
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();
        Assert.Equal("No courses this semester. Show all courses to see older ones.", vm.EmptyText);
    }

    [Fact]
    public async Task Server_error_becomes_a_message()
    {
        using var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (_, _) => new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var vm = h.Courses;

        await vm.EnsureLoadedAsync();

        Assert.Equal("itslearning returned an error (500). Try again in a moment.", vm.ErrorMessage);
        Assert.False(vm.IsLoading);
    }
}
