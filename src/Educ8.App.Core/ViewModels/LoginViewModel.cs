using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Itslearning;
using Educ8.Itslearning.Auth;
using Microsoft.Extensions.Options;

namespace Educ8.App.Core.ViewModels;

/// <summary>
/// Sign-in in the user's default browser, so an existing browser session (school SSO) is reused.
/// <see cref="StartCommand"/> opens itslearning's sign-in page; at the end itslearning sends the browser to
/// <c>itsl-itslearning://login/?…</c>, which the OS hands to the app (it is registered as that scheme's handler).
/// The platform passes that URL to <see cref="AcceptRedirectAsync"/>. If another handler receives it instead
/// (e.g. <c>educ8-mcp handle-url</c>), it arrives through the shared <see cref="ICallbackInbox"/>, which is watched while
/// <see cref="IsActive"/>.
/// </summary>
public sealed partial class LoginViewModel(LoginService login, IOpenUrl openUrl, IOptions<ItslearningOptions> options) : ObservableObject
{
    private readonly string _redirectScheme = new Uri(options.Value.RedirectUri).Scheme;
    private CancellationTokenSource? _wait;

    /// <summary>True while a sign-in is waiting for the browser (show "Continue in your browser").</summary>
    [ObservableProperty]
    public partial bool IsActive { get; private set; }

    /// <summary>The sign-in page opened in the browser.</summary>
    [ObservableProperty]
    public partial Uri? AuthorizeUrl { get; private set; }

    /// <summary>True while the code is being exchanged for tokens (show a spinner).</summary>
    [ObservableProperty]
    public partial bool IsBusy { get; private set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Raised after a successful sign-in (the shell also follows the session itself).</summary>
    public event EventHandler? Completed;

    [RelayCommand]
    private void Start()
    {
        StopWaiting();
        ErrorMessage = null;
        var pending = login.Begin();
        AuthorizeUrl = pending.AuthorizeUrl;
        IsActive = true;
        openUrl.Open(pending.AuthorizeUrl);
        var wait = _wait = new CancellationTokenSource();
        _ = WaitForOtherHandlerAsync(pending, wait.Token);
    }

    /// <summary>For when the browser tab got closed or never opened.</summary>
    [RelayCommand]
    private void OpenBrowserAgain()
    {
        if (AuthorizeUrl is { } url) openUrl.Open(url);
    }

    [RelayCommand]
    private void Cancel()
    {
        StopWaiting();
        IsActive = false;
        IsBusy = false;
    }

    /// <summary>True if <paramref name="url"/> is an itslearning login redirect (<c>itsl-itslearning://…</c>).</summary>
    public bool IsRedirect(string url) =>
        Uri.TryCreate(url, UriKind.Absolute, out var u) && u.Scheme.Equals(_redirectScheme, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Call with a URL the OS handed to the app. Completes this app's sign-in, or passes the redirect on to the process that
    /// started that login (<c>educ8-mcp</c>). Returns true if it was this app's sign-in (the platform should show its window);
    /// false for other logins and for URLs that aren't login redirects.
    /// </summary>
    public async Task<bool> AcceptRedirectAsync(string url)
    {
        if (!IsRedirect(url)) return false;
        var own = false;
        IsBusy = IsActive;
        try
        {
            own = await login.AcceptRedirectAsync(url);
            if (own) Succeeded();
        }
        catch (Exception e) when (e is LoginException or HttpRequestException or TaskCanceledException or IOException)
        {
            // A bad link while nobody is signing in here was meant for another process: nothing to show.
            if (IsActive) Fail(e);
        }
        finally
        {
            IsBusy = false;
        }
        return own;
    }

    /// <summary>Picks up the redirect if another URL handler put it in the inbox; reports a sign-in that expired.</summary>
    private async Task WaitForOtherHandlerAsync(PendingLogin pending, CancellationToken ct)
    {
        try
        {
            if (await login.WaitForInboxAsync(pending, ct)) Succeeded();
            else if (!ct.IsCancellationRequested && login.IsExpired(pending) && IsActive)
            {
                StopWaiting();
                ErrorMessage = Strings.LoginExpired;
                IsActive = false;
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception e) when (e is LoginException or HttpRequestException or TaskCanceledException or IOException)
        {
            Fail(e);
        }
    }

    private void Succeeded()
    {
        StopWaiting();
        IsActive = false;
        Completed?.Invoke(this, EventArgs.Empty);
    }

    private void Fail(Exception e)
    {
        StopWaiting();
        ErrorMessage = e is LoginException
            ? string.Format(CultureInfo.CurrentCulture, Strings.LoginFailed, e.Message)
            : Presentation.Texts.Error(e);
        IsActive = false;
    }

    private void StopWaiting()
    {
        _wait?.Cancel();
        _wait?.Dispose();
        _wait = null;
    }
}
