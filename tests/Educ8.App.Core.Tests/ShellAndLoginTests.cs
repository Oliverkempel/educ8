using System.Net;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Tests;

namespace Educ8.App.Core.Tests;

public class ShellAndLoginTests
{
    private static void Routes(AppTestHost h) => h.T.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath switch
    {
        "/restapi/oauth2/token" => FakeItslearning.Json(FakeItslearning.TokenJson("A", "R")),
        "/restapi/personal/person/v1" => FakeItslearning.Json("""{"PersonId":5,"FirstName":"Test","LastName":"Student"}"""),
        "/restapi/personal/courses/v4" => FakeItslearning.Json(AppTestHost.CoursesJson(AppTestHost.Course(1, "Web Development, (E26)", "T1-1-E26", "2026-11-18T08:00:00Z"))),
        _ => new HttpResponseMessage(HttpStatusCode.NotFound),
    };

    [Fact]
    public async Task Without_a_session_the_welcome_page_shows()
    {
        using var h = new AppTestHost();
        await h.Shell.InitializeAsync();
        Assert.Equal(ShellState.SignedOut, h.Shell.State);
        Assert.Null(h.Shell.ErrorMessage);
    }

    [Fact]
    public async Task With_a_session_the_user_and_first_page_load()
    {
        using var h = new AppTestHost();
        h.T.SignedIn();
        Routes(h);

        await h.Shell.InitializeAsync();

        Assert.Equal(ShellState.SignedIn, h.Shell.State);
        Assert.Equal("Signed in as Test Student", h.Shell.AccountText);
        Assert.Equal(["Courses", "Calendar", "MCP server"], h.Shell.NavigationItems.Select(n => n.Title));
        Assert.Equal("Web Development, (E26)", Assert.Single(h.Shell.Courses.Courses).Title);
    }

    [Fact]
    public async Task Signing_out_clears_session_and_pages()
    {
        using var h = new AppTestHost();
        h.T.SignedIn();
        Routes(h);
        await h.Shell.InitializeAsync();

        await h.Shell.SignOutCommand.ExecuteAsync(null);

        Assert.Equal(ShellState.SignedOut, h.Shell.State);
        Assert.Null(h.T.Store.Tokens);
        Assert.Empty(h.Shell.Courses.Courses);
    }

    [Fact]
    public async Task Offline_at_start_shows_a_friendly_error()
    {
        using var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (_, _) => throw new HttpRequestException("network down");

        await h.Shell.InitializeAsync();

        Assert.Equal(ShellState.SignedOut, h.Shell.State);
        Assert.StartsWith("Couldn't reach itslearning", h.Shell.ErrorMessage);
    }

    [Fact]
    public async Task Login_intercepts_the_redirect_and_signs_in()
    {
        using var h = new AppTestHost();
        Routes(h);
        var shell = h.Shell;
        await shell.InitializeAsync();
        var login = shell.Login;
        var completed = false;
        login.Completed += (_, _) => completed = true;

        login.StartCommand.Execute(null);
        Assert.True(login.IsActive);
        var state = System.Web.HttpUtility.ParseQueryString(login.AuthorizeUrl!.Query)["state"];

        Assert.False(login.OnNavigating(new Uri("https://login.microsoftonline.com/x"))); // ordinary pages load normally
        await login.CompleteAsync($"itsl-itslearning://login/?state={state}&code=900937-abc");

        Assert.True(completed);
        Assert.False(login.IsActive);
        Assert.Equal("A", h.T.Store.Tokens!.AccessToken);
        Assert.Equal(ShellState.SignedIn, shell.State); // via TokenManager.SessionChanged
    }

    [Fact]
    public async Task Redirect_navigation_is_cancelled_and_bad_state_reported()
    {
        using var h = new AppTestHost();
        Routes(h);
        var login = h.Login;
        login.StartCommand.Execute(null);

        Assert.True(login.OnNavigating(new Uri("itsl-itslearning://login/?state=00000000000000000000000000000000&code=x")));
        await Task.Delay(50); // completion runs in the background
        Assert.StartsWith("Signing in didn't work", login.ErrorMessage);
        Assert.False(login.IsActive);
    }
}
