using Educ8.Itslearning.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class LoginServiceTests
{
    [Fact]
    public async Task Completes_login_for_matching_state()
    {
        using var t = new TestServices();
        t.Http.Respond = (_, _) => FakeItslearning.Json(FakeItslearning.TokenJson("A", "R"));
        using var sp = t.Build();
        var login = sp.GetRequiredService<LoginService>();

        var pending = login.Begin();
        Assert.Contains($"state={pending.State}", pending.AuthorizeUrl.Query);
        Assert.Same(pending, login.Current);

        await login.CompleteAsync($"itsl-itslearning://login/?state={pending.State}&code=900937-xyz");

        Assert.Equal("A", t.Store.Tokens!.AccessToken);
        Assert.Null(login.Current);
        Assert.Contains("code=900937-xyz", t.Http.Requests.Single().Body);
    }

    [Fact]
    public async Task Rejects_unknown_state_without_calling_itslearning()
    {
        using var t = new TestServices();
        using var sp = t.Build();
        var login = sp.GetRequiredService<LoginService>();
        login.Begin();

        await Assert.ThrowsAsync<LoginException>(() => login.CompleteAsync("itsl-itslearning://login/?state=00000000000000000000000000000000&code=x"));
        Assert.Empty(t.Http.Requests);
    }

    [Fact]
    public async Task Rejects_expired_login()
    {
        using var t = new TestServices();
        using var sp = t.Build();
        var login = sp.GetRequiredService<LoginService>();
        var pending = login.Begin();

        t.Time.Advance(LoginService.PendingLifetime + TimeSpan.FromSeconds(1));
        await Assert.ThrowsAsync<LoginException>(() => login.CompleteAsync($"state={pending.State}&code=x"));
    }

    [Fact]
    public async Task Accepted_redirect_completes_own_login_and_forwards_others()
    {
        using var t = new TestServices();
        t.Http.Respond = (_, _) => FakeItslearning.Json(FakeItslearning.TokenJson("A", "R"));
        using var sp = t.Build();
        var login = sp.GetRequiredService<LoginService>();
        var inbox = sp.GetRequiredService<ICallbackInbox>();

        // A login some other process started (say educ8-mcp): passed on through the inbox, no token request here.
        var foreign = "itsl-itslearning://login/?state=0123456789abcdef0123456789abcdef&code=900937-other";
        Assert.False(await login.AcceptRedirectAsync(foreign));
        Assert.Empty(t.Http.Requests);
        Assert.Equal(foreign, await inbox.TryTakeAsync("0123456789abcdef0123456789abcdef"));

        var pending = login.Begin();
        Assert.True(await login.AcceptRedirectAsync($"itsl-itslearning://login/?state={pending.State}&code=900937-own"));
        Assert.Equal("A", t.Store.Tokens!.AccessToken);
    }

    [Fact]
    public async Task Picks_up_redirect_delivered_by_another_process()
    {
        using var t = new TestServices();
        t.Http.Respond = (_, _) => FakeItslearning.Json(FakeItslearning.TokenJson("A", "R"));
        using var sp = t.Build();
        var login = sp.GetRequiredService<LoginService>();
        var pending = login.Begin();
        Assert.False(await login.TryCompleteFromInboxAsync());

        var url = $"itsl-itslearning://login/?state={pending.State}&code=c";
        await sp.GetRequiredService<ICallbackInbox>().DeliverAsync(AuthorizationRedirect.Parse(url), url);

        Assert.True(await login.TryCompleteFromInboxAsync());
        Assert.NotNull(t.Store.Tokens);
    }
}
