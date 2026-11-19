using System.IO.Pipelines;
using System.Net;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

/// <summary>Phase 2 tools through the MCP protocol, with a fake itslearning (REST JSON shaped like real responses + saved web pages).</summary>
public sealed class CourseContentToolTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private IHost _host = null!;
    private McpClient _client = null!;

    private const string Resources = """
        {"Resources":{"EntityArray":[
          {"ElementId":2262801,"ElementType":"Folder","LearningToolId":0,"Title":"General Course Information","CourseId":50352,"Active":true,"Path":" / OSDS, (E26)"},
          {"ElementId":2266446,"ElementType":"LearningToolElement","LearningToolId":5,"Title":"Course description","CourseId":50352,"Active":true,"Path":" / OSDS, (E26)"},
          {"ElementId":2371764,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"OS&Linux.pdf","CourseId":50352,"Active":true,"Homework":true,"Path":" / OSDS, (E26) / 02: OS / Exercise"}
        ],"Total":3,"CurrentPageIndex":0,"PageSize":100},"AddElementUrl":null}
        """;

    private static string Resource(int id, string type, int tool, string title) =>
        $$"""{"CourseCode":"T510046101-1-E26","CourseTitle":"OSDS, (E26)","ElementId":{{id}},"ElementType":"{{type}}","LearningToolId":{{tool}},"Title":"{{title}}","CourseId":50352,"Url":"https://school.itslearning.com/main.aspx?CourseID=50352&ElementID={{id}}","Active":true,"Path":" / OSDS, (E26) / General"}""";

    public async Task InitializeAsync()
    {
        _t.SignedIn();
        _t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/personal/courses/50352/v1" => FakeItslearning.Json("""{"CourseId":50352,"Title":"OSDS, (E26)","Code":"T510046101-1-E26"}"""),
            "/restapi/personal/courses/50352/topics/v1" => FakeItslearning.Json("""{"EntityArray":[{"TopicId":105776,"TopicName":"11: Web Technologies","FolderId":2358879}],"Total":1,"CurrentPageIndex":0,"PageSize":100}"""),
            "/restapi/personal/courses/50352/resources/v1" => FakeItslearning.Json(Resources),
            "/restapi/personal/courses/50352/folders/2262801/resources/v1" => FakeItslearning.Json(Resources),
            "/restapi/personal/courses/50352/folders/2358879/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[{"ElementId":777,"ElementType":"Folder","LearningToolId":0,"Title":"Lecture","CourseId":50352,"Active":true}],"Total":1}}"""),
            "/restapi/personal/courses/50352/folders/777/resources/v1" => FakeItslearning.Json("""{"Resources":{"EntityArray":[{"ElementId":555,"ElementType":"LearningToolElement","LearningToolId":5009,"Title":"Week 11 slides.pdf","CourseId":50352,"Active":true}],"Total":1}}"""),
            "/restapi/personal/courses/50352/resources/search/v1" => FakeItslearning.Json(req.RequestUri.Query.Contains("searchText=linux") ? Resources : """{"Resources":{"EntityArray":[],"Total":0}}"""),
            "/restapi/personal/planner/v1" => FakeItslearning.Json("""
                {"EntityArray":[
                  {"PlanId":2,"PlanName":"Lecture","PlanDescription":"<p>Read <b>chapter 3</b></p>","Start":"2026-11-19T11:00:00Z","Stop":"2026-11-19T13:00:00Z","TopicId":105776},
                  {"PlanId":1,"PlanName":"Preparation","PlanDescription":"","Start":"2026-11-12T15:00:00Z","Stop":"2026-11-12T16:00:00Z","TopicId":0}
                ],"Total":2,"CurrentPageIndex":0,"PageSize":100}
                """),
            "/restapi/personal/courses/50352/tasks/v1" => FakeItslearning.Json("""{"EntityArray":[{"TaskId":1,"ElementId":2425693,"ElementType":"LearningToolElement","LearningToolId":5002,"Title":"Demo Exam","Status":"NotStarted","Deadline":null}],"Total":1}"""),
            "/restapi/personal/courses/50352/bulletins/v1" => new HttpResponseMessage(HttpStatusCode.InternalServerError),
            "/restapi/personal/courses/50352/calendarevents/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"Id":1284442,"FromDate":"2026-11-23T07:00:00Z","ToDate":"2026-11-23T15:00:00Z","EventTitle":"Project work - Campus TEK","ImportDescription":"Book selv projektlokaler","CourseId":50352,"LocationTitle":"OSDS"}],"Total":1}
                """),
            "/restapi/personal/courses/50352/stream/v1" => FakeItslearning.Json("""
                {"EntityArray":[{"NotificationId":1,"Text":"Teacher added Week 11 slides.pdf","LocationTitle":"OSDS","LocationId":50352,"PublishedDate":"2026-11-17T10:00:00Z","ElementId":555,"ElementType":"LearningToolElement","LearningToolId":5009,"ElementsCount":1}],"Total":1}
                """),
            "/restapi/personal/courses/resources/2266446/v1" => FakeItslearning.Json(Resource(2266446, "LearningToolElement", 5, "Course description")),
            "/restapi/personal/courses/resources/2371739/v1" => FakeItslearning.Json(Resource(2371739, "LearningToolElement", 5010, "Linux kernel article")),
            "/restapi/personal/courses/resources/2371764/v1" => FakeItslearning.Json(Resource(2371764, "LearningToolElement", 5009, "OS&Linux.pdf")),
            "/restapi/personal/courses/resources/2262801/v1" => FakeItslearning.Json(Resource(2262801, "Folder", 0, "General Course Information")),
            "/restapi/personal/sso/url/v1" => FakeItslearning.Json("""{"Url":"https://school.itslearning.com/sso-login?x=1"}"""),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        // The web UI: the SSO link lands on the element view; its iframe opens the tool page for that element.
        int lastElement = 0;
        _t.Web.Respond = (req, _) =>
        {
            var u = req.RequestUri!;
            if (u.AbsolutePath == "/sso-login") return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), "https://school.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=" + lastElement);
            if (u.AbsolutePath.Contains("ViewLearningToolElement")) return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), u.ToString());
            if (u.Host == "platform.itslearning.com")
                return lastElement == 2371739
                    ? Fixtures.Html(Fixtures.Read("link_2371739.html"), "https://resource.itslearning.com/Link/View.aspx")
                    : Fixtures.Html(Fixtures.Read("page_coursedesc_2266446.html"), "https://page.itslearning.com/PageInstance.aspx");
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };
        var http = _t.Http.Respond;
        _t.Http.Respond = (req, body) =>
        {
            if (req.RequestUri!.AbsolutePath == "/restapi/personal/sso/url/v1")
                lastElement = int.Parse(Uri.UnescapeDataString(req.RequestUri.Query).Split("LearningToolElementId=")[1].Split('&')[0]);
            return http(req, body);
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

    private async Task<string> Call(string tool, Dictionary<string, object?> args)
    {
        var result = await _client.CallToolAsync(tool, args);
        var text = string.Join("\n", result.Content.OfType<TextContentBlock>().Select(c => c.Text));
        Assert.False(result.IsError == true, text);
        return text;
    }

    [Fact]
    public async Task Overview_combines_sections_and_survives_a_failing_one()
    {
        var text = await Call("get_course_overview", new() { ["courseId"] = 50352 });

        Assert.Contains("# OSDS, (E26)", text);
        Assert.Contains("semester E26", text);
        Assert.Contains("| 11: Web Technologies | 2358879 |", text);
        // plans sorted by start, with topic name and converted description
        Assert.True(text.IndexOf("**Preparation**", StringComparison.Ordinal) < text.IndexOf("**Lecture**", StringComparison.Ordinal));
        Assert.Contains("Thu 2026-11-19 11:00–13:00 · **Lecture** (topic: 11: Web Technologies): Read **chapter 3**", text);
        Assert.Contains("**Demo Exam** · Test · element 2425693 · no deadline set", text);
        Assert.Contains("## Latest announcements\n\n_(couldn't load this part right now)_", text);
        Assert.Contains("## Recent changes (last 14 days)\n\n- Tue 2026-11-17 10:00 · Teacher added Week 11 slides.pdf · File · element 555 · read_file", text);
        // Regression: a project day that exists only in the calendar (no lesson plan) must appear, and plans must not pass for the timetable.
        Assert.Contains("## Upcoming calendar events (next 7 days)\n\n- Mon 2026-11-23 07:00–15:00 · **Project work - Campus TEK** · Book selv projektlokaler · event 1284442", text);
        Assert.Contains("they are not the full timetable", text);
        Assert.Contains("| 2266446 | Page | Course description | read |", text);
    }

    [Fact]
    public async Task Browse_folder_shows_kinds_and_next_steps()
    {
        var text = await Call("browse_course_materials", new() { ["courseId"] = 50352, ["folderId"] = 2262801 });
        Assert.Contains("## Folder: General Course Information", text);
        Assert.Contains("| 2262801 | Folder | General Course Information | browse |", text);
        Assert.Contains("| 2371764 | File | OS&Linux.pdf (homework) | read_file |", text);
    }

    [Fact]
    public async Task Search_shows_location_or_explains_no_match()
    {
        var text = await Call("search_course_materials", new() { ["courseId"] = 50352, ["query"] = "linux" });
        Assert.Contains("| 2371764 | File | OS&Linux.pdf (homework) | 02: OS / Exercise |", text);

        text = await Call("search_course_materials", new() { ["courseId"] = 50352, ["query"] = "zzz" });
        Assert.Contains("only matches titles", text);
    }

    [Fact]
    public async Task Reads_page_as_markdown()
    {
        var text = await Call("read_material", new() { ["elementId"] = 2266446 });
        Assert.Contains("# Course description", text);
        Assert.Contains("Page · element 2266446 · OSDS, (E26) (course 50352) · location: General", text);
        Assert.Contains("[Course description](https://odin.example.edu/sitecore/index.php?a=searchfagbesk&bbcourseid=T510046101-1-E26&lang=en)", text);
    }

    [Fact]
    public async Task Reads_link_target()
    {
        var text = await Call("read_material", new() { ["elementId"] = 2371739 });
        Assert.Contains("Link target: <https://digilent.com/blog/demystifiying-the-linux-kernel/>", text);
    }

    [Fact]
    public async Task Folder_is_listed()
    {
        var folder = await Call("read_material", new() { ["elementId"] = 2262801 });
        Assert.Contains("| 2266446 | Page | Course description | read |", folder);
    }

    [Fact]
    public async Task Overview_lists_material_of_each_lesson_plan()
    {
        // Plan "Lecture" in topic 105776 → subfolder "Lecture" of the topic folder 2358879.
        var text = await Call("get_course_overview", new() { ["courseId"] = 50352 });
        Assert.Contains("**Lecture** (topic: 11: Web Technologies): Read **chapter 3**\n  - File: Week 11 slides.pdf · element 555 · read_file\n", text);
    }

    [Fact]
    public async Task Overview_reads_material_from_the_plan_itself()
    {
        // Plan "Preparation" has no topic folder; its elements come from the plan endpoint (shape as live, synthetic content).
        var http = _t.Http.Respond;
        _t.Http.Respond = (req, body) => req.RequestUri!.AbsolutePath == "/restapi/personal/course/50352/plans/1/v2"
            ? FakeItslearning.Json("""
                {"id":1,"title":"Preparation","url":"/plans/courses/50352/plan/1","elements":[
                  {"id":901,"title":"Reading guide.pdf","url":"/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=901&IsPlanContext=True","type":"LearningToolElement","typeName":"File","learningToolId":5009,"isResource":true,"isActive":true,"isFolder":false},
                  {"id":902,"title":"Hidden draft","type":"LearningToolElement","learningToolId":5,"isResource":true,"isActive":false,"isFolder":false}
                ],"learningObjectives":[],"customFields":null}
                """)
            : http(req, body);

        var text = await Call("get_course_overview", new() { ["courseId"] = 50352 });
        Assert.Contains("**Preparation**", text);
        Assert.Contains("  - File: Reading guide.pdf · element 901 · read_file\n", text);
        Assert.DoesNotContain("Hidden draft", text);
        // Plan "Lecture" has no plan data here (404), so the topic-folder fallback still finds its material.
        Assert.Contains("  - File: Week 11 slides.pdf · element 555 · read_file\n", text);
    }
}
