using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Itslearning;
using Educ8.Mcp;
using Educ8.Mcp.Control;
using Microsoft.Extensions.Options;

namespace Educ8.App.Core.ViewModels;

/// <summary>
/// The MCP page: controls the MCP server that clients like Claude Desktop start themselves (stdio). Settings are written to
/// config.json and picked up by running servers right away; status and activity are read from the files servers write.
/// </summary>
public sealed partial class McpViewModel : ObservableObject, IDisposable
{
    internal static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(3);
    private const int MaxActivity = 40;

    private readonly string? _dataDirectory;
    private readonly TimeProvider _time;
    private readonly IUiDispatcher _dispatcher;
    private readonly McpActivityLog _log;
    private readonly McpServerRegistry _registry;
    private ITimer? _timer;
    private bool _applying;

    public McpViewModel(IOptions<ItslearningOptions> options, TimeProvider time, IUiDispatcher dispatcher)
    {
        _dataDirectory = options.Value.DataDirectory;
        _time = time;
        _dispatcher = dispatcher;
        _log = new McpActivityLog(_dataDirectory);
        _registry = new McpServerRegistry(_dataDirectory);
        Tools = new(McpToolCatalog.All.Select(t => new McpToolItemViewModel(t, this)));
        LoadSettings();
        Refresh();
    }

    public string IntroText => Strings.McpIntro;

    /// <summary>The <c>EDUC8_ALLOW_WRITE_ACTIONS</c> environment variable decides; the switch can't change it.</summary>
    public bool IsWriteActionsOverridden { get; } = Educ8McpOptions.IsOverriddenByEnvironment();

    public string WriteActionsHint => IsWriteActionsOverridden ? Strings.McpWriteOverridden : Strings.McpWriteHint;

    /// <summary>Lets the AI use tools that change things in itslearning (submit assignments). Saved immediately.</summary>
    [ObservableProperty]
    public partial bool AllowWriteActions { get; set; }

    /// <summary>Every tool: read tools first, then write tools. Each has its own switch.</summary>
    public ObservableCollection<McpToolItemViewModel> Tools { get; }

    /// <summary>Servers running right now (started by MCP clients).</summary>
    public ObservableCollection<McpServerItemViewModel> Servers { get; } = [];

    [ObservableProperty]
    public partial string StatusText { get; private set; } = "";

    /// <summary>Recent tool calls, newest first.</summary>
    public ObservableCollection<McpActivityItemViewModel> Activity { get; } = [];

    [ObservableProperty]
    public partial string? ActivityEmptyText { get; private set; }

    partial void OnAllowWriteActionsChanged(bool value)
    {
        foreach (var t in Tools) t.OnAvailabilityChanged();
        if (_applying || IsWriteActionsOverridden) return;
        Educ8McpOptions.SaveAllowWriteActions(value, _dataDirectory);
    }

    internal void ToolSwitched()
    {
        if (_applying) return;
        Educ8McpOptions.SaveDisabledTools(Tools.Where(t => !t.IsEnabled).Select(t => t.Name), _dataDirectory);
    }

    /// <summary>Re-reads settings, running servers and recent activity.</summary>
    [RelayCommand]
    public void Refresh()
    {
        LoadSettings();
        var now = _time.GetUtcNow();
        var zone = _time.LocalTimeZone;

        var servers = _registry.ListRunning();
        Sync(Servers, servers.Select(s => new McpServerItemViewModel(s, now, zone)).ToList(), (a, b) => a.Key == b.Key);
        StatusText = servers.Count switch
        {
            0 => Strings.McpNotRunning,
            1 => Strings.McpRunningOne,
            _ => string.Format(CultureInfo.CurrentCulture, Strings.McpRunningMany, servers.Count),
        };

        var activity = _log.ReadRecent(MaxActivity);
        Sync(Activity, activity.Select(e => new McpActivityItemViewModel(e, now, zone)).ToList(), (a, b) => a.Key == b.Key);
        ActivityEmptyText = activity.Count == 0 ? Strings.McpNoActivity : null;
    }

    /// <summary>Refreshes every few seconds while the page is shown (servers come and go, calls happen).</summary>
    public void StartAutoRefresh()
    {
        Refresh();
        _timer ??= _time.CreateTimer(_ => _dispatcher.Post(Refresh), null, RefreshInterval, RefreshInterval);
    }

    public void StopAutoRefresh()
    {
        _timer?.Dispose();
        _timer = null;
    }

    public void Dispose() => StopAutoRefresh();

    private void LoadSettings()
    {
        var options = Educ8McpOptions.Load(_dataDirectory);
        _applying = true;
        try
        {
            AllowWriteActions = options.AllowWriteActions;
            foreach (var t in Tools) t.IsEnabled = !options.DisabledTools.Contains(t.Name);
        }
        finally
        {
            _applying = false;
        }
    }

