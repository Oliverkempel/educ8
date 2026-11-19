using System.Net;
using System.Text;
using Educ8.Itslearning;
using Educ8.Itslearning.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Educ8.Itslearning.Tests;

/// <summary>Fake itslearning: routes requests to a handler function and records them.</summary>
public sealed class FakeItslearning
{
    public List<(HttpMethod Method, string PathAndQuery, string? Authorization, string? Body)> Requests { get; } = [];
    public Func<HttpRequestMessage, string?, HttpResponseMessage> Respond { get; set; } = (_, _) => new HttpResponseMessage(HttpStatusCode.NotFound);

    public HttpMessageHandler CreateHandler() => new Handler(this);

    public static HttpResponseMessage Json(string json, HttpStatusCode status = HttpStatusCode.OK) =>
        new(status) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

    public static string TokenJson(string access, string refresh, int expiresIn = 300) =>
        $$"""{"access_token":"{{access}}","token_type":"Bearer","expires_in":{{expiresIn}},"refresh_token":"{{refresh}}"}""";

    private sealed class Handler(FakeItslearning fake) : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var body = request.Content is null ? null : await request.Content.ReadAsStringAsync(ct);
            lock (fake.Requests)
                fake.Requests.Add((request.Method, request.RequestUri!.PathAndQuery, request.Headers.Authorization?.ToString(), body));
            var response = fake.Respond(request, body);
            response.RequestMessage ??= request; // a handler may set a different final URL to simulate redirects
            return response;
        }
    }
}

public sealed class FakeWebHandlerFactory(FakeItslearning web) : Educ8.Itslearning.Content.IWebHandlerFactory
{
    public HttpMessageHandler Create(System.Net.CookieContainer cookies) => web.CreateHandler();
}

public static class Fixtures
{
    public static string Read(string name) => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", name));

    /// <summary>A response that looks like it ended at <paramref name="finalUrl"/> after redirects.</summary>
    public static HttpResponseMessage Html(string html, string finalUrl)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(html, Encoding.UTF8, "text/html") };
        response.RequestMessage = new HttpRequestMessage(HttpMethod.Get, finalUrl);
        return response;
    }
}

/// <summary>A clock tests can move.</summary>
public sealed class ManualTimeProvider(DateTimeOffset start) : TimeProvider
{
    public DateTimeOffset Now { get; set; } = start;
    public override DateTimeOffset GetUtcNow() => Now;
    public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    public void Advance(TimeSpan by) => Now += by;
}

public sealed class InMemoryTokenStore : ITokenStore
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    public TokenSet? Tokens { get; set; }
    public int Saves { get; private set; }

    public Task<TokenSet?> LoadAsync(CancellationToken ct = default) => Task.FromResult(Tokens);
    public Task SaveAsync(TokenSet tokens, CancellationToken ct = default) { Tokens = tokens; Saves++; return Task.CompletedTask; }
    public Task ClearAsync(CancellationToken ct = default) { Tokens = null; return Task.CompletedTask; }

    public async Task<IAsyncDisposable> LockAsync(CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        return new Releaser(_lock);
    }

    private sealed class Releaser(SemaphoreSlim s) : IAsyncDisposable
    {
        public ValueTask DisposeAsync() { s.Release(); return ValueTask.CompletedTask; }
    }
}

/// <summary>Builds the real core services, with fake HTTP, clock, token store and a temp data dir.</summary>
public sealed class TestServices : IDisposable
{
    public static readonly DateTimeOffset T0 = new(2026, 11, 18, 12, 0, 0, TimeSpan.Zero);

    public FakeItslearning Http { get; } = new();

    /// <summary>The itslearning web UI (cookie session, tool pages), separate from the REST API.</summary>
    public FakeItslearning Web { get; } = new();

    public ManualTimeProvider Time { get; } = new(T0);
    public InMemoryTokenStore Store { get; } = new();
    public string DataDir { get; } = Directory.CreateTempSubdirectory("educ8-test-").FullName;
    public IServiceCollection Services { get; } = new ServiceCollection();

    public TestServices()
    {
        Services.AddLogging(b => b.SetMinimumLevel(LogLevel.Warning));
        Services.AddSingleton<TimeProvider>(Time);
        Services.AddSingleton<ITokenStore>(Store);
        Services.AddSingleton<Educ8.Itslearning.Content.IWebHandlerFactory>(new FakeWebHandlerFactory(Web));
        Services.AddItslearning(new ItslearningOptions { BaseUrl = new Uri("https://school.itslearning.com"), ClientId = "test-client", DataDirectory = DataDir });
        Services.ConfigureHttpClientDefaults(b => b.ConfigurePrimaryHttpMessageHandler(() => Http.CreateHandler()));
    }

    public ServiceProvider Build() => Services.BuildServiceProvider();

    /// <summary>Stores a token set valid for 5 minutes from now.</summary>
    public void SignedIn(string access = "access-1", string refresh = "refresh-1") =>
        Store.Tokens = new TokenSet(access, refresh, Time.Now.AddSeconds(300));

    public void Dispose()
    {
        try { Directory.Delete(DataDir, recursive: true); } catch (IOException) { }
    }
}
