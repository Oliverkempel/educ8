using System.Net;
using Educ8.Itslearning.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class OAuthClientTests
{
    [Fact]
    public void Authorize_url_contains_encoded_parameters()
    {
        using var t = new TestServices();
        using var sp = t.Build();
        var url = sp.GetRequiredService<OAuthClient>().BuildAuthorizeUrl("0123456789abcdef");

        Assert.StartsWith("https://sdu.itslearning.com/oauth2/authorize.aspx?", url.ToString());
        Assert.Contains("client_id=test-client", url.Query);
        Assert.Contains("state=0123456789abcdef", url.Query);
        Assert.Contains("response_type=code", url.Query);
        Assert.Contains("scope=All", url.Query);
        Assert.Contains("redirect_uri=itsl-itslearning%3A%2F%2Flogin", url.Query);
    }

    [Fact]
    public async Task Exchange_posts_form_and_computes_expiry()
    {
        using var t = new TestServices();
        t.Http.Respond = (_, _) => FakeItslearning.Json(FakeItslearning.TokenJson("A", "R", 300));
        using var sp = t.Build();

        var tokens = await sp.GetRequiredService<OAuthClient>().ExchangeCodeAsync("the-code");

        Assert.Equal("A", tokens.AccessToken);
        Assert.Equal("R", tokens.RefreshToken);
        Assert.Equal(TestServices.T0.AddSeconds(300), tokens.ExpiresAt);
        var request = Assert.Single(t.Http.Requests);
        Assert.Equal("/restapi/oauth2/token", request.PathAndQuery);
        Assert.Contains("grant_type=authorization_code", request.Body);
        Assert.Contains("code=the-code", request.Body);
        Assert.Contains("client_id=test-client", request.Body);
        Assert.DoesNotContain("client_secret", request.Body);
    }

    [Fact]
    public async Task Rejected_grant_is_reported()
    {
        using var t = new TestServices();
        t.Http.Respond = (_, _) => FakeItslearning.Json("""{"error":"invalid_grant"}""", HttpStatusCode.BadRequest);
        using var sp = t.Build();

        var e = await Assert.ThrowsAsync<OAuthException>(() => sp.GetRequiredService<OAuthClient>().RefreshAsync("old"));
        Assert.True(e.IsGrantRejected);
        Assert.Equal("invalid_grant", e.Error);
    }
}
