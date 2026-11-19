using Microsoft.Extensions.Logging;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Control;

/// <summary>The connected MCP session, remembered from the first request so the server can notify the client later.</summary>
public sealed class McpSessionTracker
{
    public McpServer? Server { get; set; }
}

/// <summary>
/// Watches <c>config.json</c> while the server runs. When the desktop app changes the MCP settings, the shared
/// <see cref="Educ8McpOptions"/> instance is updated and the client is told that the tool list changed, so new settings
/// apply without restarting the client.
/// </summary>
public sealed class McpSettingsWatcher : IDisposable
{
    private readonly Educ8McpOptions _options;
    private readonly McpSessionTracker _session;
    private readonly string? _dataDirectory;
    private readonly ILogger _logger;
    private readonly FileSystemWatcher _watcher;
    private readonly Timer _debounce;

    public McpSettingsWatcher(Educ8McpOptions options, McpSessionTracker session, string? dataDirectory, ILogger logger)
    {
        _options = options;
        _session = session;
        _dataDirectory = dataDirectory;
        _logger = logger;
        var path = Educ8McpOptions.ConfigPath(dataDirectory);
        Itslearning.Educ8Paths.EnsureDirectory(Path.GetDirectoryName(path)!);
        _debounce = new Timer(_ => _ = ReloadAsync(), null, Timeout.Infinite, Timeout.Infinite);
        _watcher = new FileSystemWatcher(Path.GetDirectoryName(path)!, Path.GetFileName(path))
        {
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Size,
        };
        _watcher.Changed += (_, _) => _debounce.Change(250, Timeout.Infinite);
        _watcher.Created += (_, _) => _debounce.Change(250, Timeout.Infinite);
        _watcher.Renamed += (_, _) => _debounce.Change(250, Timeout.Infinite);
        _watcher.EnableRaisingEvents = true;
    }

    /// <summary>Re-reads the settings; if what clients may use changed, sends <c>notifications/tools/list_changed</c>.</summary>
    public async Task ReloadAsync()
    {
        try
        {
            var before = McpToolCatalog.All.Where(t => _options.IsAvailable(t.Name)).Select(t => t.Name).ToHashSet();
            _options.CopyFrom(Educ8McpOptions.Load(_dataDirectory));
            var after = McpToolCatalog.All.Where(t => _options.IsAvailable(t.Name)).Select(t => t.Name).ToHashSet();
            if (before.SetEquals(after)) return;
            _logger.LogInformation("MCP settings changed: {Count} tools available, write actions {Write}.", after.Count, _options.AllowWriteActions ? "on" : "off");
            if (_session.Server is { } server)
                await server.SendNotificationAsync(NotificationMethods.ToolListChangedNotification);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Couldn't apply changed MCP settings.");
        }
    }

    public void Dispose()
    {
        _watcher.Dispose();
        _debounce.Dispose();
    }
}
