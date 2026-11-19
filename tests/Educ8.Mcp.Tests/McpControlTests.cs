using System.IO.Pipelines;
using Educ8.Itslearning.Tests;
using Educ8.Mcp.Control;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

/// <summary>What the desktop app's MCP page controls: tool switches, write actions, live reload, activity and status.</summary>
public sealed class McpControlTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private readonly Educ8McpOptions _options = new() { DisabledTools = new HashSet<string> { "whats_new" } };
    private readonly RecordingSink _sink = new();
    private IHost _host = null!;
    private McpClient _client = null!;

    private sealed class RecordingSink : IMcpActivitySink
    {
        public List<McpActivityEntry> Entries { get; } = [];
        public void Record(McpActivityEntry entry, string? clientVersion) { lock (Entries) Entries.Add(entry); }
    }

    public async Task InitializeAsync()
    {
        var clientToServer = new Pipe();
        var serverToClient = new Pipe();
        var builder = Host.CreateEmptyApplicationBuilder(settings: null);
        foreach (var d in _t.Services) builder.Services.Add(d);
        builder.Services.AddSingleton<IMcpActivitySink>(_sink);
        builder.Services.AddMcpServer()
            .WithStreamServerTransport(clientToServer.Reader.AsStream(), serverToClient.Writer.AsStream())
            .AddEduc8Tools(_options);
        _host = builder.Build();
        await _host.StartAsync();
        _client = await McpClient.CreateAsync(new StreamClientTransport(clientToServer.Writer.AsStream(), serverToClient.Reader.AsStream()),
            new McpClientOptions { ClientInfo = new() { Name = "test-client", Version = "9.9" } });
    }

    public async Task DisposeAsync()
    {
        await _client.DisposeAsync();
        await _host.StopAsync();
        _host.Dispose();
        _t.Dispose();
    }

    [Fact]
    public async Task Turned_off_tools_are_hidden_and_refused()
    {
        var tools = await _client.ListToolsAsync();
        Assert.DoesNotContain(tools, t => t.Name == "whats_new");
        Assert.Contains(tools, t => t.Name == "auth_status");

        var result = await _client.CallToolAsync("whats_new", new Dictionary<string, object?> { ["days"] = 3 });
        Assert.True(result.IsError);
        Assert.Contains("turned off in the Educ8 desktop app", ((TextContentBlock)result.Content[0]).Text);

        var entry = Assert.Single(_sink.Entries);
        Assert.Equal(("whats_new", true, false, "test-client", """{"days":3}"""), (entry.Tool, entry.Blocked, entry.Ok, entry.Client, entry.Arguments));
    }

    [Fact]
    public async Task Settings_apply_while_running()
    {
        Assert.DoesNotContain(await _client.ListToolsAsync(), t => t.Name == "submit_assignment");
        var refused = await _client.CallToolAsync("submit_assignment", new Dictionary<string, object?> { ["elementId"] = 1, ["text"] = "x" });
        Assert.Contains("Write actions are turned off", ((TextContentBlock)refused.Content[0]).Text);

        _options.AllowWriteActions = true;
        _options.DisabledTools = new HashSet<string>();
        var tools = await _client.ListToolsAsync();
        Assert.Contains(tools, t => t.Name == "submit_assignment");
        Assert.Contains(tools, t => t.Name == "whats_new");
    }

    [Fact]
    public async Task Calls_are_recorded_with_result_and_client()
    {
        await _client.CallToolAsync("auth_status");
        await _client.CallToolAsync("list_my_courses"); // not logged in → tool error

        Assert.Equal([("auth_status", true), ("list_my_courses", false)], _sink.Entries.Select(e => (e.Tool, e.Ok)));
        Assert.All(_sink.Entries, e => Assert.Equal("test-client", e.Client));
        Assert.Contains("login", _sink.Entries[1].Error);
    }

    [Fact]
    public async Task Changed_settings_file_notifies_the_client()
    {
        var notified = new TaskCompletionSource();
        await using var _ = _client.RegisterNotificationHandler(NotificationMethods.ToolListChangedNotification, (_, _) =>
        {
            notified.TrySetResult();
            return ValueTask.CompletedTask;
        });
        await _client.ListToolsAsync(); // the server learns its session from a request

        using var watcher = new McpSettingsWatcher(_options, _host.Services.GetRequiredService<McpSessionTracker>(), _t.DataDir, NullLogger.Instance);
        Educ8McpOptions.SaveDisabledTools(["auth_status"], _t.DataDir);
        await watcher.ReloadAsync();

        await notified.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Assert.Equal(["auth_status"], _options.DisabledTools);
        Assert.DoesNotContain(await _client.ListToolsAsync(), t => t.Name == "auth_status");
    }
}

