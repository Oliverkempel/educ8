using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;
using Educ8.Mcp;
using Educ8.Mcp.Control;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.App.Core.Tests;

public class McpViewModelTests
{
    private static McpViewModel Page(AppTestHost h) => h.Provider.GetRequiredService<McpViewModel>();

    [Fact]
    public void Lists_every_tool_with_write_tools_locked_until_allowed()
    {
        using var h = new AppTestHost();
        var vm = Page(h);

        Assert.Equal(McpToolCatalog.All.Select(t => t.Name), vm.Tools.Select(t => t.Name));
        Assert.False(vm.AllowWriteActions);
        var submit = vm.Tools.Single(t => t.Name == "submit_assignment");
        Assert.True(submit.IsWrite);
        Assert.False(submit.CanSwitch);
        Assert.False(submit.IsAvailable);
        var read = vm.Tools.Single(t => t.Name == "read_file");
        Assert.True(read.IsAvailable);
        Assert.Equal("Downloads a course file and returns its content.", read.Summary);

        vm.AllowWriteActions = true;
        Assert.True(submit.CanSwitch);
        Assert.True(submit.IsAvailable);
    }

    [Fact]
    public void Switches_are_saved_for_the_server_and_settings_from_elsewhere_show_up()
    {
        using var h = new AppTestHost();
        var vm = Page(h);

        vm.Tools.Single(t => t.Name == "whats_new").IsEnabled = false;
        vm.AllowWriteActions = true;
        var saved = Educ8McpOptions.Load(h.T.DataDir, _ => null);
        Assert.Equal(["whats_new"], saved.DisabledTools);
        Assert.True(saved.AllowWriteActions);

        // Changed by the CLI (`educ8-mcp write-actions off`) while the page is open.
        Educ8McpOptions.SaveAllowWriteActions(false, h.T.DataDir);
        Educ8McpOptions.SaveDisabledTools(["read_file"], h.T.DataDir);
        vm.Refresh();
        Assert.False(vm.AllowWriteActions);
        Assert.True(vm.Tools.Single(t => t.Name == "whats_new").IsEnabled);
        Assert.False(vm.Tools.Single(t => t.Name == "read_file").IsEnabled);
        // Applying settings read from disk doesn't write them back.
        Assert.Equal(["read_file"], Educ8McpOptions.Load(h.T.DataDir, _ => null).DisabledTools);
    }

    [Fact]
    public void Shows_running_servers_and_recent_activity()
    {
        using var h = new AppTestHost();
        var vm = Page(h);
        Assert.Equal("No AI app is using Educ8 right now.", vm.StatusText);
        Assert.Equal("No tool calls yet.", vm.ActivityEmptyText);

        // What a running server writes (here: this test process plays the server).
        var registry = new McpServerRegistry(h.T.DataDir);
        registry.Register(TestServices.T0.AddMinutes(-20));
        registry.RecordCall("claude-ai", "1.9.2", TestServices.T0.AddMinutes(-2));
        var log = new McpActivityLog(h.T.DataDir);
        log.Append(new McpActivityEntry { Time = TestServices.T0.AddMinutes(-3), Tool = "read_file", Ok = true, DurationMs = 1234, Client = "claude-ai", Arguments = """{"elementId":72}""" });
        log.Append(new McpActivityEntry { Time = TestServices.T0.AddMinutes(-2), Tool = "submit_assignment", Blocked = true, Error = "Turned off in Educ8", Client = "claude-ai" });

        vm.Refresh();
        Assert.Equal("1 AI app is using Educ8.", vm.StatusText);
        var server = Assert.Single(vm.Servers);
        Assert.Equal(("Claude Desktop 1.9.2", "Running since Today, 11:40 · 1 call · last Today, 11:58"), (server.ClientText, server.DetailText));

        Assert.Null(vm.ActivityEmptyText);
        Assert.Equal([McpCallOutcome.Blocked, McpCallOutcome.Ok], vm.Activity.Select(a => a.Outcome));
        Assert.Equal("Claude Desktop · Today, 11:58 · refused (turned off)", vm.Activity[0].DetailText);
        Assert.Equal(("Claude Desktop · Today, 11:57 · 1.2 s", """{"elementId":72}"""), (vm.Activity[1].DetailText, vm.Activity[1].ArgumentsText));

        registry.Unregister();
        vm.Refresh();
        Assert.Empty(vm.Servers);
    }

    [Fact]
    public void A_new_call_is_inserted_at_the_top_without_rebuilding_the_list()
    {
        using var h = new AppTestHost();
        var vm = Page(h);
        var log = new McpActivityLog(h.T.DataDir);
        log.Append(new McpActivityEntry { Time = TestServices.T0.AddMinutes(-5), Tool = "read_file", Ok = true });
        log.Append(new McpActivityEntry { Time = TestServices.T0.AddMinutes(-4), Tool = "get_event", Ok = true });
        vm.Refresh();

        var changes = new List<System.Collections.Specialized.NotifyCollectionChangedAction>();
        vm.Activity.CollectionChanged += (_, e) => changes.Add(e.Action);
        log.Append(new McpActivityEntry { Time = TestServices.T0.AddMinutes(-1), Tool = "whats_new", Ok = true });
        vm.Refresh();
        vm.Refresh(); // nothing new: no changes

        Assert.Equal([System.Collections.Specialized.NotifyCollectionChangedAction.Add], changes);
        Assert.Equal(["What's new", "Calendar event details", "Read a course file"], vm.Activity.Select(a => a.Title));
    }
}
