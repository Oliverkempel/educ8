using System.Collections.Concurrent;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace Educ8.Itslearning.Auth;

/// <summary>A login that has been started and is waiting for its redirect.</summary>
public sealed record PendingLogin(string State, Uri AuthorizeUrl, DateTimeOffset StartedAt);

/// <summary>
/// Runs the OAuth2 authorization code flow independent of any UI:
/// <see cref="Begin"/> gives the URL to show; the host captures the <c>itsl-itslearning://login/?…</c> redirect
/// (WebView in the desktop app, URL-scheme handler or user paste for the MCP server) and passes it to
/// <see cref="CompleteAsync"/>.
/// </summary>
public sealed class LoginService(OAuthClient oauth, TokenManager tokens, ICallbackInbox inbox, TimeProvider time, ILogger<LoginService> logger)
{
    /// <summary>How long a started login stays valid.</summary>
    public static readonly TimeSpan PendingLifetime = TimeSpan.FromMinutes(15);

    private readonly ConcurrentDictionary<string, PendingLogin> _pending = new();

    /// <summary>The most recently started login that hasn't completed or expired.</summary>
    public PendingLogin? Current =>
        _pending.Values.Where(p => !IsExpired(p)).OrderByDescending(p => p.StartedAt).FirstOrDefault();

    public PendingLogin Begin()
    {
        foreach (var expired in _pending.Values.Where(IsExpired).ToList()) _pending.TryRemove(expired.State, out _);
        var state = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant();
        var pending = new PendingLogin(state, oauth.BuildAuthorizeUrl(state), time.GetUtcNow());
        _pending[state] = pending;
        return pending;
    }

    /// <summary>Completes a login from the redirect URL (or pasted text containing it).</summary>
    public async Task CompleteAsync(string redirectUrl, CancellationToken ct = default)
    {
        var redirect = AuthorizationRedirect.Parse(redirectUrl);
        if (!_pending.TryRemove(redirect.State, out var pending) || IsExpired(pending))
            throw new LoginException("This login link doesn't belong to a login started here, or it has expired. Start a new login.");

        TokenSet tokenSet;
        try
        {
            tokenSet = await oauth.ExchangeCodeAsync(redirect.Code, ct).ConfigureAwait(false);
        }
        catch (OAuthException e)
        {
            throw new LoginException($"itslearning rejected the login code ({e.Message}). Codes are single-use and short-lived; start a new login.");
        }
        await tokens.SignInAsync(tokenSet, ct).ConfigureAwait(false);
        logger.LogInformation("Logged in to itslearning.");
    }

    /// <summary>
    /// Completes pending logins whose redirect was delivered to the <see cref="ICallbackInbox"/> by another process.
    /// Returns true if a login completed.
    /// </summary>
    public async Task<bool> TryCompleteFromInboxAsync(CancellationToken ct = default)
    {
        foreach (var pending in _pending.Values.ToList())
        {
            if (IsExpired(pending)) { _pending.TryRemove(pending.State, out _); continue; }
            if (await inbox.TryTakeAsync(pending.State, ct).ConfigureAwait(false) is { } url)
            {
                await CompleteAsync(url, ct).ConfigureAwait(false);
                return true;
            }
        }
        return false;
    }

    /// <summary>Waits (polling the inbox) until the pending login completes, it expires, or <paramref name="ct"/> fires.</summary>
    public async Task<bool> WaitForInboxAsync(PendingLogin pending, CancellationToken ct = default)
    {
        while (!ct.IsCancellationRequested && _pending.ContainsKey(pending.State) && !IsExpired(pending))
        {
            if (await TryCompleteFromInboxAsync(ct).ConfigureAwait(false)) return true;
            await Task.Delay(500, ct).ConfigureAwait(false);
        }
        return false;
    }

    private bool IsExpired(PendingLogin p) => time.GetUtcNow() - p.StartedAt > PendingLifetime;
}
