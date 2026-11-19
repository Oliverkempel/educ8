using System.Net;
using Educ8.Itslearning.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class TokenManagerTests
{
    [Fact]
    public async Task Uses_stored_token_while_valid()
    {
        using var t = new TestServices();
        t.SignedIn("access-1");
        using var sp = t.Build();

        Assert.Equal("access-1", await sp.GetRequiredService<TokenManager>().GetAccessTokenAsync());
        Assert.Empty(t.Http.Requests);
    }

    [Fact]
    public async Task Refreshes_near_expiry_and_saves_rotated_refresh_token()
    {
        using var t = new TestServices();
        t.SignedIn("access-1", "refresh-1");
        t.Http.Respond = (_, body) =>
        {
            Assert.Contains("refresh_token=refresh-1", body);
            return FakeItslearning.Json(FakeItslearning.TokenJson("access-2", "refresh-2"));
        };
        using var sp = t.Build();
        var manager = sp.GetRequiredService<TokenManager>();

        t.Time.Advance(TimeSpan.FromSeconds(290)); // within the 30 s margin
        Assert.Equal("access-2", await manager.GetAccessTokenAsync());
        Assert.Equal("refresh-2", t.Store.Tokens!.RefreshToken);
    }

    [Fact]
    public async Task Uses_token_refreshed_by_another_process_instead_of_refreshing_again()
    {
        using var t = new TestServices();
        t.SignedIn("access-1", "refresh-1");
        using var sp = t.Build();
        var manager = sp.GetRequiredService<TokenManager>();
        Assert.Equal("access-1", await manager.GetAccessTokenAsync());

        // Another process refreshes and stores a new, valid set.
        t.Store.Tokens = new TokenSet("access-other", "refresh-other", t.Time.Now.AddSeconds(300));

        // Our 401 path forces a refresh, but the store already has a newer token: use it, don't spend the refresh token.
        Assert.Equal("access-other", await manager.GetAccessTokenAsync(forceRefresh: true));
        Assert.Empty(t.Http.Requests);
    }

    [Fact]
    public async Task Rejected_refresh_clears_session()
    {
        using var t = new TestServices();
        t.SignedIn();
        t.Http.Respond = (_, _) => FakeItslearning.Json("""{"error":"invalid_grant"}""", HttpStatusCode.BadRequest);
        using var sp = t.Build();
        var manager = sp.GetRequiredService<TokenManager>();
        bool? sessionEvent = null;
        manager.SessionChanged += (_, signedIn) => sessionEvent = signedIn;

        t.Time.Advance(TimeSpan.FromMinutes(10));
        await Assert.ThrowsAsync<NotAuthenticatedException>(() => manager.GetAccessTokenAsync());
        Assert.Null(t.Store.Tokens);
        Assert.False(sessionEvent);
    }

    [Fact]
    public async Task Not_logged_in_throws()
    {
        using var t = new TestServices();
        using var sp = t.Build();
        await Assert.ThrowsAsync<NotAuthenticatedException>(() => sp.GetRequiredService<TokenManager>().GetAccessTokenAsync());
    }

    [Fact]
    public async Task Sign_out_clears_store()
    {
        using var t = new TestServices();
        t.SignedIn();
        using var sp = t.Build();
        var manager = sp.GetRequiredService<TokenManager>();

        await manager.SignOutAsync();
        Assert.Null(t.Store.Tokens);
        Assert.False(await manager.HasSessionAsync());
    }
}
