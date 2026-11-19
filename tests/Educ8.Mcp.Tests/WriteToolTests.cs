using System.IO.Pipelines;
using System.Net;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

public sealed class WriteSettingTests : IDisposable
{
    private readonly string _dir = Directory.CreateTempSubdirectory("educ8-settings-").FullName;

    public void Dispose() => Directory.Delete(_dir, recursive: true);

    [Fact]
    public void Off_by_default_then_config_then_environment()
    {
        Assert.False(Educ8McpOptions.Load(_dir, _ => null).AllowWriteActions);

        File.WriteAllText(Path.Combine(_dir, "config.json"), """{ "ClientId": "abc", "AllowWriteActions": true }""");
        Assert.True(Educ8McpOptions.Load(_dir, _ => null).AllowWriteActions);
        Assert.False(Educ8McpOptions.Load(_dir, k => k == Educ8McpOptions.AllowWriteActionsEnvVar ? "off" : null).AllowWriteActions);
    }

    [Fact]
    public void Saving_keeps_other_settings()
    {
        File.WriteAllText(Path.Combine(_dir, "config.json"), """{ "ClientId": "abc" }""");
        Educ8McpOptions.SaveAllowWriteActions(true, _dir);

        var json = File.ReadAllText(Path.Combine(_dir, "config.json"));
        Assert.Contains("\"ClientId\": \"abc\"", json);
        Assert.True(Educ8McpOptions.Load(_dir, _ => null).AllowWriteActions);
    }
}

