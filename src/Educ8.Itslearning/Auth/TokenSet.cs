using System.Text.Json.Serialization;

namespace Educ8.Itslearning.Auth;

/// <summary>An access/refresh token pair. The refresh token rotates on every refresh, so always persist the newest set.</summary>
public sealed record TokenSet(
    [property: JsonPropertyName("access_token")] string AccessToken,
    [property: JsonPropertyName("refresh_token")] string RefreshToken,
    [property: JsonPropertyName("expires_at")] DateTimeOffset ExpiresAt)
{
    /// <summary>True if the access token is still usable for at least <paramref name="margin"/>.</summary>
    public bool IsValidAt(DateTimeOffset now, TimeSpan margin) => ExpiresAt - margin > now;

    public override string ToString() => $"TokenSet(expires {ExpiresAt:u})"; // never print tokens
}
