using System.Text.Json;
using System.Text.Json.Nodes;
using Educ8.Itslearning;

namespace Educ8.Mcp;

/// <summary>
/// Settings for the MCP tool set (not for itslearning access itself), stored in <c>&lt;data dir&gt;/config.json</c>.
/// The desktop app changes them; a running server picks the changes up (<see cref="Control.McpSettingsWatcher"/>).
/// </summary>
public sealed class Educ8McpOptions
{
    public const string AllowWriteActionsKey = "AllowWriteActions";
    public const string DisabledToolsKey = "DisabledTools";
    public const string AllowWriteActionsEnvVar = "EDUC8_ALLOW_WRITE_ACTIONS";

    private volatile IReadOnlySet<string> _disabledTools = new HashSet<string>(StringComparer.Ordinal);
    private volatile bool _allowWriteActions;

    /// <summary>
    /// Allows tools that change anything in itslearning (submitting assignments, uploading files, ...).
    /// Off by default. When off, those tools are hidden from clients and refused if called anyway.
    /// </summary>
    public bool AllowWriteActions
    {
        get => _allowWriteActions;
        set => _allowWriteActions = value;
    }

    /// <summary>Tools the user turned off (by tool name). Hidden from clients and refused if called anyway.</summary>
    public IReadOnlySet<string> DisabledTools
    {
        get => _disabledTools;
        set => _disabledTools = new HashSet<string>(value, StringComparer.Ordinal);
    }

    /// <summary>Whether clients may see and call <paramref name="toolName"/> right now.</summary>
    public bool IsAvailable(string toolName) =>
        !DisabledTools.Contains(toolName) && (AllowWriteActions || Control.McpToolCatalog.Find(toolName)?.IsWrite != true);

    /// <summary>Takes the values of <paramref name="other"/> (used when config.json changes under a running server).</summary>
    public void CopyFrom(Educ8McpOptions other)
    {
        AllowWriteActions = other.AllowWriteActions;
        DisabledTools = other.DisabledTools;
    }

    /// <summary>Defaults → <c>&lt;data dir&gt;/config.json</c> → environment (<see cref="AllowWriteActionsEnvVar"/>).</summary>
    public static Educ8McpOptions Load(string? dataDirectory = null, Func<string, string?>? getEnv = null)
    {
        getEnv ??= Environment.GetEnvironmentVariable;
        var options = new Educ8McpOptions();
        var config = ReadConfig(ConfigPath(dataDirectory));
        if (config?[AllowWriteActionsKey] is JsonValue v && v.TryGetValue<bool>(out var fromFile)) options.AllowWriteActions = fromFile;
        if (config?[DisabledToolsKey] is JsonArray disabled)
            options.DisabledTools = disabled.OfType<JsonValue>().Select(d => d.TryGetValue<string>(out var s) ? s : null).OfType<string>().ToHashSet();
        if (ParseBool(getEnv(AllowWriteActionsEnvVar)) is { } fromEnv) options.AllowWriteActions = fromEnv;
        return options;
    }

    /// <summary>Turns write actions on/off in <c>config.json</c>, keeping the other settings.</summary>
    public static void SaveAllowWriteActions(bool allow, string? dataDirectory = null) =>
        Update(dataDirectory, config => config[AllowWriteActionsKey] = allow);

    /// <summary>Stores which tools are turned off in <c>config.json</c>, keeping the other settings.</summary>
    public static void SaveDisabledTools(IEnumerable<string> toolNames, string? dataDirectory = null) =>
        Update(dataDirectory, config => config[DisabledToolsKey] = new JsonArray(toolNames.Distinct().Order(StringComparer.Ordinal).Select(n => (JsonNode?)n).ToArray()));

    /// <summary>True when the environment variable overrides config.json (so changes to the file won't matter).</summary>
    public static bool IsOverriddenByEnvironment(Func<string, string?>? getEnv = null) =>
        ParseBool((getEnv ?? Environment.GetEnvironmentVariable)(AllowWriteActionsEnvVar)) is not null;

    public static string ConfigPath(string? dataDirectory) =>
        Path.Combine(dataDirectory ?? Educ8Paths.DefaultDataDirectory, Educ8Paths.ConfigFileName);

    private static void Update(string? dataDirectory, Action<JsonObject> change)
    {
        var path = ConfigPath(dataDirectory);
        Educ8Paths.EnsureDirectory(Path.GetDirectoryName(path)!);
        var config = ReadConfig(path) ?? new JsonObject();
        change(config);
        // Write a temp file and move it over, so a server reading config.json at the same moment never sees half a file.
        var temp = path + ".tmp";
        File.WriteAllText(temp, config.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) + "\n");
        File.Move(temp, path, overwrite: true);
    }

    private static JsonObject? ReadConfig(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            return JsonNode.Parse(File.ReadAllText(path), documentOptions: new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }) as JsonObject;
        }
        catch (Exception e) when (e is JsonException or IOException)
        {
            return null;
        }
    }

    private static bool? ParseBool(string? value) => value?.Trim().ToLowerInvariant() switch
    {
        "1" or "true" or "yes" or "on" => true,
        "0" or "false" or "no" or "off" => false,
        _ => null,
    };
}
