using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Resources;
using Educ8.Itslearning;
using Educ8.Itslearning.Auth;
using Microsoft.Extensions.Options;

namespace Educ8.App.Core.ViewModels;

/// <summary>
/// Sign-in in an embedded web view. The platform UI shows <see cref="AuthorizeUrl"/> when <see cref="IsActive"/>, and passes every
/// navigation to <see cref="OnNavigating"/>. When itslearning redirects to <c>itsl-itslearning://login/?…</c>, this view model
/// cancels that navigation and completes the login. No URL handler or pasting is needed.
/// </summary>
public sealed partial class LoginViewModel(LoginService login, IOptions<ItslearningOptions> options) : ObservableObject
{
    private readonly string _redirectScheme = new Uri(options.Value.RedirectUri).Scheme;

    /// <summary>True while the sign-in web view should be shown.</summary>
    [ObservableProperty]
    public partial bool IsActive { get; private set; }

    /// <summary>The page to load in the web view.</summary>
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
        ErrorMessage = null;
        AuthorizeUrl = login.Begin().AuthorizeUrl;
        IsActive = true;
    }

    [RelayCommand]
    private void Cancel()
    {
        IsActive = false;
        IsBusy = false;
    }

    /// <summary>
    /// Call for every navigation in the sign-in web view. Returns true if the platform UI must <b>cancel</b> the navigation
    /// (it's the itslearning redirect, handled here).
    /// </summary>
    public bool OnNavigating(Uri url)
    {
        if (!url.Scheme.Equals(_redirectScheme, StringComparison.OrdinalIgnoreCase)) return false;
        _ = CompleteAsync(url.OriginalString);
        return true;
    }

    internal async Task CompleteAsync(string redirectUrl)
    {
        IsBusy = true;
        try
        {
            await login.CompleteAsync(redirectUrl);
            IsActive = false;
            Completed?.Invoke(this, EventArgs.Empty);
        }
        catch (LoginException e)
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.LoginFailed, e.Message);
            IsActive = false;
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException)
        {
            ErrorMessage = Presentation.Texts.Error(e);
            IsActive = false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
