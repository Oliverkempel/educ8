using Microsoft.Extensions.Options;

namespace Educ8.Itslearning.Auth;

/// <summary>
/// Hand-off point for login redirect URLs captured outside the process that started the login
/// (e.g. by an OS URL-scheme handler). A process that receives the redirect for its own login calls
/// <see cref="LoginService.CompleteAsync"/> directly.
/// </summary>
public interface ICallbackInbox
{
    /// <summary>Delivers a redirect URL for whoever is waiting on its state.</summary>
    Task DeliverAsync(AuthorizationRedirect redirect, string rawUrl, CancellationToken ct = default);

    /// <summary>Takes (and removes) a delivered redirect URL for <paramref name="state"/>, if any.</summary>
    Task<string?> TryTakeAsync(string state, CancellationToken ct = default);
}

/// <summary>File-based inbox: <c>&lt;data dir&gt;/callbacks/&lt;state&gt;.url</c>. Works across processes without sockets.</summary>
public sealed class FileCallbackInbox(IOptions<ItslearningOptions> options) : ICallbackInbox
{
    private string Directory => Educ8Paths.EnsureDirectory(Path.Combine(options.Value.DataDirectory, Educ8Paths.CallbackDirectoryName));

    public async Task DeliverAsync(AuthorizationRedirect redirect, string rawUrl, CancellationToken ct = default)
    {
        var path = PathFor(redirect.State);
        await File.WriteAllTextAsync(path, rawUrl, ct).ConfigureAwait(false);
        Educ8Paths.RestrictToUser(path);
    }

    public async Task<string?> TryTakeAsync(string state, CancellationToken ct = default)
    {
        var path = PathFor(state);
        if (!File.Exists(path)) return null;
        var url = await File.ReadAllTextAsync(path, ct).ConfigureAwait(false);
        File.Delete(path);
        return url;
    }

    private string PathFor(string state)
    {
        if (!AuthorizationRedirect.StateFormat().IsMatch(state)) throw new LoginException("Invalid login state.");
        return Path.Combine(Directory, state + ".url");
    }
}
