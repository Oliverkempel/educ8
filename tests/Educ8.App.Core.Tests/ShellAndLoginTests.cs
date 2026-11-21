using System.Net;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;

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
        Assert.Equal(["Courses", "Calendar", "Notifications", "Messages", "MCP server"], h.Shell.NavigationItems.Select(n => n.Title));
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
    public async Task Login_opens_the_browser_and_signs_in_from_the_handed_over_redirect()
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
        Assert.Equal(login.AuthorizeUrl, Assert.Single(h.OpenUrl.Opened));
        var state = System.Web.HttpUtility.ParseQueryString(login.AuthorizeUrl!.Query)["state"];

        login.OpenBrowserAgainCommand.Execute(null);
        Assert.Equal(2, h.OpenUrl.Opened.Count);

        Assert.False(await login.AcceptRedirectAsync("https://school.itslearning.com/")); // not a login redirect
        Assert.True(await login.AcceptRedirectAsync($"itsl-itslearning://login/?state={state}&code=900937-abc"));

        Assert.True(completed);
        Assert.False(login.IsActive);
        Assert.Equal("A", h.T.Store.Tokens!.AccessToken);
        Assert.Equal(ShellState.SignedIn, shell.State); // via TokenManager.SessionChanged
    }

    [Fact]
    public async Task Login_completes_from_the_inbox_when_another_handler_got_the_redirect()
    {
        using var h = new AppTestHost();
        Routes(h);
        var login = h.Login;
        var completed = new TaskCompletionSource();
        login.Completed += (_, _) => completed.TrySetResult();

        login.StartCommand.Execute(null);
        var state = System.Web.HttpUtility.ParseQueryString(login.AuthorizeUrl!.Query)["state"]!;
        var url = $"itsl-itslearning://login/?state={state}&code=900937-abc";
        await h.Provider.GetRequiredService<ICallbackInbox>().DeliverAsync(AuthorizationRedirect.Parse(url), url); // as educ8-mcp handle-url does

        await completed.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Assert.False(login.IsActive);
        Assert.Equal("A", h.T.Store.Tokens!.AccessToken);
    }

    [Fact]
    public async Task Redirects_for_other_logins_are_passed_on_and_bad_ones_reported_only_while_signing_in()
    {
        using var h = new AppTestHost();
        Routes(h);
        var login = h.Login;
        const string foreign = "itsl-itslearning://login/?state=0123456789abcdef0123456789abcdef&code=x";

        Assert.False(await login.AcceptRedirectAsync(foreign)); // educ8-mcp's login: into the inbox for it
        Assert.Equal(foreign, await h.Provider.GetRequiredService<ICallbackInbox>().TryTakeAsync("0123456789abcdef0123456789abcdef"));
        Assert.False(await login.AcceptRedirectAsync("itsl-itslearning://login/?nothing"));
        Assert.Null(login.ErrorMessage);

        login.StartCommand.Execute(null);
        Assert.False(await login.AcceptRedirectAsync("itsl-itslearning://login/?error=access_denied"));
        Assert.StartsWith("Signing in didn't work", login.ErrorMessage);
        Assert.False(login.IsActive);
    }

    [Fact]
    public void Cancel_stops_waiting()
    {
        using var h = new AppTestHost();
        var login = h.Login;
        login.StartCommand.Execute(null);
        login.CancelCommand.Execute(null);
        Assert.False(login.IsActive);
        Assert.Null(login.ErrorMessage);
    }
}
