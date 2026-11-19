using Educ8.Itslearning.Auth;
using Microsoft.Extensions.Options;

namespace Educ8.Itslearning.Tests;

public sealed class FileStorageTests : IDisposable
{
    private readonly string _dir = Directory.CreateTempSubdirectory("educ8-files-").FullName;
    private IOptions<ItslearningOptions> Options => Microsoft.Extensions.Options.Options.Create(new ItslearningOptions { DataDirectory = _dir });

    public void Dispose() => Directory.Delete(_dir, recursive: true);

    [Fact]
    public async Task Token_store_round_trips_and_is_private()
    {
        var store = new FileTokenStore(Options);
        Assert.Null(await store.LoadAsync());

        var tokens = new TokenSet("a", "r", new DateTimeOffset(2026, 11, 18, 12, 0, 0, TimeSpan.Zero));
        await store.SaveAsync(tokens);

        Assert.Equal(tokens, await store.LoadAsync());
        if (!OperatingSystem.IsWindows())
            Assert.Equal(UnixFileMode.UserRead | UnixFileMode.UserWrite, File.GetUnixFileMode(Path.Combine(_dir, Educ8Paths.TokenFileName)));

        await store.ClearAsync();
        Assert.Null(await store.LoadAsync());
    }

    [Fact]
    public async Task Corrupt_token_file_means_not_logged_in()
    {
        await File.WriteAllTextAsync(Path.Combine(_dir, Educ8Paths.TokenFileName), "{not json");
        Assert.Null(await new FileTokenStore(Options).LoadAsync());
    }

    [Fact]
    public async Task Lock_is_exclusive()
    {
        var a = new FileTokenStore(Options);
        var b = new FileTokenStore(Options);
        var first = await a.LockAsync();
        var second = b.LockAsync();
        await Task.Delay(300);
        Assert.False(second.IsCompleted);
        await first.DisposeAsync();
        await using var _ = await second.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Callback_inbox_hands_over_once()
    {
        var inbox = new FileCallbackInbox(Options);
        var url = "itsl-itslearning://login/?state=0123456789abcdef&code=c";
        await inbox.DeliverAsync(AuthorizationRedirect.Parse(url), url);

        Assert.Equal(url, await inbox.TryTakeAsync("0123456789abcdef"));
        Assert.Null(await inbox.TryTakeAsync("0123456789abcdef"));
    }

    [Fact]
    public void Settings_loader_layers_config_file_and_environment()
    {
        File.WriteAllText(Path.Combine(_dir, Educ8Paths.ConfigFileName), """{ "ClientId": "from-file", "Scope": "Courses" }""");
        var env = new Dictionary<string, string> { [ItslearningSettingsLoader.ScopeEnvVar] = "All" };

        var options = ItslearningSettingsLoader.Load(_dir, k => env.GetValueOrDefault(k));

        Assert.Equal("from-file", options.ClientId);
        Assert.Equal("All", options.Scope);
        Assert.Equal(ItslearningOptions.DefaultRedirectUri, options.RedirectUri);
        Assert.Equal(new Uri(ItslearningOptions.DefaultBaseUrl), options.BaseUrl);
    }

    [Fact]
    public void Missing_client_id_fails_validation() =>
        Assert.Throws<InvalidOperationException>(() => new ItslearningOptions { DataDirectory = _dir }.Validate());
}
