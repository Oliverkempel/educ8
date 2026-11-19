namespace Educ8.Itslearning.Auth;

/// <summary>
/// Persists the token set. Several processes (MCP server, desktop app) may share one store, so callers that
/// read-modify-write (refresh) must hold <see cref="LockAsync"/>.
/// </summary>
public interface ITokenStore
{
    Task<TokenSet?> LoadAsync(CancellationToken ct = default);
    Task SaveAsync(TokenSet tokens, CancellationToken ct = default);
    Task ClearAsync(CancellationToken ct = default);

    /// <summary>Acquires an exclusive lock shared across processes. Dispose to release.</summary>
    Task<IAsyncDisposable> LockAsync(CancellationToken ct = default);
}
