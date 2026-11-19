using System.Diagnostics;
using System.Text.Json;
using Educ8.Itslearning;

namespace Educ8.Mcp.Control;

/// <summary>One tool call, as recorded for the desktop app's activity list. Stays on this machine.</summary>
public sealed record McpActivityEntry
{
    public DateTimeOffset Time { get; init; }
    public string Tool { get; init; } = "";
    public bool Ok { get; init; }

    /// <summary>True when the call was refused because the tool is turned off.</summary>
    public bool Blocked { get; init; }
    public long DurationMs { get; init; }

    /// <summary>The MCP client's name ("claude-ai", "claude-code", …), if it sent one.</summary>
    public string? Client { get; init; }

    /// <summary>The call's arguments as JSON, shortened.</summary>
    public string? Arguments { get; init; }

    /// <summary>The start of the error text, for failed calls.</summary>
    public string? Error { get; init; }
    public int ProcessId { get; init; }
}

/// <summary>Where the MCP filters report tool calls. The stdio host registers <see cref="McpActivityRecorder"/>; tests may register nothing.</summary>
public interface IMcpActivitySink
{
    void Record(McpActivityEntry entry, string? clientVersion);
}

/// <summary>
/// The activity log: <c>&lt;data dir&gt;/mcp/activity.jsonl</c>, one JSON object per line, appended by every running server
/// and read by the desktop app. Trimmed to the newest entries when it grows past <see cref="MaxBytes"/>.
/// </summary>
public sealed class McpActivityLog(string? dataDirectory = null)
{
    public const long MaxBytes = 512 * 1024;
    public const int KeepAfterTrim = 300;
    private const int MaxArgumentChars = 300;
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public string Path { get; } = System.IO.Path.Combine(McpPaths.Directory(dataDirectory), "activity.jsonl");