public sealed class McpControlFileTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "educ8-mcp-tests", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        try { Directory.Delete(_dir, recursive: true); } catch (DirectoryNotFoundException) { }
    }

    [Fact]
    public void Settings_keep_other_keys_and_round_trip()
    {
        Directory.CreateDirectory(_dir);
        File.WriteAllText(Path.Combine(_dir, "config.json"), """{ "ClientId": "abc" }""");
        Educ8McpOptions.SaveDisabledTools(["read_file", "get_event", "read_file"], _dir);
        Educ8McpOptions.SaveAllowWriteActions(true, _dir);

        var loaded = Educ8McpOptions.Load(_dir, _ => null);
        Assert.True(loaded.AllowWriteActions);
        Assert.Equal(["get_event", "read_file"], loaded.DisabledTools.Order());
        Assert.Contains("\"ClientId\": \"abc\"", File.ReadAllText(Path.Combine(_dir, "config.json")));
        Assert.False(loaded.IsAvailable("read_file"));
        Assert.True(loaded.IsAvailable("submit_assignment"));
    }

    [Fact]
    public void Catalog_lists_every_tool_and_marks_write_tools()
    {
        Assert.Contains(McpToolCatalog.All, t => t is { Name: "read_file", IsWrite: false } && t.Description.Length > 0);
        Assert.True(McpToolCatalog.Find("submit_assignment")?.IsWrite);
        Assert.Equal(McpToolCatalog.All.Count, McpToolCatalog.All.Select(t => t.Name).Distinct().Count());
    }

    [Fact]
    public void Activity_log_reads_newest_first_and_trims()
    {
        var log = new McpActivityLog(_dir);
        var t0 = new DateTimeOffset(2026, 11, 19, 12, 0, 0, TimeSpan.Zero);
        for (var i = 0; i < 5; i++) log.Append(new McpActivityEntry { Time = t0.AddMinutes(i), Tool = $"tool{i}", Ok = true });
        File.AppendAllText(log.Path, "not json\n");

        Assert.Equal(["tool4", "tool3"], log.ReadRecent(2).Select(e => e.Tool));

        var big = new string('x', 4000);
        for (var i = 0; i < 200; i++) log.Append(new McpActivityEntry { Time = t0, Tool = "bulk", Arguments = big });
        Assert.True(new FileInfo(log.Path).Length <= McpActivityLog.MaxBytes + 8000);
        Assert.Equal("bulk", log.ReadRecent(1).Single().Tool);
    }

    [Fact]
    public void Registry_lists_live_servers_and_drops_dead_ones()
    {
        var registry = new McpServerRegistry(_dir);
        registry.Register(DateTimeOffset.UtcNow);
        registry.RecordCall("claude-ai", "1.2", DateTimeOffset.UtcNow);
        File.WriteAllText(Path.Combine(registry.Directory, "999999.json"), """{"processId":999999,"startedAt":"2026-11-19T10:00:00Z"}""");

        var running = Assert.Single(registry.ListRunning());
        Assert.Equal((Environment.ProcessId, "claude-ai", 1), (running.ProcessId, running.Client, running.Calls));
        Assert.False(File.Exists(Path.Combine(registry.Directory, "999999.json")));

        registry.Unregister();
        Assert.Empty(registry.ListRunning());
    }
}
