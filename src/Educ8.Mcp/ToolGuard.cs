using System.Net;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using ModelContextProtocol;

namespace Educ8.Mcp;

/// <summary>
/// Runs tool bodies: first picks up a login finished in another process (URL handler), then maps itslearning
/// failures to <see cref="McpException"/>s whose messages the model can act on.
/// </summary>
internal static class ToolGuard
{
    public const string LoginHint = "Call the `login` tool to sign in to itslearning.";

    public static async Task<T> RunAsync<T>(LoginService login, Func<Task<T>> body, CancellationToken ct)
    {
        try
        {
            await login.TryCompleteFromInboxAsync(ct).ConfigureAwait(false);
            return await body().ConfigureAwait(false);
        }
        catch (NotAuthenticatedException e)
        {
            throw new McpException($"{e.Message} {LoginHint}");
        }
        catch (LoginException e)
        {
            throw new McpException(e.Message);
        }
        catch (Educ8.Itslearning.Content.ContentUnavailableException e)
        {
            throw new McpException($"{e.Message} The user can open the item in itslearning instead.");
        }
        catch (ItslearningApiException e)
        {
            throw new McpException(e.StatusCode switch
            {
                HttpStatusCode.Forbidden => $"You don't have access to that in itslearning ({e.Path}).",
                HttpStatusCode.NotFound => $"Not found in itslearning, or you don't have access ({e.Path}).",
                _ => $"itslearning returned an error ({(int)e.StatusCode}) for {e.Path}. Try again; if it persists the item may not be available through the API.",
            });
        }
        catch (FormatException e)
        {
            throw new McpException(e.Message); // e.g. a bad page range
        }
        catch (HttpRequestException e)
        {
            throw new McpException($"Could not reach itslearning: {e.Message}");
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new McpException("itslearning took too long to respond. Try again.");
        }
    }
}
