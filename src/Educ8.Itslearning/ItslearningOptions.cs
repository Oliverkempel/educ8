namespace Educ8.Itslearning;

/// <summary>Settings for talking to one itslearning site. Shared by every host (MCP server, desktop app).</summary>
public sealed class ItslearningOptions
{
    public const string DefaultRedirectUri = "itsl-itslearning://login";
    public const string DefaultScope = "All";

    /// <summary>Your school's itslearning site, e.g. https://yourschool.itslearning.com. Required.</summary>
    public Uri? BaseUrl { get; set; }

    /// <summary>The configured site; throws with a helpful message when it isn't set.</summary>
    public Uri Site => BaseUrl ?? throw new InvalidOperationException(NotConfigured("BaseUrl", ItslearningSettingsLoader.BaseUrlEnvVar));

    /// <summary>OAuth2 client id. Required; never hard-coded.</summary>
    public string ClientId { get; set; } = "";

    /// <summary>Registered OAuth2 redirect URI.</summary>
    public string RedirectUri { get; set; } = DefaultRedirectUri;

    /// <summary>Space-separated OAuth2 scopes.</summary>
    public string Scope { get; set; } = DefaultScope;

    /// <summary>Per-user directory for config, tokens and login callbacks.</summary>
    public string DataDirectory { get; set; } = Educ8Paths.DefaultDataDirectory;

    /// <summary>Throws if required settings are missing.</summary>
    public void Validate()
    {
        if (BaseUrl is null)
            throw new InvalidOperationException(NotConfigured("BaseUrl", ItslearningSettingsLoader.BaseUrlEnvVar));
        if (string.IsNullOrWhiteSpace(ClientId))
            throw new InvalidOperationException(NotConfigured("ClientId", ItslearningSettingsLoader.ClientIdEnvVar));
        if (string.IsNullOrWhiteSpace(RedirectUri))
            throw new InvalidOperationException("itslearning RedirectUri is not configured.");
    }

    private string NotConfigured(string key, string envVar) =>
        $"itslearning {key} is not configured. Set {envVar} or \"{key}\" in {Path.Combine(DataDirectory, Educ8Paths.ConfigFileName)}.";
}
