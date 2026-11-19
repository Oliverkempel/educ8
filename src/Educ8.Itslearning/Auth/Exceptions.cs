using System.Net;

namespace Educ8.Itslearning.Auth;

/// <summary>No usable itslearning session: never logged in, logged out, or the refresh token expired.</summary>
public sealed class NotAuthenticatedException(string message) : Exception(message);

/// <summary>The OAuth2 token endpoint rejected a request.</summary>
public sealed class OAuthException(HttpStatusCode statusCode, string? error, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    /// <summary>OAuth2 error code from the response body (e.g. <c>invalid_grant</c>), if any.</summary>
    public string? Error { get; } = error;

    /// <summary>True when the grant (code or refresh token) is no longer valid and the user must log in again.</summary>
    public bool IsGrantRejected => StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized;
}

/// <summary>A login could not be completed (bad redirect URL, unknown/expired state, ...).</summary>
public sealed class LoginException(string message) : Exception(message);
