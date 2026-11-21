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
    Notifications,
    Messages,
    Mcp,
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
    private readonly TimeProvider _time;
    private ITimer? _unreadTimer;

    /// <summary>How often the unread notification count is checked while signed in.</summary>
    internal static readonly TimeSpan UnreadCountInterval = TimeSpan.FromMinutes(5);

    public ShellViewModel(
        TokenManager tokens, Func<ItslearningClient> client, IUiDispatcher dispatcher, CourseCatalog catalog, TimeProvider time,
        LoginViewModel login, CoursesViewModel courses, CalendarViewModel calendar, NotificationsViewModel notifications, MessagesViewModel messages,
        McpViewModel mcp)
    {
        _tokens = tokens;
        _client = client;
        _dispatcher = dispatcher;
        _catalog = catalog;
        _time = time;
        Login = login;
        Courses = courses;
        Calendar = calendar;
        Notifications = notifications;
        Messages = messages;
        Mcp = mcp;
        _tokens.SessionChanged += (_, _) => _dispatcher.Post(() => _ = InitializeAsync());
    }

    public LoginViewModel Login { get; }
    public CoursesViewModel Courses { get; }
    public CalendarViewModel Calendar { get; }
    public NotificationsViewModel Notifications { get; }
    public MessagesViewModel Messages { get; }
    public McpViewModel Mcp { get; }

    public IReadOnlyList<NavItem> NavigationItems { get; } =
    [
        new(AppPage.Courses, Strings.NavCourses, AppIcon.Courses),
        new(AppPage.Calendar, Strings.NavCalendar, AppIcon.Calendar),
        new(AppPage.Notifications, Strings.NavNotifications, AppIcon.Notification),
        new(AppPage.Messages, Strings.NavMessages, AppIcon.Messages),
        new(AppPage.Mcp, Strings.NavMcp, AppIcon.Mcp),
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
            StartUnreadCount();
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

    partial void OnCurrentPageChanged(AppPage value)
    {
        if (value != AppPage.Mcp) Mcp.StopAutoRefresh();
        _ = ActivateCurrentPageAsync();
    }

    /// <summary>Loads the shown page's data the first time it's shown.</summary>
    private Task ActivateCurrentPageAsync() => State != ShellState.SignedIn
        ? Task.CompletedTask
        : CurrentPage switch
        {
            AppPage.Courses => Courses.EnsureLoadedAsync(),
            AppPage.Calendar => Calendar.EnsureLoadedAsync(),
            AppPage.Notifications => Notifications.RefreshCommand.ExecuteAsync(null), // always fresh when opened
            AppPage.Messages => Messages.RefreshCommand.ExecuteAsync(null),
            AppPage.Mcp => StartMcpRefresh(),
            _ => Task.CompletedTask,
        };

    private Task StartMcpRefresh()
    {
        Mcp.StartAutoRefresh();
        return Task.CompletedTask;
    }

    /// <summary>The sidebar's unread badges (notifications, messages): checked now and every <see cref="UnreadCountInterval"/> while signed in.</summary>
    private void StartUnreadCount()
    {
        RefreshUnreadCounts();
        _unreadTimer ??= _time.CreateTimer(_ => _dispatcher.Post(RefreshUnreadCounts), null, UnreadCountInterval, UnreadCountInterval);
    }

    private void RefreshUnreadCounts()
    {
        _ = Notifications.RefreshUnreadCountAsync();
        _ = Messages.RefreshUnreadCountAsync();
    }

    private void SignedOut()
    {
        _unreadTimer?.Dispose();
        _unreadTimer = null;
        Notifications.Reset();
        Messages.Reset();
        UserName = null;
        State = ShellState.SignedOut;
        _catalog.Clear();
        Courses.Reset();
        Calendar.Reset();
    }
}