/// <summary>submit_assignment through the MCP protocol with write actions enabled.</summary>
public sealed class WriteToolTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private readonly Educ8McpOptions _options = new() { AllowWriteActions = true };
    private readonly string _files = Directory.CreateTempSubdirectory("educ8-upload-").FullName;
    private IHost _host = null!;
    private McpClient _client = null!;

    private const int OpenAssignment = 4001, Overdue = 4002;

    public async Task InitializeAsync()
    {
        _t.SignedIn();
        File.WriteAllText(Path.Combine(_files, "answer.txt"), "my solution");
        Directory.CreateDirectory(Path.Combine(_files, ".secret"));
        File.WriteAllText(Path.Combine(_files, ".secret", "key"), "do not upload");

        _t.Http.Respond = (req, body) => (req.Method.Method, req.RequestUri!.AbsolutePath) switch
        {
            ("GET", "/restapi/personal/person/v1") => FakeItslearning.Json("""{"PersonId":77,"FirstName":"Test","LastName":"Student"}"""),
            ("GET", "/restapi/personal/assignments/4001/v1") => FakeItslearning.Json("""{"ElementId":4001,"Title":"Reflection","Deadline":"2026-11-25T22:59:00Z","IsSubmissionAfterDeadlineAllowed":false,"IsActive":true,"SubmissionType":"Individual","Files":[]}"""),
            ("GET", "/restapi/personal/assignments/4002/v1") => FakeItslearning.Json("""{"ElementId":4002,"Title":"Old one","Deadline":"2026-03-02T15:32:00Z","IsSubmissionAfterDeadlineAllowed":false,"IsActive":true,"Files":[]}"""),
            ("GET", var p) when p.Contains("/answers/persons/77/") => new HttpResponseMessage(HttpStatusCode.InternalServerError),
            ("GET", "/restapi/personal/sso/url/v1") => FakeItslearning.Json("""{"Url":"https://sdu.itslearning.com/sso-login"}"""),
            ("POST", "/restapi/personal/fileupload/temp/v1") => FakeItslearning.Json("""{"FileRepoId":"repo-1","SignedFileRepoId":"sig","Name":"answer.txt","MimeType":"text/plain","SourceType":1,"IsTemplate":false}"""),
            ("POST", "/restapi/personal/assignments/4001/answers/v1") => FakeItslearning.Json("""{"AnswerId":999}"""),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
        _t.Web.Respond = (req, _) =>
        {
            var u = req.RequestUri!;
            if (u.AbsolutePath == "/sso-login" || u.AbsolutePath.Contains("ViewLearningToolElement"))
                return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), "https://sdu.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx");
            if (u.Host == "platform.itslearning.com")
                return Fixtures.Html(Fixtures.Read("assignment_unsubmitted.html"), "https://assignment.itslearning.com/view.aspx");
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };

        var clientToServer = new Pipe();
        var serverToClient = new Pipe();
        var builder = Host.CreateEmptyApplicationBuilder(settings: null);
        foreach (var d in _t.Services) builder.Services.Add(d);
        builder.Services.AddMcpServer()
            .WithStreamServerTransport(clientToServer.Reader.AsStream(), serverToClient.Writer.AsStream())
            .AddEduc8Tools(_options);
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
        Directory.Delete(_files, recursive: true);
    }

    private async Task<(string Text, bool IsError)> Submit(Dictionary<string, object?> args)
    {
        var r = await _client.CallToolAsync("submit_assignment", args);
        return (string.Join("\n", r.Content.OfType<TextContentBlock>().Select(c => c.Text)), r.IsError == true);
    }

    private int Posts => _t.Http.Requests.Count(r => r.Method == HttpMethod.Post);

    [Fact]
    public async Task Write_tool_is_listed_and_marked_destructive()
    {
        var tool = (await _client.ListToolsAsync()).Single(t => t.Name == "submit_assignment");
        Assert.True(tool.ProtocolTool.Annotations?.DestructiveHint);
        Assert.False(tool.ProtocolTool.Annotations?.ReadOnlyHint);
    }

    [Fact]
    public async Task Without_confirm_only_previews()
    {
        var file = Path.Combine(_files, "answer.txt");
        var (text, isError) = await Submit(new() { ["elementId"] = OpenAssignment, ["text"] = "Hello\nworld", ["filePaths"] = new[] { file } });

        Assert.False(isError, text);
        Assert.StartsWith("PREVIEW: nothing has been submitted.", text);
        Assert.Contains("Assignment: \"Reflection\" (element 4001)", text);
        Assert.Contains("> Hello\n> world", text);
        Assert.Contains($"- {file} (11 B)", text);
        Assert.Contains("confirm=true", text);
        Assert.Equal(0, Posts);
    }

    [Fact]
    public async Task Confirm_uploads_then_submits()
    {
        var file = Path.Combine(_files, "answer.txt");
        var (text, isError) = await Submit(new() { ["elementId"] = OpenAssignment, ["text"] = "Hello <b>\nworld\n\nBye", ["filePaths"] = new[] { file }, ["confirm"] = true });

        Assert.False(isError, text);
        Assert.Contains("Submitted to \"Reflection\" (element 4001). Answer id 999. Files: answer.txt.", text);

        var upload = _t.Http.Requests.Single(r => r.PathAndQuery == "/restapi/personal/fileupload/temp/v1");
        Assert.Contains("filename=answer.txt", upload.Body);
        Assert.Contains("my solution", upload.Body);

        var submit = _t.Http.Requests.Single(r => r.PathAndQuery == "/restapi/personal/assignments/4001/answers/v1");
        Assert.Contains("\"FileRepoId\":\"repo-1\"", submit.Body);
        Assert.Contains("\"SignedFileRepoId\":\"sig\"", submit.Body);
        using var json = System.Text.Json.JsonDocument.Parse(submit.Body!);
        Assert.Equal("<p>Hello &lt;b&gt;<br />world</p><p>Bye</p>", json.RootElement.GetProperty("Text").GetString()); // escaped, paragraphs kept
    }

    [Theory]
    [InlineData("relative/answer.txt", "isn't an absolute path")]
    [InlineData("{dir}/missing.txt", "doesn't exist")]
    [InlineData("{dir}/.secret/key", "hidden file or inside a hidden folder")]
    [InlineData("{dir}", "is a folder")]
    public async Task Unsafe_or_bad_files_are_refused(string path, string expected)
    {
        var (text, isError) = await Submit(new() { ["elementId"] = OpenAssignment, ["filePaths"] = new[] { path.Replace("{dir}", _files) }, ["confirm"] = true });
        Assert.True(isError);
        Assert.Contains(expected, text);
        Assert.Equal(0, Posts);
    }

    [Fact]
    public async Task Closed_deadline_is_refused()
    {
        var (text, isError) = await Submit(new() { ["elementId"] = Overdue, ["text"] = "late", ["confirm"] = true });
        Assert.True(isError);
        Assert.Contains("deadline for \"Old one\" has passed", text);
        Assert.Equal(0, Posts);
    }

    [Fact]
    public async Task Turning_the_setting_off_blocks_even_registered_tools()
    {
        _options.AllowWriteActions = false;
        var (text, isError) = await Submit(new() { ["elementId"] = OpenAssignment, ["text"] = "x", ["confirm"] = true });
        Assert.True(isError);
        Assert.Contains("Write actions are disabled", text);
        Assert.Equal(0, Posts);
    }
}
