using System.Net;
using Educ8.Itslearning.Tests;

namespace Educ8.App.Core.Tests;

public class CalendarViewModelTests
{
    private static string Ev(int id, string from, string to, string title, int course) => CalendarAndUpdatesTests.Event(id, from, to, title, course);

    private static AppTestHost Host()
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/calendar/events/v1" => FakeItslearning.Json($$"""
                {"EntityArray":[{{Ev(1, "2026-11-16T07:00:00Z", "2026-11-16T15:00:00Z", "Project work", 30)}},
                                {{Ev(2, "2026-11-18T13:00:00Z", "2026-11-18T15:00:00Z", "Lecture - Campus U150", 10)}},
                                {{Ev(3, "2026-11-18T08:00:00Z", "2026-11-18T10:00:00Z", "Exercises", 20)}},
                                {{Ev(4, "2026-11-24T08:00:00Z", "2026-11-24T10:00:00Z", "Next week lecture", 10)}}],"Total":4}
                """),
            "/restapi/personal/courses/v4" => FakeItslearning.Json(AppTestHost.CoursesJson(
                AppTestHost.Course(10, "OSDS", "T1-1-E26", "2026-11-18T08:00:00Z"),
                AppTestHost.Course(20, "Web", "T2-1-E26", "2026-11-18T08:00:00Z"),
                AppTestHost.Course(30, "Semester project", "T3-1-E26", "2026-11-16T08:00:00Z"))),
            "/restapi/personal/calendar/events/2/v1" => FakeItslearning.Json("""
                {"EventUrl":"https://school.itslearning.com/Course/Course.aspx?CourseID=10&EventId=2","Bookings":[],"ElementLinks":[],
                 "PlanLinks":[{"PlanId":501,"PlanName":"Lecture","TopicName":"11: Web"}],
                 "EventId":2,"FromDate":"2026-11-18T13:00:00Z","ToDate":"2026-11-18T15:00:00Z","EventTitle":"Lecture - Campus U150",
                 "Description":"<p>Bring a <b>laptop</b></p>","ImportDescription":"Campus U150 Teacher","EventType":"Course","LocationId":10,"LocationTitle":"OSDS"}
                """),
            "/restapi/personal/planner/v1" => FakeItslearning.Json("""{"EntityArray":[{"PlanId":501,"PlanName":"Lecture","PlanDescription":"<p>Read chapter 7</p>","TopicId":7}],"Total":1}"""),
            "/restapi/personal/courses/10/topics/v1" => FakeItslearning.Json("""{"EntityArray":[{"TopicId":7,"TopicName":"11: Web","FolderId":70}],"Total":1}"""),
            "/restapi/personal/courses/10/folders/70/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[{"ElementId":71,"ElementType":"Folder","Title":"Lecture","CourseId":10}],"Total":1}}"""),
            "/restapi/personal/courses/10/folders/71/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[{"ElementId":72,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"slides.pdf","CourseId":10,"Url":"https://school.itslearning.com/main.aspx?ElementID=72"}],"Total":1}}"""),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        return h;
    }

    [Fact]
    public async Task Week_agenda_from_monday_with_events_per_day()
    {
        using var h = Host();
        var vm = h.Calendar;
        await vm.EnsureLoadedAsync();

        Assert.Null(vm.ErrorMessage);
        Assert.Equal("Week 47", vm.WeekTitle);
        Assert.Equal("16 – 22 November 2026", vm.WeekRangeText);
        Assert.True(vm.IsCurrentWeek);
        Assert.Equal(["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"], vm.Days.Select(d => d.DayName));

        var wednesday = vm.Days[2];
        Assert.True(wednesday.IsToday);
        Assert.True(vm.Days[1].IsPast);
        Assert.Equal("18 November", wednesday.DateText);
        Assert.Equal(["Exercises", "Lecture - Campus U150"], wednesday.Events.Select(e => e.Title)); // sorted by start
        var lecture = wednesday.Events[1];
        Assert.Equal("13:00–15:00", lecture.TimeText);
        Assert.Equal("Room U1 Teacher", lecture.Subtitle);
        Assert.Equal("#408AB4", lecture.AccentColor);

        Assert.Equal("Project work", Assert.Single(vm.Days[0].Events).Title);
        Assert.False(vm.Days[3].HasEvents);
        Assert.Equal("No events", vm.Days[3].NoEventsText);
        Assert.DoesNotContain(vm.Days.SelectMany(d => d.Events), e => e.Title == "Next week lecture");
    }

    [Fact]
    public async Task Navigating_weeks_across_month_and_year()
    {
        using var h = Host();
        var vm = h.Calendar;
        await vm.EnsureLoadedAsync();

        await vm.NextWeekCommand.ExecuteAsync(null);
        Assert.Equal("Week 48", vm.WeekTitle);
        Assert.False(vm.IsCurrentWeek);
        Assert.Equal("Next week lecture", Assert.Single(vm.Days[1].Events).Title);

        await vm.NextWeekCommand.ExecuteAsync(null);
        Assert.Equal("30 November – 6 December 2026", vm.WeekRangeText);

        await vm.GoToDateCommand.ExecuteAsync(new DateOnly(2026, 12, 31));
        Assert.Equal("Week 53", vm.WeekTitle);
        Assert.Equal("28 December 2026 – 3 January 2027", vm.WeekRangeText);
        Assert.Equal("Nothing in the calendar this week.", vm.EmptyText);

        await vm.TodayCommand.ExecuteAsync(null);
        Assert.True(vm.IsCurrentWeek);
    }

    [Fact]
    public async Task Course_filter_lists_courses_with_events_and_filters_days()
    {
        using var h = Host();
        var vm = h.Calendar;
        await vm.EnsureLoadedAsync();

        Assert.Equal(["All courses", "OSDS", "Semester project", "Web"], vm.CourseFilters.Select(f => f.Title));

        vm.SelectedCourseFilter = vm.CourseFilters.Single(f => f.Title == "Semester project");
        Assert.Equal(["Project work"], vm.Days.SelectMany(d => d.Events).Select(e => e.Title));
    }

    [Fact]
    public async Task A_file_that_cannot_be_downloaded_opens_in_itslearning_instead()
    {
        using var h = Host();
        h.Files.Fail = new Educ8.Itslearning.Content.ContentUnavailableException("The file page has no download link.");
        var vm = h.Calendar;
        await vm.EnsureLoadedAsync();
        vm.SelectedEvent = vm.Days[2].Events.Single(e => e.EventId == 2);
        await WaitFor(() => vm.Details.Plans.Count > 0 && !vm.Details.IsLoading);

        var file = vm.Details.Plans.Single().Material.Single();
        await file.OpenCommand.ExecuteAsync(null);

        Assert.Empty(h.OpenFile.Opened);
        Assert.Equal(["https://school.itslearning.com/main.aspx?ElementID=72"], h.OpenUrl.Opened.Select(u => u.ToString()));
        Assert.Equal("Couldn't download the file (The file page has no download link.). Opened it in itslearning instead.", file.ErrorMessage);
    }

    [Fact]
    public async Task Selecting_an_event_loads_details_with_plan_material()
    {
        using var h = Host();
        var vm = h.Calendar;
        await vm.EnsureLoadedAsync();

        vm.SelectedEvent = vm.Days[2].Events.Single(e => e.EventId == 2);
        await WaitFor(() => vm.Details.Plans.Count > 0 && !vm.Details.IsLoading);
        var d = vm.Details;

        Assert.True(d.HasEvent);
        Assert.Equal("Lecture - Campus U150", d.Title);
        Assert.Equal("Wednesday, 18 November 2026, 13:00–15:00", d.WhenText);
        Assert.Equal("Campus U150 Teacher", d.PlaceText);
        Assert.Equal("Bring a laptop", d.DescriptionText);
        var plan = Assert.Single(d.Plans);
        Assert.Equal("Read chapter 7", plan.DescriptionText);
        var file = Assert.Single(plan.Material);
        Assert.Equal(("slides.pdf", "File"), (file.Title, file.KindText));

        // A file downloads into the cache and opens in the default app; the second click reuses the copy.
        Assert.True(file.OpensLocally);
        await file.OpenCommand.ExecuteAsync(null);
        await file.OpenCommand.ExecuteAsync(null);
        Assert.Equal([72], h.Files.Downloads);
        Assert.Equal(2, h.OpenFile.Opened.Count);
        Assert.All(h.OpenFile.Opened, path => Assert.Equal(Path.Combine(h.CacheDirectory, "72", "element-72.pdf"), path));
        Assert.Null(file.ErrorMessage);

        d.OpenInItslearningCommand.Execute(null);
        Assert.Equal(["https://school.itslearning.com/Course/Course.aspx?CourseID=10&EventId=2"], h.OpenUrl.Opened.Select(u => u.ToString()));

        vm.SelectedEvent = null;
        Assert.False(d.HasEvent);
    }

    private static async Task WaitFor(Func<bool> condition)
    {
        for (var i = 0; i < 100 && !condition(); i++) await Task.Delay(20);
        Assert.True(condition());
    }
}