    public void Append(McpActivityEntry entry)
    {
        Educ8Paths.EnsureDirectory(System.IO.Path.GetDirectoryName(Path)!);
        var line = JsonSerializer.Serialize(entry, Json) + "\n";
        try
        {
            using (var file = new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            using (var writer = new StreamWriter(file))
                writer.Write(line);
            if (new FileInfo(Path).Length > MaxBytes) Trim();
        }
        catch (IOException)
        {
            // Another process is trimming or the disk is unhappy: losing one activity line is fine.
        }
    }

    /// <summary>The newest entries first (unreadable lines skipped).</summary>
    public IReadOnlyList<McpActivityEntry> ReadRecent(int max = 50)
    {
        if (!File.Exists(Path)) return [];
        string[] lines;
        try
        {
            using var file = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            using var reader = new StreamReader(file);
            lines = reader.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }
        catch (IOException)
        {
            return [];
        }
        var entries = new List<McpActivityEntry>();
        for (var i = lines.Length - 1; i >= 0 && entries.Count < max; i--)
        {
            try
            {
                if (JsonSerializer.Deserialize<McpActivityEntry>(lines[i], Json) is { Tool.Length: > 0 } e) entries.Add(e);
            }
            catch (JsonException)
            {
            }
        }
        return entries;
    }

    /// <summary>Shortens a call's arguments for the log.</summary>
    public static string? Summarize(IReadOnlyDictionary<string, JsonElement>? arguments)
    {
        if (arguments is null || arguments.Count == 0) return null;
        var text = JsonSerializer.Serialize(arguments, Json);
        return text.Length <= MaxArgumentChars ? text : text[..MaxArgumentChars] + "…";
    }

    /// <summary>Keeps the newest entries: at most <see cref="KeepAfterTrim"/>, and at most half of <see cref="MaxBytes"/>.</summary>
    private void Trim()
    {
        var keep = new List<string>();
        long bytes = 0;
        foreach (var line in ReadRecent(KeepAfterTrim).Select(e => JsonSerializer.Serialize(e, Json)))
        {
            bytes += line.Length + 1;
            if (bytes > MaxBytes / 2) break;
            keep.Add(line);
        }
        keep.Reverse();
        var temp = Path + "." + Environment.ProcessId + ".tmp";
        File.WriteAllText(temp, string.Join("\n", keep) + "\n");
        File.Move(temp, Path, overwrite: true);
    }
}

/// <summary>A running MCP server process, as the desktop app's status list shows it.</summary>
public sealed record McpServerProcess
{
    public int ProcessId { get; init; }
    public DateTimeOffset StartedAt { get; init; }
    public string? Client { get; init; }
    public string? ClientVersion { get; init; }
    public DateTimeOffset? LastCallAt { get; init; }
    public int Calls { get; init; }
}

/// <summary>
/// Running servers announce themselves in <c>&lt;data dir&gt;/mcp/servers/&lt;pid&gt;.json</c> (written at start and after
/// each call, deleted on exit). Files of processes that are gone (crashes) are ignored and cleaned up.
/// </summary>
public sealed class McpServerRegistry(string? dataDirectory = null)
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);
    private readonly Lock _gate = new();
    private McpServerProcess _self = new() { ProcessId = Environment.ProcessId };

    public string Directory { get; } = Path.Combine(McpPaths.Directory(dataDirectory), "servers");

    private string SelfPath => Path.Combine(Directory, $"{Environment.ProcessId}.json");

    public void Register(DateTimeOffset now)
    {
        lock (_gate)
        {
            _self = _self with { StartedAt = now };
            Write();
        }
    }

    public void RecordCall(string? client, string? clientVersion, DateTimeOffset at)
    {
        lock (_gate)
        {
            _self = _self with { Client = client ?? _self.Client, ClientVersion = clientVersion ?? _self.ClientVersion, LastCallAt = at, Calls = _self.Calls + 1 };
            Write();
        }
    }

    public void Unregister()
    {
        try { File.Delete(SelfPath); } catch (IOException) { }
    }

    /// <summary>Servers whose process is still running, oldest first.</summary>
    public IReadOnlyList<McpServerProcess> ListRunning()
    {
        if (!System.IO.Directory.Exists(Directory)) return [];
        var running = new List<McpServerProcess>();
        foreach (var file in System.IO.Directory.EnumerateFiles(Directory, "*.json"))
        {
            McpServerProcess? server = null;
            try { server = JsonSerializer.Deserialize<McpServerProcess>(File.ReadAllText(file), Json); }
            catch (Exception e) when (e is JsonException or IOException) { }
            if (server is not null && IsAlive(server.ProcessId)) running.Add(server);
            else if (server is not null)
            {
                try { File.Delete(file); } catch (IOException) { } // left behind by a crashed server
            }
        }
        return running.OrderBy(s => s.StartedAt).ToList();
    }

    private void Write()
    {
        try
        {
            Educ8Paths.EnsureDirectory(Directory);
            var temp = SelfPath + ".tmp";
            File.WriteAllText(temp, JsonSerializer.Serialize(_self, Json));
            File.Move(temp, SelfPath, overwrite: true);
        }
        catch (IOException)
        {
        }
    }

    private static bool IsAlive(int pid)
    {
        try
        {
            using var p = Process.GetProcessById(pid);
            return !p.HasExited;
        }
        catch (Exception e) when (e is ArgumentException or InvalidOperationException)
        {
            return false;
        }
    }
}

/// <summary>Records tool calls in the activity log and this server's status file.</summary>
public sealed class McpActivityRecorder(McpActivityLog log, McpServerRegistry registry) : IMcpActivitySink
{
    public void Record(McpActivityEntry entry, string? clientVersion)
    {
        log.Append(entry);
        registry.RecordCall(entry.Client, clientVersion, entry.Time);
    }
}

internal static class McpPaths
{
    public static string Directory(string? dataDirectory) => Path.Combine(dataDirectory ?? Educ8Paths.DefaultDataDirectory, "mcp");
}
