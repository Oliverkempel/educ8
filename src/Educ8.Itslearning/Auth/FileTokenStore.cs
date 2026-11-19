using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Educ8.Itslearning.Auth;

/// <summary>
/// Stores tokens as JSON in the per-user data directory (file mode 0600 on Unix), with a lock file for
/// cross-process refresh. Plaintext on disk; an OS secret-store implementation can replace it later.
/// </summary>
public sealed class FileTokenStore : ITokenStore
{
    private static readonly TimeSpan LockTimeout = TimeSpan.FromSeconds(15);
    private readonly string _tokenPath;
    private readonly string _lockPath;

    public FileTokenStore(IOptions<ItslearningOptions> options)
    {
        var dir = Educ8Paths.EnsureDirectory(options.Value.DataDirectory);
        _tokenPath = Path.Combine(dir, Educ8Paths.TokenFileName);
        _lockPath = Path.Combine(dir, Educ8Paths.TokenLockFileName);
    }

    public async Task<TokenSet?> LoadAsync(CancellationToken ct = default)
    {
        if (!File.Exists(_tokenPath)) return null;
        try
        {
            await using var stream = File.OpenRead(_tokenPath);
            return await JsonSerializer.DeserializeAsync<TokenSet>(stream, cancellationToken: ct).ConfigureAwait(false);
        }
        catch (JsonException)
        {
            return null; // corrupt file = not logged in
        }
    }

    public async Task SaveAsync(TokenSet tokens, CancellationToken ct = default)
    {
        // Write to a temp file and move it in place so readers never see a half-written file.
        var temp = _tokenPath + ".tmp";
        await using (var stream = new FileStream(temp, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            Educ8Paths.RestrictToUser(temp);
            await JsonSerializer.SerializeAsync(stream, tokens, cancellationToken: ct).ConfigureAwait(false);
        }
        File.Move(temp, _tokenPath, overwrite: true);
    }

    public Task ClearAsync(CancellationToken ct = default)
    {
        File.Delete(_tokenPath);
        return Task.CompletedTask;
    }

    public async Task<IAsyncDisposable> LockAsync(CancellationToken ct = default)
    {
        var deadline = DateTime.UtcNow + LockTimeout;
        while (true)
        {
            try
            {
                // FileShare.None is enforced across processes (flock on Unix). The lock file is kept, not deleted,
                // so every process always locks the same file.
                return new FileStream(_lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1);
            }
            catch (IOException) when (DateTime.UtcNow < deadline)
            {
                await Task.Delay(100, ct).ConfigureAwait(false);
            }
        }
    }
}
