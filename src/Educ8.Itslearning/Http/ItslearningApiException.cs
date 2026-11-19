using System.Net;

namespace Educ8.Itslearning.Http;

/// <summary>An itslearning REST call failed with a non-success status.</summary>
public sealed class ItslearningApiException(HttpStatusCode statusCode, string path, string? detail)
    : Exception($"itslearning API {(int)statusCode} {statusCode} for {path}{(string.IsNullOrWhiteSpace(detail) ? "" : $": {detail}")}")
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    /// <summary>The request path (no query string, which may contain search text).</summary>
    public string Path { get; } = path;
}
