using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Documents;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.ViewModels;

/// <summary>
/// The Notifications page: itslearning's personal notifications (the bell), newest first, and the unread count for the sidebar.
/// Opening a notification marks it read on itslearning (as the website does) and opens it: in the app when it's about a course
/// as a whole (announcements: <see cref="CourseOpened"/>), otherwise in the browser. "Mark all as read" marks every unread one. Those are the app's only writes to itslearning (the user chose them, 2026-11-21).
/// </summary>
public sealed partial class NotificationsViewModel(
    Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, CourseCatalog catalog, CoursePageFactory pages, Elements.ElementNavigator elements)
    : ObservableObject
{
    /// <summary>Raised with the course page when a notification about a course opens in the app; the platform pushes it.</summary>
    public event EventHandler<CourseViewModel>? CourseOpened;

    /// <summary>How far back and how many notifications the page lists.</summary>
    internal const int DaysBack = 180, MaxItems = 100;

    private CancellationTokenSource? _loading;
    private bool _loaded;

    public ObservableCollection<NotificationItemViewModel> Items { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Unread notifications on itslearning (the sidebar badge); 0 hides it.</summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(UnreadText), nameof(HasUnread))]
    [NotifyCanExecuteChangedFor(nameof(MarkAllReadCommand))]
    public partial int UnreadCount { get; private set; }

    public bool HasUnread => UnreadCount > 0;

    /// <summary>"3" for the badge, "99+" above that, empty when none.</summary>
    public string UnreadText => UnreadCount switch { <= 0 => "", > 99 => "99+", _ => UnreadCount.ToString(System.Globalization.CultureInfo.CurrentCulture) };

    /// <summary>"No notifications" once loaded with none; otherwise null.</summary>
    public string? EmptyText => _loaded && !IsLoading && ErrorMessage is null && Items.Count == 0 ? Strings.NotificationsNone : null;

    public Task EnsureLoadedAsync() => _loaded ? Task.CompletedTask : RefreshAsync();

    /// <summary>Reloads the list and the unread count.</summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        _loading?.Cancel();
        var cts = _loading = new CancellationTokenSource();
        _loaded = true;
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var c = client();
            var now = time.GetUtcNow();
            var zone = time.LocalTimeZone;
            var list = await c.GetNotificationsAsync(now.AddDays(-DaysBack), MaxItems, cts.Token);
            if (cts.IsCancellationRequested) return;
            Items.Clear();
            foreach (var n in list) Items.Add(new NotificationItemViewModel(n, Link(c, n), now, zone));
            await RefreshUnreadCountAsync();
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            if (!cts.IsCancellationRequested) ErrorMessage = Texts.Error(e);
        }
        finally
        {
            if (_loading == cts) IsLoading = false;
            OnPropertyChanged(nameof(EmptyText));
        }
    }

    /// <summary>Updates <see cref="UnreadCount"/> only (cheap; the shell calls it after sign-in and every few minutes). Errors are ignored.</summary>
    public async Task RefreshUnreadCountAsync()
    {
        try
        {
            UnreadCount = await client().GetUnreadNotificationCountAsync();
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
        }
    }

    /// <summary>Marks the notification read (on itslearning too) and opens it: the course page in the app, else the browser.</summary>
    [RelayCommand]
    private async Task OpenAsync(NotificationItemViewModel item)
    {
        if (await TryOpenInAppAsync(item)) { }
        // About one element (e.g. "Assessment is available for …"): opens like that element (assignments in the app).
        else if (item.ElementId is { } elementId) await elements.OpenElementAsync(elementId, item.Url);
        else if (item.Url is { } url) openUrl.Open(url);
        if (item.IsRead) return;
        await MarkReadAsync([item]);
    }

    /// <summary>Opens the course page for a notification about a course as a whole, if the course is one of the user's.</summary>
    private async Task<bool> TryOpenInAppAsync(NotificationItemViewModel item)
    {
        if (item.CourseId is not { } id) return false;
        try
        {
            if ((await catalog.GetAsync(refresh: false)).FirstOrDefault(c => c.CourseId == id) is not { } course) return false;
            CourseOpened?.Invoke(this, pages.Course(course.CourseId, course.Title, course.CourseCode ?? "",
                Uri.TryCreate(course.Url, UriKind.Absolute, out var u) ? u : null));
            return true;
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            return false; // can't list courses right now: the browser still works
        }
    }

    [RelayCommand(CanExecute = nameof(HasUnread))]
    private Task MarkAllReadAsync() => MarkReadAsync(Items.Where(i => !i.IsRead).ToList());

    private async Task MarkReadAsync(IReadOnlyList<NotificationItemViewModel> items)
    {
        try
        {
            if (items.Count > 0) await client().MarkNotificationsReadAsync(items.Select(i => i.Id));
            foreach (var i in items) i.IsRead = true;
            ErrorMessage = null;
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException or NotAuthenticatedException)
        {
            ErrorMessage = string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.NotificationsMarkReadFailed, Texts.Error(e));
        }
        await RefreshUnreadCountAsync(); // the server's count (also covers unread ones older than the list)
    }

    /// <summary>Back to the unloaded state (after sign-out).</summary>
    public void Reset()
    {
        _loading?.Cancel();
        _loaded = false;
        Items.Clear();
        ErrorMessage = null;
        UnreadCount = 0;
    }

    /// <summary>The page the notification points at: <c>Url</c>, else <c>ContentUrl</c>; relative links are resolved against the site.</summary>
    private static Uri? Link(ItslearningClient c, Notification n)
    {
        foreach (var candidate in new[] { n.Url, n.ContentUrl })
        {
            if (string.IsNullOrWhiteSpace(candidate)) continue;
            if (Uri.TryCreate(candidate, UriKind.Absolute, out var absolute) && absolute.Scheme is "http" or "https") return absolute;
            if (c.BaseAddress is { } site && Uri.TryCreate(site, candidate, out var resolved)) return resolved;
        }
        return null;
    }
}

/// <summary>One notification row.</summary>
public sealed partial class NotificationItemViewModel : ObservableObject
{
    internal NotificationItemViewModel(Notification n, Uri? url, DateTimeOffset now, TimeZoneInfo zone)
    {
        Id = n.NotificationId;
        Text = HtmlToText.Convert(n.Text).Trim();
        MetaText = string.Join(" · ", new[] { n.PublishedBy?.FullName?.Trim(), n.PublishedDate is { } d ? Texts.Date(d, now, zone) : null }
            .Where(s => !string.IsNullOrEmpty(s)));
        Icon = n.Type?.Equals("Assessment", StringComparison.OrdinalIgnoreCase) == true ? AppIcon.Assignment : AppIcon.Notification;
        Url = url;
        // About a course as a whole (announcements): shown in the app. About an element (assessment, submission): the browser.
        CourseId = n.ElementId is null ? n.CourseId : null;
        ElementId = n.ElementId;
        IsRead = n.IsRead;
    }

    public int Id { get; }
    public string Text { get; }

    /// <summary>"Ann Teacher · Yesterday, 14:05".</summary>
    public string MetaText { get; }
    public AppIcon Icon { get; }
    public Uri? Url { get; }

    /// <summary>Set when the notification opens as a course page in the app.</summary>
    public int? CourseId { get; }

    /// <summary>Set when the notification is about one element (it opens like that element: in the app where a module handles it).</summary>
    public int? ElementId { get; }

    public bool OpensInApp => CourseId is not null;

    /// <summary>Unread rows are shown emphasised; set when opened or marked read.</summary>
    [ObservableProperty]
    public partial bool IsRead { get; internal set; }
}
