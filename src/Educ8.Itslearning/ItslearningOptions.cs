namespace Educ8.Itslearning;

/// <summary>Settings for talking to one itslearning site. Shared by every host (MCP server, desktop app).</summary>
public sealed class ItslearningOptions
{
    public const string DefaultBaseUrl = "https://sdu.itslearning.com";
    public const string DefaultRedirectUri = "itsl-itslearning://login";
    public const string DefaultScope = "All";

    /// <summary>Site base URL, e.g. https://sdu.itslearning.com (no trailing slash needed).</summary>
    public Uri BaseUrl { get; set; } = new(DefaultBaseUrl);

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
        if (string.IsNullOrWhiteSpace(ClientId))
            throw new InvalidOperationException(
                $"itslearning ClientId is not configured. Set ITSLEARNING_CLIENT_ID or \"ClientId\" in {Path.Combine(DataDirectory, Educ8Paths.ConfigFileName)}.");
        if (string.IsNullOrWhiteSpace(RedirectUri))
            throw new InvalidOperationException("itslearning RedirectUri is not configured.");
    }
}
