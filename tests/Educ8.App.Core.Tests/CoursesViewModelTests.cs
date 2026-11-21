using System.Net;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;

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
        AppTestHost.Course(4, "Software Programme", "IA-TEK-00000011", "2026-11-13T08:00:00Z"),
        AppTestHost.Course(5, "University Library", "L000001M01-M-F21x", "2025-01-02T08:00:00Z"));

    [Fact]
    public async Task Cards_start_collapsed_and_expanding_shows_todays_plans_and_teachers()
    {
        using var h = Host(Data);
        var inner = h.T.Http.Respond;
        string? plannerQuery = null;
        h.T.Http.Respond = (req, ct) =>
        {
            switch (req.RequestUri!.AbsolutePath)
            {
                case "/restapi/personal/planner/v1":
                    plannerQuery = req.RequestUri.Query;
                    // Today is Wed 18 Nov (UTC). Only "Lecture 11" (today) and "Project week" (a period including today) run today.
                    return FakeItslearning.Json("""
                        {"EntityArray":[
                          {"PlanId":5,"PlanName":"Lecture 10","Start":"2026-11-17T13:00:00Z","Stop":"2026-11-17T15:00:00Z","TopicId":0},
                          {"PlanId":9,"PlanName":"All of Tuesday","Start":"2026-11-17T00:00:00Z","Stop":"2026-11-18T00:00:00Z","TopicId":0},
                          {"PlanId":6,"PlanName":"Lecture 11","PlanDescription":"<p>Scheduling</p>","Start":"2026-11-18T13:00:00Z","Stop":"2026-11-18T15:00:00Z","TopicId":0},
                          {"PlanId":7,"PlanName":"Project week","Start":"2026-11-16T00:00:00Z","Stop":"2026-11-22T00:00:00Z","TopicId":0},
                          {"PlanId":8,"PlanName":"Undated","TopicId":0}],"Total":5}
                        """);
                case "/restapi/personal/course/1/plans/6/v2":
                    return FakeItslearning.Json("""
                        {"id":6,"title":"Lecture 11","elements":[{"id":90,"title":"Week 47","type":"Folder","isResource":true,"isActive":true,"isFolder":true}]}
                        """);
                case "/restapi/personal/course/1/plans/7/v2":
                    return FakeItslearning.Json("""{"id":7,"title":"Project week","elements":[]}""");
                default:
                    return inner(req, ct);
            }
        };
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();
        var card = vm.Courses[0];

        Assert.False(card.IsExpanded);
        Assert.Null(plannerQuery); // nothing loads until a card is expanded
        Assert.Equal("Ann A, Bo B, Cy C, Di D", card.TeacherNames);
        Assert.True(card.HasTeachers);

        card.IsExpanded = true;
        await Until(() => !card.ActivePlans.IsLoading);
        Assert.Contains("startDate=2026-11-18", plannerQuery);
        Assert.Contains("stopDate=2026-11-19", plannerQuery);
        Assert.Equal(["Project week", "Lecture 11"], card.ActivePlans.Items.Select(p => p.Name)); // by start
        var lecture = card.ActivePlans.Items[1];
        Assert.Equal("Week 47", Assert.Single(lecture.Material).Title);
        Assert.Null(card.ActivePlans.EmptyText);

        // Searching keeps the card (and its state); clicking a plan opens it on its own page.
        vm.SearchText = "web";
        Assert.Same(card, Assert.Single(vm.Courses));
        Assert.True(card.IsExpanded);
        PlanPageViewModel? page = null;
        vm.PlanOpened += (_, p) => page = p;
        card.OpenPlanCommand.Execute(lecture);
        Assert.NotNull(page);
        Assert.Equal(("Lecture 11", "Web Development, (E26)", 1), (page.Plan.Name, page.CourseTitle, page.CourseId));

        // A folder in the plan opens as a folder page; "Open course" opens the course page.
        FolderViewModel? folder = null;
        page.FolderOpened += (_, f) => folder = f;
        page.OpenItemCommand.Execute(page.Plan.Material[0]);
        Assert.Equal((1, (int?)90, "Week 47"), (folder!.CourseId, folder.FolderId, folder.Title));
        CourseViewModel? course = null;
        vm.CourseOpened += (_, c) => course = c;
        page.OpenCourseCommand.Execute(null);
        Assert.Equal(1, course?.CourseId);
    }

    [Fact]
    public async Task Course_without_current_plans_says_so()
    {
        using var h = Host(Data);
        var inner = h.T.Http.Respond;
        h.T.Http.Respond = (req, ct) => req.RequestUri!.AbsolutePath == "/restapi/personal/planner/v1"
            ? FakeItslearning.Json("""{"EntityArray":[],"Total":0}""")
            : inner(req, ct);
        var vm = h.Courses;
        await vm.EnsureLoadedAsync();
        var card = vm.Courses[1];
        card.IsExpanded = true;
        await Until(() => !card.ActivePlans.IsLoading);
        Assert.Equal("No current plans", card.ActivePlans.EmptyText);
    }

    [Fact]
    public void Layout_defaults_to_grid_and_is_remembered()
    {
        using var h = Host(Data);
        Assert.Equal(Services.CoursesLayout.Grid, h.Courses.Layout);
        h.Courses.Layout = Services.CoursesLayout.List;

        var later = new CoursesViewModel(h.Provider.GetRequiredService<Presentation.CourseCatalog>(), TimeProvider.System, h.OpenUrl,
            h.Provider.GetRequiredService<CoursePageFactory>(), h.Provider.GetRequiredService<Services.AppPreferences>());
        Assert.Equal(Services.CoursesLayout.List, later.Layout);
    }

    private static async Task Until(Func<bool> condition)
    {
        for (var i = 0; i < 200 && !condition(); i++) await Task.Delay(10);
        Assert.True(condition());
    }

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
        Assert.Equal("https://school.itslearning.com/main.aspx?CourseID=1", h.OpenUrl.Opened.Single().ToString());
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
