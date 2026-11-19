using System.IO.Pipelines;
using System.Net;
using Educ8.Documents.Tests;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

/// <summary>Assignment tools through the MCP protocol: REST JSON shaped like real responses + saved (scrubbed) assignment pages.</summary>
public sealed class AssignmentToolTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private IHost _host = null!;
    private McpClient _client = null!;

    private const int Graded = 3001, Open = 3002, Me = 77;
    private bool _webViewBroken;

    private static string Element(int id, string title) =>
        $$"""{"CourseTitle":"Data Management, (F26)","ElementId":{{id}},"ElementType":"LearningToolElement","LearningToolId":5006,"Title":"{{title}}","CourseId":46355,"Url":"https://sdu.itslearning.com/main.aspx?ElementID={{id}}","Active":true,"Path":" / DM / Relational Databases"}""";

    public async Task InitializeAsync()
    {
        _t.SignedIn();
        var lastElement = 0;
        _t.Http.Respond = (req, _) =>
        {
            var p = req.RequestUri!.AbsolutePath;
            switch (p)
            {
                case "/restapi/personal/person/v1":
                    return FakeItslearning.Json($$"""{"PersonId":{{Me}},"FirstName":"Test","LastName":"Student"}""");
                case "/restapi/personal/sso/url/v1":
                    lastElement = int.Parse(Uri.UnescapeDataString(req.RequestUri.Query).Split("LearningToolElementId=")[1].Split('&')[0]);
                    return FakeItslearning.Json("""{"Url":"https://sdu.itslearning.com/sso-login"}""");
                case $"/restapi/personal/courses/resources/3001/v1": return FakeItslearning.Json(Element(Graded, "Point-Giving Activity 1"));
                case $"/restapi/personal/courses/resources/3002/v1": return FakeItslearning.Json(Element(Open, "Task 3 - Reflection"));
                case "/restapi/personal/assignments/3001/v1":
                    return FakeItslearning.Json("""{"ElementId":3001,"Title":"Point-Giving Activity 1","Deadline":"2026-03-02T15:32:00Z","IsSubmissionAfterDeadlineAllowed":false,"MaxScore":5.0,"SubmissionType":"Individual","IsMandatory":true,"IsAssessmentVisibleForStudents":true,"Files":[{"FileId":901,"Name":"instructions.docx","Size":2048}],"TemplateFile":null,"RawText":"<p>Zip <b>Task1</b> and Task2.</p>"}""");
                case "/restapi/personal/assignments/3002/v1":
                    return FakeItslearning.Json("""{"ElementId":3002,"Title":"Task 3 - Reflection","Deadline":"2026-11-25T22:59:00Z","IsSubmissionAfterDeadlineAllowed":true,"SubmissionType":"Individual","Files":[],"RawText":"<ul><li>Which vulnerabilities?</li></ul>"}""");
                case "/restapi/personal/assignments/3001/settings/v1":
                    return FakeItslearning.Json("""{"StatusScale":{"Title":"Simpel status","StatusItems":[]},"AssessmentScale":{"Title":"Pass/fail","AssessmentItems":[{"Title":"Passed","PercentFromAndIncl":50,"PercentTo":100},{"Title":"Not passed","PercentFromAndIncl":0,"PercentTo":50}]}}""");
                case "/restapi/personal/assignments/3002/settings/v1":
                    return FakeItslearning.Json("""{"StatusScale":null,"AssessmentScale":null}""");
                case $"/restapi/personal/assignments/3001/answers/persons/77/v1":
                    return FakeItslearning.Json("""{"AnswerId":555,"IsDraft":false,"Submitted":"2026-03-02T14:53:22Z","Text":"","Files":[{"FileId":902,"Name":"student.zip","Size":4096}]}""");
                case $"/restapi/personal/assignments/3002/answers/persons/77/v1":
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Error calling API") };
                case "/restapi/personal/assignments/3001/answers/555/assessments/files/v1":
                    return FakeItslearning.Json("""[{"FileId":903,"Name":"feedback.txt","Size":20}]""");
                case "/restapi/personal/assignments/3001/files/901/v1": return File("instructions.docx", TestFiles.Docx());
                case "/restapi/personal/assignments/3001/answers/555/files/902/v1": return File("student.zip", TestFiles.Zip(new() { ["Task2.sql"] = "SELECT 1;" }));
                case "/restapi/personal/assignments/3001/answers/555/assessments/files/903/v1": return File("feedback.txt", "Nice work on task 2"u8.ToArray());
                case "/restapi/personal/tasks/v1":
                    return FakeItslearning.Json("""
                        {"EntityArray":[
                          {"ElementId":10,"ElementType":"CustomActivity","LearningToolId":0,"Title":"Read chapter 3","Status":"NotStarted","LocationTitle":"Old course","LocationId":1},
                          {"ElementId":11,"ElementType":"CustomActivity","LearningToolId":0,"Title":"Prepare exercise","Status":"NotStarted","LocationTitle":"Current course","LocationId":2},
                          {"ElementId":3002,"ElementType":"LearningToolElement","LearningToolId":5006,"Title":"Task 3 - Reflection","Status":"NotStarted","Deadline":"2026-11-25T22:59:00Z","LocationTitle":"Current course","LocationId":2}
                        ],"Total":3}
                        """);
                case "/restapi/personal/courses/v4":
                    return FakeItslearning.Json("""{"EntityArray":[{"CourseId":1,"Title":"Old course","LastUpdatedUtc":"2025-01-01T00:00:00Z"},{"CourseId":2,"Title":"Current course","LastUpdatedUtc":"2026-11-18T00:00:00Z"}],"Total":2}""");
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };
        _t.Web.Respond = (req, _) =>
        {
            var u = req.RequestUri!;
            if (_webViewBroken) return new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
            if (u.AbsolutePath == "/sso-login" || u.AbsolutePath.Contains("ViewLearningToolElement"))
                return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), "https://sdu.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx");
            if (u.Host == "platform.itslearning.com")
                return Fixtures.Html(Fixtures.Read(lastElement == Graded ? "assignment_graded.html" : "assignment_unsubmitted.html"), "https://assignment.itslearning.com/view.aspx");
            return new HttpResponseMessage(HttpStatusCode.NotFound);
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

    private static HttpResponseMessage File(string name, byte[] bytes)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };
        response.Content.Headers.ContentDisposition = new("attachment") { FileName = $"\"{name}\"" };
        return response;
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
    public async Task Graded_assignment_shows_everything()
    {
        var (text, isError) = await Call("get_assignment", new() { ["elementId"] = Graded });

        Assert.False(isError, text);
        Assert.Contains("# Point-Giving Activity 1", text);
        Assert.Contains("Assignment · element 3001 · Data Management, (F26) (course 46355) · location: Relational Databases", text);
        Assert.Contains("**Deadline:** Mon 2026-03-02 15:32 (UTC, passed 260 days ago); no late submissions", text);
        Assert.Contains("**Max score:** 5", text);
        Assert.Contains("**Your status:** Completed", text);
        Assert.Contains("## Description\n\nZip **Task1** and Task2.", text);
        Assert.Contains("- instructions.docx (2 KB) · fileId 901", text);
        Assert.Contains("## Grading scale: Pass/fail\n\n- Passed (50–100%)", text);
        Assert.Contains("Submitted: 2. marts 2026 15:53 by Person Name", text);
        Assert.Contains("- student.zip (4 KB, your file) · fileId 902", text);
        Assert.Contains("**Score:** 5 of 5 (100%)", text);
        Assert.Contains("Task 1: Good ER diagram.", text);
        Assert.Contains("- feedback.txt (20 B, feedback file) · fileId 903", text);
    }

    [Fact]
    public async Task Open_assignment_without_submission()
    {
        var (text, _) = await Call("get_assignment", new() { ["elementId"] = Open });

        Assert.Contains("**Deadline:** Wed 2026-11-25 22:59 (UTC, in 7 days); late submissions allowed", text);
        Assert.Contains("**Your status:** Not submitted", text);
        Assert.Contains("- Which vulnerabilities?", text);
        Assert.Contains("## Your submission\n\nNot submitted.", text);
        Assert.DoesNotContain("## Assessment and feedback", text);
    }

    [Fact]
    public async Task Web_view_failure_degrades_gracefully()
    {
        _webViewBroken = true;
        var (text, isError) = await Call("get_assignment", new() { ["elementId"] = Graded });

        Assert.False(isError, text);
        Assert.Contains("- student.zip (4 KB, your file)", text);
        Assert.Contains("Note: Your status, score and the teacher's feedback couldn't be loaded", text);
    }

    [Fact]
    public async Task Reads_teacher_own_and_feedback_files()
    {
        var (teacher, _) = await Call("read_assignment_file", new() { ["elementId"] = Graded, ["fileId"] = 901 });
        Assert.Contains("Word document", teacher);
        Assert.Contains("the teacher's file for assignment \"Point-Giving Activity 1\" (element 3001)", teacher);
        Assert.Contains("## Assignment 2", teacher);

        var (own, _) = await Call("read_assignment_file", new() { ["elementId"] = Graded, ["fileId"] = 902, ["zipEntry"] = "Task2.sql" });
        Assert.Contains("```sql\nSELECT 1;\n```", own);

        var (feedback, _) = await Call("read_assignment_file", new() { ["elementId"] = Graded, ["fileId"] = 903 });
        Assert.Contains("Nice work on task 2", feedback);

        var (unknown, isError) = await Call("read_assignment_file", new() { ["elementId"] = Graded, ["fileId"] = 999 });
        Assert.True(isError);
        Assert.Contains("Call `get_assignment` to see the file ids", unknown);
    }

    [Fact]
    public async Task Tasks_grouped_by_current_course_with_assignments_first()
    {
        var (text, _) = await Call("list_my_tasks", new());

        Assert.True(text.IndexOf("### Current course", StringComparison.Ordinal) < text.IndexOf("### Old course", StringComparison.Ordinal));
        Assert.True(text.IndexOf("| 3002 | Assignment |", StringComparison.Ordinal) < text.IndexOf("| 11 | Activity |", StringComparison.Ordinal));
        Assert.Contains("| 3002 | Assignment | Task 3 - Reflection | 2026-11-25 22:59 | NotStarted |", text);
    }

    [Fact]
    public async Task Read_material_on_assignment_gives_assignment_details()
    {
        var (text, _) = await Call("read_material", new() { ["elementId"] = Open });
        Assert.Contains("**Your status:** Not submitted", text);
    }
}
