using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;

namespace Educ8.App.Core.ViewModels;

public enum ShellState
{
    /// <summary>Checking for a stored session.</summary>
    Starting,

    /// <summary>No session: show the welcome page with "Sign in".</summary>
    SignedOut,

    /// <summary>Signed in: show the sidebar and pages.</summary>
    SignedIn,
}

public enum AppPage
{
    Courses,
    Calendar,
}

/// <summary>A sidebar entry.</summary>
public sealed record NavItem(AppPage Page, string Title, AppIcon Icon);

/// <summary>
/// The window: session state, sidebar navigation and the account area. Owns the page view models.
/// Follows <see cref="TokenManager.SessionChanged"/>, so signing in or out anywhere (also from the MCP server's CLI) updates it.
/// </summary>
public sealed partial class ShellViewModel : ObservableObject
{
    private readonly TokenManager _tokens;
    private readonly Func<ItslearningClient> _client;
    private readonly IUiDispatcher _dispatcher;
    private readonly CourseCatalog _catalog;

    public ShellViewModel(
        TokenManager tokens, Func<ItslearningClient> client, IUiDispatcher dispatcher, CourseCatalog catalog,
        LoginViewModel login, CoursesViewModel courses, CalendarViewModel calendar)
    {
        _tokens = tokens;
        _client = client;
        _dispatcher = dispatcher;
        _catalog = catalog;
        Login = login;
        Courses = courses;
        Calendar = calendar;
        _tokens.SessionChanged += (_, _) => _dispatcher.Post(() => _ = InitializeAsync());
    }

    public LoginViewModel Login { get; }
    public CoursesViewModel Courses { get; }
    public CalendarViewModel Calendar { get; }

    public IReadOnlyList<NavItem> NavigationItems { get; } =
    [
        new(AppPage.Courses, Strings.NavCourses, AppIcon.Courses),
        new(AppPage.Calendar, Strings.NavCalendar, AppIcon.Calendar),
    ];

    [ObservableProperty]
    public partial ShellState State { get; private set; } = ShellState.Starting;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AccountText))]
    public partial string? UserName { get; private set; }

    public string AccountText => UserName is null ? "" : string.Format(CultureInfo.CurrentCulture, Strings.SignedInAs, UserName);

    [ObservableProperty]
    public partial AppPage CurrentPage { get; set; } = AppPage.Courses;

    /// <summary>Problem shown on the welcome page (e.g. offline at start).</summary>
    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Checks the stored session and shows the right state. Called at startup and when the session changes.</summary>
    [RelayCommand]
    public async Task InitializeAsync()
    {
        ErrorMessage = null;
        if (!await _tokens.HasSessionAsync())
        {
            SignedOut();
            return;
        }
        try
        {
            var me = await _client().GetCurrentPersonAsync();
            UserName = me.DisplayName;
            State = ShellState.SignedIn;
            await ActivateCurrentPageAsync();
        }
        catch (NotAuthenticatedException)
        {
            SignedOut();
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException or ItslearningApiException)
        {
            SignedOut();
            ErrorMessage = Texts.Error(e);
        }
    }

    [RelayCommand]
    private Task SignOutAsync() => _tokens.SignOutAsync(); // SessionChanged → InitializeAsync → SignedOut

    partial void OnCurrentPageChanged(AppPage value) => _ = ActivateCurrentPageAsync();

    /// <summary>Loads the shown page's data the first time it's shown.</summary>
    private Task ActivateCurrentPageAsync() => State != ShellState.SignedIn
        ? Task.CompletedTask
        : CurrentPage switch
        {
            AppPage.Courses => Courses.EnsureLoadedAsync(),
            AppPage.Calendar => Calendar.EnsureLoadedAsync(),
            _ => Task.CompletedTask,
        };

    private void SignedOut()
    {
        UserName = null;
        State = ShellState.SignedOut;
        _catalog.Clear();
        Courses.Reset();
        Calendar.Reset();
    }
}
