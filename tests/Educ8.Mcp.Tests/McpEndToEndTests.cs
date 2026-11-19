using System.IO.Pipelines;
using System.Net;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

/// <summary>
/// Drives the real tools through the MCP protocol (in-memory streams), with only itslearning's HTTP faked.
/// The same AddEduc8Tools() call is what the stdio host and a future in-app HTTP host use.
/// </summary>
public sealed class McpEndToEndTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private IHost _host = null!;
    private McpClient _client = null!;

    public async Task InitializeAsync()
    {
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

    private async Task<(string Text, bool IsError)> Call(string tool, Dictionary<string, object?>? args = null)
    {
        var result = await _client.CallToolAsync(tool, args ?? []);
        return (string.Join("\n", result.Content.OfType<TextContentBlock>().Select(c => c.Text)), result.IsError == true);
    }

    [Fact]
    public async Task Lists_tools_with_read_only_hints()
    {
        var tools = await _client.ListToolsAsync();
        Assert.Equal(
            ["auth_status", "browse_course_materials", "get_assignment", "get_course_overview", "get_event", "get_schedule", "list_my_courses", "list_my_tasks", "login", "login_complete",
             "read_assignment_file", "read_file", "read_material", "search_course_materials", "whats_new"],
            tools.Select(t => t.Name).Order());
        foreach (var name in new[] { "list_my_courses", "get_course_overview", "browse_course_materials", "search_course_materials", "read_material", "read_file",
                                     "list_my_tasks", "get_assignment", "read_assignment_file" })
            Assert.True(tools.Single(t => t.Name == name).ProtocolTool.Annotations?.ReadOnlyHint, name);
    }

    [Fact]
    public async Task Write_tools_hidden_by_default()
    {
        var tools = await _client.ListToolsAsync();
        Assert.DoesNotContain(tools, t => t.Name == "submit_assignment");
        Assert.All(tools.Where(t => t.Name is not ("login" or "login_complete")), t => Assert.True(t.ProtocolTool.Annotations?.ReadOnlyHint, t.Name));
    }

    [Fact]
    public async Task Not_logged_in_gives_actionable_error()
    {
        var (text, isError) = await Call("list_my_courses");
        Assert.True(isError);
        Assert.Contains("login", text);

        (text, _) = await Call("auth_status");
        Assert.Contains("Not logged in to school.itslearning.com", text);
    }

    [Fact]
    public async Task Login_flow_then_list_courses()
    {
        _t.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
        {
            "/restapi/oauth2/token" => FakeItslearning.Json(FakeItslearning.TokenJson("A", "R")),
            "/restapi/personal/person/v1" => FakeItslearning.Json("""{"PersonId":5,"FirstName":"Test","LastName":"Student"}"""),
            "/restapi/personal/courses/v4" => FakeItslearning.Json(ItslearningClientTests.CoursePage(0, 2, [7, 8])),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };

        var (text, _) = await Call("login");
        var state = System.Text.RegularExpressions.Regex.Match(text, "state=([0-9a-f]+)").Groups[1].Value;
        Assert.NotEmpty(state);

        (text, var isError) = await Call("login_complete", new() { ["redirectUrl"] = $"itsl-itslearning://login/?state={state}&code=900937-c" });
        Assert.False(isError, text);
        Assert.Contains("Logged in to itslearning as Test Student", text);

        (text, _) = await Call("list_my_courses", new() { ["search"] = "course 8" });
        Assert.Contains("| 8 | Course 8 | C8-1-E26 | E26 |", text);
        Assert.DoesNotContain("| 7 |", text);

        (text, _) = await Call("login");
        Assert.Contains("Already logged in as Test Student", text);
    }

    [Fact]
    public async Task Login_complete_rejects_foreign_state()
    {
        await Call("login");
        var (text, isError) = await Call("login_complete", new() { ["redirectUrl"] = "itsl-itslearning://login/?state=00000000000000000000000000000000&code=x" });
        Assert.True(isError);
        Assert.Contains("doesn't belong to a login started here", text);
        Assert.Empty(_t.Http.Requests);
    }
}
