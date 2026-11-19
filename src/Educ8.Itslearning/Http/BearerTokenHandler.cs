using System.Net;
using System.Net.Http.Headers;
using Educ8.Itslearning.Auth;

namespace Educ8.Itslearning.Http;

/// <summary>Adds the bearer token; on 401 refreshes once and retries.</summary>
public sealed class BearerTokenHandler(IAccessTokenProvider tokens) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        // Buffer the body so the request can be re-sent after a refresh.
        byte[]? body = null;
        if (request.Content is not null)
            body = await request.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await tokens.GetAccessTokenAsync(false, ct).ConfigureAwait(false));
        var response = await base.SendAsync(request, ct).ConfigureAwait(false);
        if (response.StatusCode != HttpStatusCode.Unauthorized) return response;

        response.Dispose();
        using var retry = Clone(request, body);
        retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await tokens.GetAccessTokenAsync(true, ct).ConfigureAwait(false));
        return await base.SendAsync(retry, ct).ConfigureAwait(false);
    }

    private static HttpRequestMessage Clone(HttpRequestMessage original, byte[]? body)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri) { Version = original.Version };
        foreach (var header in original.Headers.Where(h => h.Key != "Authorization"))
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        if (body is not null)
        {
            clone.Content = new ByteArrayContent(body);
            foreach (var header in original.Content!.Headers)
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
        return clone;
    }
}
