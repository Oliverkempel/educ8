using System.Net;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;

namespace Educ8.App.Core.Tests;

public class CourseViewModelTests
{
    private const string File72 = """{"ElementId":72,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"slides.pdf","CourseId":10,"Url":"https://school.itslearning.com/main.aspx?ElementID=72","Active":true}""";

    /// <summary>Course 10 with one item in each overview section and a two-level folder tree. Clock: Wed 2026-11-18 12:00 UTC.</summary>
    private static AppTestHost Host(Func<string, HttpResponseMessage?>? overrides = null)
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (req, _) => overrides?.Invoke(req.RequestUri!.AbsolutePath) ?? req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/courses/v4" => FakeItslearning.Json(AppTestHost.CoursesJson(AppTestHost.Course(10, "Statistics, (E26)", "T700075101-1-E26", "2026-11-18T08:00:00Z"))),
            "/restapi/personal/courses/10/bulletins/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"LightBulletinId":1,"Text":" Exam room changed to U55 ","PublishedDate":"2026-11-18T09:00:00Z","PublishedBy":{"PersonId":1,"FullName":"Ann Teacher"},"Pinned":true}],"Total":1}
                """),
            "/restapi/personal/planner/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"PlanId":5,"PlanName":"Lecture 10","PlanDescription":"<p>Regression, part 2</p>","Start":"2026-11-17T13:00:00Z","Stop":"2026-11-17T15:00:00Z","TopicId":0}],"Total":1}
                """),
            "/restapi/personal/courses/10/topics/v1" => FakeItslearning.Json("""{"EntityArray":[],"Total":0}"""),
            "/restapi/personal/course/10/plans/5/v2" => FakeItslearning.Json("""
                {"id":5,"title":"Lecture 10","elements":[{"id":72,"title":"slides.pdf","url":"/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=72","type":"LearningToolElement","learningToolId":5009,"isResource":true,"isActive":true,"isFolder":false}]}
                """),
            "/restapi/personal/courses/10/tasks/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"TaskId":1,"ElementId":80,"ElementType":"LearningToolElement","LearningToolId":5006,"Title":"Hand-in 3","Deadline":"2026-11-20T22:59:00Z","Url":"https://school.itslearning.com/main.aspx?ElementID=80"}],"Total":1}
                """),
            "/restapi/personal/courses/10/stream/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"NotificationId":1,"Text":"Ann added <b>slides.pdf</b>","PublishedDate":"2026-11-17T10:00:00Z","PublishedBy":{"FullName":"Ann Teacher"},"ElementId":72,"ElementType":"LearningToolElement","LearningToolId":5009,"Url":"https://school.itslearning.com/main.aspx?ElementID=72"}],"Total":1}
                """),
            "/restapi/personal/courses/10/resources/v1" => FakeItslearning.Json("""
                {"Resources":{"EntityArray":[{"ElementId":300,"ElementType":"Folder","LearningToolId":0,"Title":"Week 1","CourseId":10,"Active":true},FILE72,
                  {"ElementId":301,"ElementType":"Folder","LearningToolId":0,"Title":"Hidden","CourseId":10,"Active":false}],"Total":3}}
                """.Replace("FILE72", File72)),
            "/restapi/personal/courses/10/folders/300/resources/v1" => FakeItslearning.Json("""
                {"Resources":{"EntityArray":[{"ElementId":400,"ElementType":"LearningToolElement","LearningToolId":5,"Title":"Reading guide","CourseId":10,"Url":"https://school.itslearning.com/main.aspx?ElementID=400","Active":true}],"Total":1}}
                """),
            "/restapi/personal/course/10/plans/topics/v1" => FakeItslearning.Json("""[{"id":7,"name":"Course info","planCount":1},{"id":8,"name":"Unused","planCount":0}]"""),
            "/restapi/personal/course/10/plans/topics/7/all/v1" => FakeItslearning.Json("""
                {"entityArray":[{"id":9,"isActive":true,"title":"About the course","start":null,"stop":null,"orderNumber":1,"elements":[]}],"total":1,"currentPageIndex":0,"pageSize":100}
                """),
            "/restapi/personal/course/10/plans/topics/0/all/v1" => FakeItslearning.Json("""
                {"entityArray":[
                  {"id":5,"isActive":true,"title":"Lecture 10","start":"2026-11-17T13:00:00+00:00","stop":"2026-11-17T15:00:00+00:00","orderNumber":3,
                   "elements":[{"id":72,"title":"slides.pdf","url":"/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=72","type":"LearningToolElement","learningToolId":5009,"isActive":true}]},
                  {"id":6,"isActive":true,"title":" ","start":null,"orderNumber":4,"elements":null},
                  {"id":4,"isActive":true,"title":"Exam information","description":"<p>Oral exam</p>","start":null,"orderNumber":2,"elements":[]}
                ],"total":3,"currentPageIndex":0,"pageSize":100}
                """),
            "/restapi/personal/courses/10/folders/302/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[],"Total":0}}"""),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        return h;
    }

    private static async Task<CourseViewModel> OpenCourse(AppTestHost h)
    {
        await h.Courses.EnsureLoadedAsync();
        CourseViewModel? opened = null;
        h.Courses.CourseOpened += (_, c) => opened = c;
        h.Courses.OpenCourseCommand.Execute(h.Courses.Courses.Single());
        Assert.NotNull(opened);
        return opened;
    }

    [Fact]
    public async Task Opening_a_card_gives_its_course_page()
    {
        using var h = Host();
        var course = await OpenCourse(h);

        Assert.Equal((10, "Statistics, (E26)", "T700075101-1-E26"), (course.CourseId, course.Title, course.Subtitle));
        course.OpenInItslearningCommand.Execute(null);
        Assert.Equal("https://school.itslearning.com/main.aspx?CourseID=10", h.OpenUrl.Opened.Single().ToString());
    }

    [Fact]
    public async Task Overview_shows_announcements_plans_tasks_and_changes()
    {
        using var h = Host();
        var course = await OpenCourse(h);
        await course.EnsureLoadedAsync();

        var news = Assert.Single(course.Announcements.Items);
        Assert.Equal(("Exam room changed to U55", "Ann Teacher · Today, 09:00", true), (news.Text, news.MetaText, news.IsPinned));
        Assert.Equal("Hi all Week 3: bring a laptop.",
            new AnnouncementItemViewModel("Hi all\n\nWeek 3:   bring a laptop.\n", "", false).PreviewText);

        var plan = Assert.Single(course.Plans.Items);
        Assert.Equal(("Lecture 10", "Tue 17 Nov, 13:00–15:00", "Regression, part 2"), (plan.Name, plan.WhenText, plan.DescriptionText));
        Assert.Equal("https://school.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=72",
            Assert.Single(plan.Material).Url?.ToString());

        var task = Assert.Single(course.Tasks.Items);
        Assert.Equal(("Hand-in 3", "Assignment · Due Fri 20 Nov, 22:59"), (task.Title, task.SubtitleText));

        var change = Assert.Single(course.Changes.Items);
        Assert.Equal(("Ann added slides.pdf", "File · Ann Teacher · Yesterday, 10:00"), (change.Title, change.SubtitleText));
        Assert.True(change.OpensLocally);

        Assert.All(new string?[] { course.Announcements.EmptyText, course.Plans.EmptyText, course.Tasks.EmptyText, course.Changes.EmptyText }, Assert.Null);
        Assert.False(course.IsLoading);
    }

    [Fact]
    public async Task A_failing_section_shows_its_error_and_the_rest_still_loads()
    {
        using var h = Host(path => path switch
        {
            "/restapi/personal/courses/10/tasks/v1" => new HttpResponseMessage(HttpStatusCode.InternalServerError),
            "/restapi/personal/courses/10/bulletins/v1" => FakeItslearning.Json("""{"EntityArray":[],"Total":0}"""),
            _ => null,
        });
        var course = await OpenCourse(h);
        await course.EnsureLoadedAsync();

        Assert.Empty(course.Tasks.Items);
        Assert.Equal("itslearning returned an error (500). Try again in a moment.", course.Tasks.EmptyText);
        Assert.Equal("No announcements.", course.Announcements.EmptyText);
        Assert.Single(course.Plans.Items);
        Assert.Single(course.Changes.Items);
    }

    [Fact]
    public async Task Resources_browse_folders_as_pages_and_open_files()
    {
        using var h = Host();
        var course = await OpenCourse(h);
        var root = course.Resources;
        await root.EnsureLoadedAsync();

        // Inactive (hidden) items are left out.
        Assert.Equal(["Week 1", "slides.pdf"], root.Items.Select(i => i.Title));

        FolderViewModel? opened = null;
        root.FolderOpened += (_, f) => opened = f;
        root.OpenCommand.Execute(root.Items[0]);
        Assert.NotNull(opened);
        Assert.Equal((10, 300, "Week 1"), (opened.CourseId, opened.FolderId, opened.Title));
        await opened.EnsureLoadedAsync();
        Assert.Equal(("Reading guide", "Page"), (opened.Items.Single().Title, opened.Items.Single().SubtitleText));

        root.OpenCommand.Execute(root.Items[1]);
        await WaitFor(() => h.OpenFile.Opened.Count == 1);
        Assert.Equal([72], h.Files.Downloads);
    }

    [Fact]
    public async Task Plans_tab_lists_every_plan_in_course_order()
    {
        using var h = Host();
        var course = await OpenCourse(h);
        var plans = course.AllPlans;
        Assert.Null(plans.EmptyText); // loads when the tab is shown
        await plans.EnsureLoadedAsync();

        // One list in course order across topics; the untitled empty placeholder plan is left out; topics without plans skipped.
        Assert.Equal([("About the course", "Course info", "No date", ""), ("Exam information", "", "No date", "Oral exam"),
                ("Lecture 10", "", "Tue 17 Nov, 13:00–15:00", "")],
            plans.Plans.Select(p => (p.Name, p.TopicText, p.WhenText, p.DescriptionText)));
        Assert.Equal("slides.pdf", Assert.Single(plans.Plans[2].Material).Title);
        Assert.Equal("No material found for this lesson.", plans.Plans[0].EmptyText);

        // A folder in a plan opens as an in-app page; a file opens in the default app.
        FolderViewModel? opened = null;
        course.FolderOpened += (_, f) => opened = f;
        course.OpenItemCommand.Execute(new MaterialItemViewModel(300, "Week 1", Itslearning.Models.ResourceKind.Folder, null, h.OpenUrl,
            (Services.MaterialFiles)h.Provider.GetService(typeof(Services.MaterialFiles))!));
        Assert.Equal((10, 300, "Week 1"), (opened!.CourseId, opened.FolderId, opened.Title));
        course.OpenItemCommand.Execute(plans.Plans[2].Material[0]);
        await WaitFor(() => h.OpenFile.Opened.Count == 1);
        Assert.Null(plans.EmptyText);
    }

    [Fact]
    public async Task An_empty_folder_says_so()
    {
        using var h = Host();
        var course = await OpenCourse(h);
        FolderViewModel? opened = null;
        course.Resources.FolderOpened += (_, f) => opened = f;
        course.Resources.OpenCommand.Execute(new MaterialItemViewModel(302, "Empty", Itslearning.Models.ResourceKind.Folder, null, h.OpenUrl,
            (Services.MaterialFiles)h.Provider.GetService(typeof(Services.MaterialFiles))!));

        Assert.Null(opened!.EmptyText); // not loaded yet
        await opened.EnsureLoadedAsync();
        Assert.Equal("This folder is empty.", opened.EmptyText);
    }

    private static async Task WaitFor(Func<bool> condition)
    {
        for (var i = 0; i < 100 && !condition(); i++) await Task.Delay(20);
        Assert.True(condition());
    }
}
