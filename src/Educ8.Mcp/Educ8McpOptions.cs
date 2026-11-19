using System.Text.Json;
using System.Text.Json.Nodes;
using Educ8.Itslearning;

namespace Educ8.Mcp;

/// <summary>Settings for the MCP tool set (not for itslearning access itself).</summary>
public sealed class Educ8McpOptions
{
    public const string AllowWriteActionsKey = "AllowWriteActions";
    public const string AllowWriteActionsEnvVar = "EDUC8_ALLOW_WRITE_ACTIONS";

    /// <summary>
    /// Allows tools that change anything in itslearning (submitting assignments, uploading files, ...).
    /// Off by default. When off, those tools aren't even registered, so the model can't see or call them.
    /// </summary>
    public bool AllowWriteActions { get; set; }

    /// <summary>Defaults → <c>&lt;data dir&gt;/config.json</c> → environment (<see cref="AllowWriteActionsEnvVar"/>).</summary>
    public static Educ8McpOptions Load(string? dataDirectory = null, Func<string, string?>? getEnv = null)
    {
        getEnv ??= Environment.GetEnvironmentVariable;
        var options = new Educ8McpOptions();
        var config = ReadConfig(ConfigPath(dataDirectory));
        if (config?[AllowWriteActionsKey] is JsonValue v && v.TryGetValue<bool>(out var fromFile)) options.AllowWriteActions = fromFile;
        if (ParseBool(getEnv(AllowWriteActionsEnvVar)) is { } fromEnv) options.AllowWriteActions = fromEnv;
        return options;
    }

    /// <summary>Turns write actions on/off in <c>config.json</c>, keeping the other settings.</summary>
    public static void SaveAllowWriteActions(bool allow, string? dataDirectory = null)
    {
        var path = ConfigPath(dataDirectory);
        Educ8Paths.EnsureDirectory(Path.GetDirectoryName(path)!);
        var config = ReadConfig(path) ?? new JsonObject();
        config[AllowWriteActionsKey] = allow;
        File.WriteAllText(path, config.ToJsonString(new JsonSerializerOptions { WriteIndented = true }) + "\n");
    }

    /// <summary>True when the environment variable overrides config.json (so CLI changes to the file won't matter).</summary>
    public static bool IsOverriddenByEnvironment(Func<string, string?>? getEnv = null) =>
        ParseBool((getEnv ?? Environment.GetEnvironmentVariable)(AllowWriteActionsEnvVar)) is not null;

    private static string ConfigPath(string? dataDirectory) =>
        Path.Combine(dataDirectory ?? Educ8Paths.DefaultDataDirectory, Educ8Paths.ConfigFileName);

    private static JsonObject? ReadConfig(string path)
    {
        if (!File.Exists(path)) return null;
        try
        {
            return JsonNode.Parse(File.ReadAllText(path), documentOptions: new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true }) as JsonObject;
        }
        catch (JsonException)
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
