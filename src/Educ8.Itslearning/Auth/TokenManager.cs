using Microsoft.Extensions.Logging;

namespace Educ8.Itslearning.Auth;

/// <summary>Supplies a valid access token, refreshing it when needed.</summary>
public interface IAccessTokenProvider
{
    /// <param name="forceRefresh">Refresh even if the cached token looks valid (e.g. after a 401).</param>
    Task<string> GetAccessTokenAsync(bool forceRefresh = false, CancellationToken ct = default);
}

/// <summary>
/// Owns the itslearning session: caches the access token (5-minute lifetime), refreshes it under a cross-process
/// lock (refresh tokens rotate, so two processes must never refresh with the same one), and signs in/out.
/// One instance per process; the desktop app and MCP server each have one, sharing the <see cref="ITokenStore"/>.
/// </summary>
public sealed class TokenManager(OAuthClient oauth, ITokenStore store, TimeProvider time, ILogger<TokenManager> logger) : IAccessTokenProvider
{
    /// <summary>Refresh this long before expiry.</summary>
    internal static readonly TimeSpan ExpiryMargin = TimeSpan.FromSeconds(30);

    private readonly SemaphoreSlim _gate = new(1, 1);
    private TokenSet? _cached;

    /// <summary>Raised after sign-in, sign-out, or when the session expires. Lets a UI update itself.</summary>
    public event EventHandler<bool>? SessionChanged;

    /// <summary>True if a token set is stored (it may still turn out to be expired on refresh).</summary>
    public async Task<bool> HasSessionAsync(CancellationToken ct = default) =>
        _cached is not null || await store.LoadAsync(ct).ConfigureAwait(false) is not null;

    public async Task<string> GetAccessTokenAsync(bool forceRefresh = false, CancellationToken ct = default)
    {
        var cached = _cached;
        if (!forceRefresh && cached is not null && cached.IsValidAt(time.GetUtcNow(), ExpiryMargin))
            return cached.AccessToken;

        await _gate.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            if (!forceRefresh && _cached is { } c && c.IsValidAt(time.GetUtcNow(), ExpiryMargin))
                return c.AccessToken;

            await using var _ = await store.LockAsync(ct).ConfigureAwait(false);
            var stored = await store.LoadAsync(ct).ConfigureAwait(false);
            if (stored is null)
            {
                _cached = null;
                throw new NotAuthenticatedException("Not logged in to itslearning.");
            }

            // Another process may have refreshed already: use its token instead of refreshing again.
            var someoneElseRefreshed = _cached is not null && stored.AccessToken != _cached.AccessToken;
            if ((!forceRefresh || someoneElseRefreshed) && stored.IsValidAt(time.GetUtcNow(), ExpiryMargin))
            {
                _cached = stored;
                return stored.AccessToken;
            }

            TokenSet refreshed;
            try
            {
                refreshed = await oauth.RefreshAsync(stored.RefreshToken, ct).ConfigureAwait(false);
            }
            catch (OAuthException e) when (e.IsGrantRejected)
            {
                logger.LogWarning("itslearning refresh token was rejected ({Status}); session cleared.", (int)e.StatusCode);
                await store.ClearAsync(ct).ConfigureAwait(false);
                _cached = null;
                SessionChanged?.Invoke(this, false);
                throw new NotAuthenticatedException("The itslearning session has expired. Log in again.");
            }

            await store.SaveAsync(refreshed, ct).ConfigureAwait(false);
            _cached = refreshed;
            logger.LogDebug("itslearning access token refreshed; expires {ExpiresAt:u}.", refreshed.ExpiresAt);
            return refreshed.AccessToken;
        }
        finally
        {
            _gate.Release();
        }
    }

    /// <summary>Stores a freshly obtained token set (after login).</summary>
    public async Task SignInAsync(TokenSet tokens, CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            await using var _ = await store.LockAsync(ct).ConfigureAwait(false);
            await store.SaveAsync(tokens, ct).ConfigureAwait(false);
            _cached = tokens;
        }
        finally
        {
            _gate.Release();
        }
        SessionChanged?.Invoke(this, true);
    }

    /// <summary>Forgets the session locally (all processes sharing the store are signed out).</summary>
    public async Task SignOutAsync(CancellationToken ct = default)
    {
        await _gate.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            await using var _ = await store.LockAsync(ct).ConfigureAwait(false);
            await store.ClearAsync(ct).ConfigureAwait(false);
            _cached = null;
        }
        finally
        {
            _gate.Release();
        }
        SessionChanged?.Invoke(this, false);
    }
}