    /// <summary>
    /// Updates the list with as few changes as possible, so live lists don't redraw (and lose their scroll position):
    /// nothing when unchanged; when new items arrived at the top (newest first), inserts just those and drops what fell
    /// off the end; otherwise replaces everything.
    /// </summary>
    internal static void Sync<T>(ObservableCollection<T> target, IReadOnlyList<T> items, Func<T, T, bool> same)
    {
        if (target.Count == items.Count && target.Zip(items).All(p => same(p.First, p.Second))) return;
        var firstOld = target.Count == 0 ? -1 : items.ToList().FindIndex(i => same(i, target[0]));
        if (firstOld > 0 && items.Skip(firstOld).Zip(target).All(p => same(p.First, p.Second)))
        {
            for (var i = firstOld - 1; i >= 0; i--) target.Insert(0, items[i]);
            while (target.Count > items.Count) target.RemoveAt(target.Count - 1);
            return;
        }
        target.Clear();
        foreach (var item in items) target.Add(item);
    }
}

/// <summary>One tool with its on/off switch. Write tools only work while write actions are allowed.</summary>
public sealed partial class McpToolItemViewModel : ObservableObject
{
    private readonly McpViewModel _page;

    internal McpToolItemViewModel(McpToolInfo tool, McpViewModel page)
    {
        _page = page;
        Name = tool.Name;
        Title = tool.Title;
        IsWrite = tool.IsWrite;
        Summary = FirstSentence(tool.Description);
    }

    public string Name { get; }
    public string Title { get; }
    public bool IsWrite { get; }

    /// <summary>The tool description's first sentence.</summary>
    public string Summary { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAvailable))]
    public partial bool IsEnabled { get; set; } = true;

    /// <summary>Whether clients can use it: switched on, and for write tools, write actions allowed.</summary>
    public bool IsAvailable => IsEnabled && (!IsWrite || _page.AllowWriteActions);

    /// <summary>Write tools' switches only matter while write actions are allowed.</summary>
    public bool CanSwitch => !IsWrite || _page.AllowWriteActions;

    partial void OnIsEnabledChanged(bool value) => _page.ToolSwitched();

    internal void OnAvailabilityChanged()
    {
        OnPropertyChanged(nameof(IsAvailable));
        OnPropertyChanged(nameof(CanSwitch));
    }

    /// <summary>Up to the first ". ", ": " or line break (a colon becomes a full stop); at most 160 characters.</summary>
    private static string FirstSentence(string text)
    {
        var ends = new[] { text.IndexOf(". ", StringComparison.Ordinal), text.IndexOf(": ", StringComparison.Ordinal), text.IndexOf('\n') }
            .Where(i => i > 0).DefaultIfEmpty(-1).Min();
        var sentence = (ends > 0 ? text[..ends] : text).TrimEnd('.', ' ') + ".";
        return sentence.Length <= 160 ? sentence : sentence[..157] + "…";
    }
}

/// <summary>A running server: which client started it and how busy it's been.</summary>
public sealed class McpServerItemViewModel(McpServerProcess server, DateTimeOffset now, TimeZoneInfo zone)
{
    internal string Key { get; } = $"{server.ProcessId}/{server.Calls}/{server.Client}";

    /// <summary>"Claude Desktop" (or the client's own name), or "MCP client" before the first request.</summary>
    public string ClientText { get; } = Texts.McpClient(server.Client) + (server.ClientVersion is { Length: > 0 } v ? $" {v}" : "");

    /// <summary>"Running since Today, 19:40 · 12 calls · last Today, 19:44".</summary>
    public string DetailText { get; } = string.Join(" · ", new[]
    {
        string.Format(CultureInfo.CurrentCulture, Strings.McpRunningSince, Texts.Date(server.StartedAt, now, zone)),
        string.Format(CultureInfo.CurrentCulture, server.Calls == 1 ? Strings.McpCallsOne : Strings.McpCalls, server.Calls),
        server.LastCallAt is { } last ? string.Format(CultureInfo.CurrentCulture, Strings.McpLastCall, Texts.Date(last, now, zone)) : null,
    }.Where(s => s is not null));
}

/// <summary>One tool call in the activity list.</summary>
public sealed class McpActivityItemViewModel(McpActivityEntry entry, DateTimeOffset now, TimeZoneInfo zone)
{
    internal string Key { get; } = $"{entry.Time:O}/{entry.ProcessId}/{entry.Tool}";

    public string Title { get; } = McpToolCatalog.Find(entry.Tool)?.Title ?? entry.Tool;

    /// <summary>"Claude Desktop · Today, 19:44 · 1.2 s".</summary>
    public string DetailText { get; } = string.Join(" · ", new[]
    {
        Texts.McpClient(entry.Client),
        Texts.Date(entry.Time, now, zone),
        entry.Blocked ? Strings.McpBlocked : Texts.Duration(entry.DurationMs),
    });

    /// <summary>The arguments the AI passed (shortened JSON), if any.</summary>
    public string ArgumentsText { get; } = entry.Arguments ?? "";
    public string? ErrorText { get; } = entry.Ok ? null : entry.Error;
    public McpCallOutcome Outcome { get; } = entry.Blocked ? McpCallOutcome.Blocked : entry.Ok ? McpCallOutcome.Ok : McpCallOutcome.Failed;
}

public enum McpCallOutcome
{
    Ok,
    Failed,
    Blocked,
}
