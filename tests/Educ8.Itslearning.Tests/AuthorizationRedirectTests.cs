using Educ8.Itslearning.Auth;

namespace Educ8.Itslearning.Tests;

public class AuthorizationRedirectTests
{
    private const string State = "c35ded053b8d9ae72732c577219cdc34";

    [Theory]
    [InlineData("itsl-itslearning://login/?state=" + State + "&code=900937-abc123")]
    [InlineData("  itsl-itslearning://login/?state=" + State + "&code=900937-abc123\n")]
    [InlineData("top.location.replace('itsl-itslearning://login/?state=" + State + "&code=900937-abc123');")]
    [InlineData("state=" + State + "&code=900937-abc123")]
    public void Parses_redirects_and_pasted_text(string text)
    {
        var redirect = AuthorizationRedirect.Parse(text);
        Assert.Equal(State, redirect.State);
        Assert.Equal("900937-abc123", redirect.Code);
    }

    [Fact]
    public void Reports_oauth_error() =>
        Assert.Contains("access_denied", Assert.Throws<LoginException>(() => AuthorizationRedirect.Parse("itsl-itslearning://login/?error=access_denied")).Message);

    [Theory]
    [InlineData("https://example.com/")]
    [InlineData("itsl-itslearning://login/?state=" + State)]
    [InlineData("itsl-itslearning://login/?state=../../etc&code=x")]
    [InlineData("itsl-itslearning://login/?state=ABCDEF0123456789ABCD&code=x")]
    public void Rejects_bad_input(string text) =>
        Assert.Throws<LoginException>(() => AuthorizationRedirect.Parse(text));
}
