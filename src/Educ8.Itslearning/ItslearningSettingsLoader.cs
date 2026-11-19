using System.Text.Json;

namespace Educ8.Itslearning;

/// <summary>
/// Builds <see cref="ItslearningOptions"/> from defaults, then <c>&lt;data dir&gt;/config.json</c>, then environment variables.
/// Hosts without their own configuration system (console host, desktop app) use this so they read the same settings.
/// </summary>
public static class ItslearningSettingsLoader
{
    public const string BaseUrlEnvVar = "ITSLEARNING_BASE_URL";
    public const string ClientIdEnvVar = "ITSLEARNING_CLIENT_ID";
    public const string RedirectUriEnvVar = "ITSLEARNING_REDIRECT_URI";
    public const string ScopeEnvVar = "ITSLEARNING_SCOPE";

    public static ItslearningOptions Load(string? dataDirectory = null, Func<string, string?>? getEnv = null)
    {
        getEnv ??= Environment.GetEnvironmentVariable;
        var options = new ItslearningOptions { DataDirectory = dataDirectory ?? Educ8Paths.DefaultDataDirectory };

        var configPath = Path.Combine(options.DataDirectory, Educ8Paths.ConfigFileName);
        if (File.Exists(configPath))
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(configPath), new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
            var root = doc.RootElement;
            if (Str(root, "BaseUrl") is { } baseUrl) options.BaseUrl = new Uri(baseUrl);
            if (Str(root, "ClientId") is { } clientId) options.ClientId = clientId;
            if (Str(root, "RedirectUri") is { } redirect) options.RedirectUri = redirect;
            if (Str(root, "Scope") is { } scope) options.Scope = scope;
        }

        if (getEnv(BaseUrlEnvVar) is { Length: > 0 } envBase) options.BaseUrl = new Uri(envBase);
        if (getEnv(ClientIdEnvVar) is { Length: > 0 } envClient) options.ClientId = envClient;
        if (getEnv(RedirectUriEnvVar) is { Length: > 0 } envRedirect) options.RedirectUri = envRedirect;
        if (getEnv(ScopeEnvVar) is { Length: > 0 } envScope) options.Scope = envScope;
        return options;
    }

    private static string? Str(JsonElement root, string name) =>
        root.ValueKind == JsonValueKind.Object && root.TryGetProperty(name, out var v) && v.ValueKind == JsonValueKind.String && v.GetString() is { Length: > 0 } s
            ? s
            : null;
}
