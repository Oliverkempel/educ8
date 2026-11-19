using System.IO.Pipelines;
using System.Net;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

/// <summary>get_schedule, get_event and whats_new through the MCP protocol. Clock: Wed 2026-11-18 12:00 UTC, time zone UTC.</summary>
public sealed class ScheduleAndUpdatesToolTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private IHost _host = null!;
    private McpClient _client = null!;

    private static string Ev(int id, string from, string to, string title, int course) =>
        CalendarAndUpdatesTests.Event(id, from, to, title, course);

    public async Task InitializeAsync()
    {
        _t.SignedIn();
        _t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/calendar/events/v1" => FakeItslearning.Json($$"""
                {"EntityArray":[{{Ev(1, "2026-11-18T13:00:00Z", "2026-11-18T15:00:00Z", "Lecture - Campus U150", 10)}},
                                {{Ev(2, "2026-11-20T08:00:00Z", "2026-11-20T12:00:00Z", "Lessons - Campus U170", 11)}},
                                {{Ev(3, "2026-12-30T08:00:00Z", "2026-12-30T10:00:00Z", "Far away", 10)}}],"Total":3}
                """),
            "/restapi/personal/courses/10/v1" => FakeItslearning.Json("""{"CourseId":10,"Title":"OSDS"}"""),
            "/restapi/personal/courses/10/calendarevents/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"Id":1,"FromDate":"2026-11-18T13:00:00Z","ToDate":"2026-11-18T15:00:00Z","EventTitle":"Lecture - Campus U150","ImportDescription":"Campus U150","CourseId":10,"LocationTitle":"OSDS"}],"Total":1}
                """),
            "/restapi/personal/calendar/events/1/v1" => FakeItslearning.Json("""
                {"EventUrl":"https://school.itslearning.com/Course/Course.aspx?CourseID=10&EventId=1","HasMoreContent":false,"Bookings":[],
                 "ElementLinks":[{"ElementId":900,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"handout.pdf"}],
                 "PlanLinks":[{"PlanId":501,"PlanName":"Lecture","TopicName":"11: Web","Url":"https://school.itslearning.com/plans/courses/10/plan/501"}],
                 "EventId":1,"FromDate":"2026-11-18T13:00:00Z","ToDate":"2026-11-18T15:00:00Z","EventTitle":"Lecture - Campus U150","ImportDescription":"Campus U150 Teacher","EventType":"Course","LocationId":10,"LocationTitle":"OSDS"}
                """),
            "/restapi/personal/planner/v1" => FakeItslearning.Json("""{"EntityArray":[{"PlanId":501,"PlanName":"Lecture","PlanDescription":"<p>Read <b>chapter 7</b></p>","Start":"2026-11-18T13:00:00Z","Stop":"2026-11-18T15:00:00Z","TopicId":7}],"Total":1}"""),
            "/restapi/personal/courses/10/topics/v1" => FakeItslearning.Json("""{"EntityArray":[{"TopicId":7,"TopicName":"11: Web","FolderId":70}],"Total":1}"""),
            "/restapi/personal/courses/10/folders/70/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[{"ElementId":71,"ElementType":"Folder","Title":"Lecture","CourseId":10}],"Total":1}}"""),
            "/restapi/personal/courses/10/folders/71/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[{"ElementId":72,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"slides.pdf","CourseId":10}],"Total":1}}"""),
            "/restapi/personal/notifications/stream/v2" => FakeItslearning.Json("""
                {"EntityArray":[
                  {"NotificationId":11,"Text":"Teacher added Intro in Web","LocationTitle":"Web","LocationId":11,"PublishedDate":"2026-11-18T10:00:00Z","ElementId":55,"ElementType":"LearningToolElement","LearningToolId":5,"ElementsCount":1},
                  {"NotificationId":12,"Text":"New announcement available in OSDS","LocationTitle":"OSDS","LocationId":10,"PublishedDate":"2026-11-17T11:41:00Z","ElementId":0,"ElementType":"Unknown","ElementsCount":0,
                   "LightBulletin":{"LightBulletinId":5,"Text":"Evaluation   has\nstarted","PublishedBy":{"FullName":"Teacher T"},"CommentsCount":2}},
                  {"NotificationId":13,"Text":"Teacher added A and 1 other element","LocationTitle":"Web","LocationId":11,"PublishedDate":"2026-11-16T09:00:00Z","ElementId":0,"ElementType":"Unknown","ElementsCount":2,"HasMoreElements":true},
                  {"NotificationId":14,"Text":"Too old","LocationTitle":"Web","LocationId":11,"PublishedDate":"2026-10-01T09:00:00Z","ElementId":0}
                ],"Total":4}
                """),
            "/restapi/personal/notifications/13/elements/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"ElementId":61,"ElementType":"LearningToolElement","LearningToolId":5006,"Title":"Task 4"},{"ElementId":62,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"lec10.pdf"}],"Total":2}
                """),
            "/restapi/personal/notifications/v2" => FakeItslearning.Json("""
                {"EntityArray":[
                  {"NotificationId":21,"Text":"Assessment is available for MVP","PublishedDate":"2026-11-17T21:53:00Z","Type":"Assessment","ContentUrl":"https://school.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=2363975","IsRead":false},
                  {"NotificationId":22,"Text":"New announcement available in OSDS","PublishedDate":"2026-11-17T11:41:30Z","Type":"Unknown","ContentUrl":"https://school.itslearning.com/Course/Course.aspx?CourseID=10","IsRead":true}
                ],"Total":2}
                """),
            "/restapi/personal/courses/11/v1" => FakeItslearning.Json("""{"CourseId":11,"Title":"Web"}"""),
            "/restapi/personal/courses/11/stream/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"NotificationId":31,"Text":"Teacher added Intro in Web","LocationTitle":"Web","LocationId":11,"PublishedDate":"2026-11-18T10:00:00Z","ElementId":55,"ElementType":"LearningToolElement","LearningToolId":5,"ElementsCount":1}],"Total":1}
                """),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };

        var clientToServer = new Pipe();
        var serverToClient = new Pipe();
        var builder = Host.CreateEmptyApplicationBuilder(settings: null);
        foreach (var d in _t.Services) builder.Services.Add(d);
        builder.Services.AddMcpServer()
            .WithStreamServerTransport(clientToServer.Reader.AsStream(), serverToClient.Writer.AsStream())
            .AddEduc8Tools();
        _host = builder.Build();
        await _host.StartAsync();
        _client = await McpClient.CreateAsync(new StreamClientTransport(clientToServer.Writer.AsStream(), serverToClient.Reader.AsStream()));
    }

    public async Task DisposeAsync()
    {
        await _client.DisposeAsync();
        await _host.StopAsync();
        _host.Dispose();
        _t.Dispose();
    }

    private async Task<(string Text, bool IsError)> Call(string tool, Dictionary<string, object?> args)
    {
        var r = await _client.CallToolAsync(tool, args);
        return (string.Join("\n", r.Content.OfType<TextContentBlock>().Select(c => c.Text)), r.IsError == true);
    }

    [Fact]
    public async Task Schedule_groups_by_day_with_rooms()
    {
        var (text, isError) = await Call("get_schedule", new());

        Assert.False(isError, text);
        Assert.Contains("## Schedule Wed 2026-11-18 – Tue 2026-11-24", text);
        Assert.Contains("### Wed 2026-11-18\n\n- 13:00–15:00 · **Lecture - Campus U150** · Course 10 (course 10) · Room U1 Teacher · event 1", text);
        Assert.Contains("### Fri 2026-11-20", text);
        Assert.DoesNotContain("Far away", text);
    }

    [Fact]
    public async Task Course_schedule_and_bad_dates()
    {
        var (text, _) = await Call("get_schedule", new() { ["courseId"] = 10, ["from"] = "2026-11-18", ["days"] = 1 });
        Assert.Contains("## Schedule for OSDS, Wed 2026-11-18 – Wed 2026-11-18", text);
        Assert.Contains("- 13:00–15:00 · **Lecture - Campus U150** · Campus U150 · event 1", text);

        var (bad, isError) = await Call("get_schedule", new() { ["from"] = "next week" });
        Assert.True(isError);
        Assert.Contains("isn't a date", bad);
    }

    [Fact]
    public async Task Event_shows_plan_description_and_its_material()
    {
        var (text, isError) = await Call("get_event", new() { ["eventId"] = 1 });

        Assert.False(isError, text);
        Assert.Contains("**When:** Wed 2026-11-18 13:00–15:00 (UTC)", text);
        Assert.Contains("**Timetable info:** Campus U150 Teacher", text);
        Assert.Contains("- **Lecture** (topic: 11: Web) · https://school.itslearning.com/plans/courses/10/plan/501\n  Read **chapter 7**\n  - File: slides.pdf · element 72 · read_file", text);
        Assert.Contains("## Linked material\n\n- File: handout.pdf · element 900 · read_file", text);
    }

    [Fact]
    public async Task Whats_new_combines_bell_and_stream()
    {
        var (text, isError) = await Call("whats_new", new());

        Assert.False(isError, text);
        Assert.Contains("- Tue 2026-11-17 21:53 · Assessment is available for MVP **(unread)** · element 2363975 · read_material", text);
        Assert.DoesNotContain("New announcement available in OSDS", text); // bell duplicate of the stream announcement dropped
        Assert.Contains("#### Wed 2026-11-18\n\n- 10:00 · Web (course 11) · Teacher added Intro in Web · Page · element 55 · read", text);
        Assert.Contains("**Announcement** by Teacher T: Evaluation has started (2 comments)", text);
        Assert.Contains("Teacher added A and 1 other element (2 items)\n  - Assignment: Task 4 · element 61 · get_assignment\n  - File: lec10.pdf · element 62 · read_file", text);
        Assert.DoesNotContain("Too old", text);
    }

    [Fact]
    public async Task Whats_new_for_one_course_uses_the_course_stream()
    {
        var (text, _) = await Call("whats_new", new() { ["courseId"] = 11 });
        Assert.Contains("## What's new in Web in the last 7 days", text);
        Assert.Contains("- 10:00 · Teacher added Intro in Web · Page · element 55 · read", text);
        Assert.DoesNotContain("### For you", text);
    }
}
