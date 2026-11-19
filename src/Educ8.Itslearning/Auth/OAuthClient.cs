using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Educ8.Itslearning.Auth;

/// <summary>Talks to itslearning's OAuth2 endpoints (authorization code flow, refresh). Holds no state.</summary>
public sealed class OAuthClient(HttpClient http, IOptions<ItslearningOptions> options, TimeProvider time)
{
    private readonly ItslearningOptions _options = options.Value;

    /// <summary>The URL the user opens to sign in (itslearning login page → SDU Office 365 → back with a code).</summary>
    public Uri BuildAuthorizeUrl(string state)
    {
        var query = string.Join('&',
            Pair("client_id", _options.ClientId),
            Pair("state", state),
            Pair("response_type", "code"),
            Pair("scope", _options.Scope),
            Pair("redirect_uri", _options.RedirectUri));
        return new Uri(_options.BaseUrl, "oauth2/authorize.aspx?" + query);

        static string Pair(string k, string v) => $"{k}={Uri.EscapeDataString(v)}";
    }

    public Task<TokenSet> ExchangeCodeAsync(string code, CancellationToken ct = default) =>
        RequestAsync(new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = _options.RedirectUri,
        }, ct);

    public Task<TokenSet> RefreshAsync(string refreshToken, CancellationToken ct = default) =>
        RequestAsync(new Dictionary<string, string>
        {
            ["client_id"] = _options.ClientId,
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken,
        }, ct);

    private async Task<TokenSet> RequestAsync(Dictionary<string, string> form, CancellationToken ct)
    {
        using var response = await http.PostAsync(new Uri(_options.BaseUrl, "restapi/oauth2/token"), new FormUrlEncodedContent(form), ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            string? error = null, description = null;
            try
            {
                using var doc = JsonDocument.Parse(body);
                error = doc.RootElement.TryGetProperty("error", out var e) ? e.GetString() : null;
                description = doc.RootElement.TryGetProperty("error_description", out var d) ? d.GetString() : null;
            }
            catch (JsonException) { /* non-JSON error body */ }
            throw new OAuthException(response.StatusCode, error,
                $"itslearning token endpoint returned {(int)response.StatusCode}{(error is null ? "" : $" ({error})")}{(description is null ? "" : $": {description}")}");
        }

        var token = await response.Content.ReadFromJsonAsync<TokenResponse>(ct).ConfigureAwait(false)
                    ?? throw new OAuthException(response.StatusCode, null, "Empty token response.");
        if (string.IsNullOrEmpty(token.AccessToken) || string.IsNullOrEmpty(token.RefreshToken))
            throw new OAuthException(response.StatusCode, null, "Token response is missing access_token or refresh_token.");
        return new TokenSet(token.AccessToken, token.RefreshToken, time.GetUtcNow().AddSeconds(token.ExpiresIn));
    }

    private sealed record TokenResponse(
        [property: JsonPropertyName("access_token")] string? AccessToken,
        [property: JsonPropertyName("token_type")] string? TokenType,
        [property: JsonPropertyName("expires_in")] int ExpiresIn,
        [property: JsonPropertyName("refresh_token")] string? RefreshToken);
}
