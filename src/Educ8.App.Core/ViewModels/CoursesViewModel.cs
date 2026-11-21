using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.ViewModels;

/// <summary>
/// Start page: all the user's courses, with search and a current-semester filter, shown as a grid of cards or as a list of
/// collapsible rows (<see cref="Layout"/>, remembered). Expanded, a row shows the course's current plans and its teachers.
/// </summary>
public sealed partial class CoursesViewModel(CourseCatalog catalog, TimeProvider time, IOpenUrl openUrl, CoursePageFactory pages, AppPreferences preferences)
    : ObservableObject
{
    /// <summary>Grid of cards (default) or list of collapsible rows. Saved when changed.</summary>
    [ObservableProperty]
    public partial CoursesLayout Layout { get; set; } = preferences.CoursesLayout;

    partial void OnLayoutChanged(CoursesLayout value) => preferences.CoursesLayout = value;

    /// <summary>Raised with the course page when a card is opened; the platform shows it (pushes a page).</summary>
    public event EventHandler<CourseViewModel>? CourseOpened;

    /// <summary>Raised with a plan's page when an active plan on a card is opened; the platform pushes it.</summary>
    public event EventHandler<PlanPageViewModel>? PlanOpened;

    [RelayCommand]
    private void OpenCourse(CourseCardViewModel card) =>
        CourseOpened?.Invoke(this, pages.Course(card.CourseId, card.Title, card.Code, card.Url));

    private void OpenPlan(CourseCardViewModel card, PlanItemViewModel plan) =>
        PlanOpened?.Invoke(this, pages.PlanPage(card.CourseId, card.Title, plan, () => OpenCourse(card)));

    /// <summary>Cards by course id, kept while searching and filtering (so expanded cards stay expanded); rebuilt on load.</summary>
    private Dictionary<int, CourseCardViewModel> _cards = [];

    /// <summary>Courses without a semester code count as current when active within this many days.</summary>
    internal const int RecentDays = 120;

    private IReadOnlyList<PersonCourse> _all = [];
    private bool _loaded;

    /// <summary>The cards to show (filtered and sorted, most recently active first).</summary>
    public ObservableCollection<CourseCardViewModel> Courses { get; } = [];

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    [ObservableProperty]
    public partial string SearchText { get; set; } = "";

    /// <summary>false: current semester only (default); true: all courses.</summary>
    [ObservableProperty]
    public partial bool ShowAll { get; set; }

    /// <summary>"12 courses".</summary>
    [ObservableProperty]
    public partial string CountText { get; private set; } = "";

    /// <summary>Shown instead of the grid when there are no cards (and no error).</summary>
    [ObservableProperty]
    public partial string? EmptyText { get; private set; }

    public bool IsEmpty => EmptyText is not null;

    public Task EnsureLoadedAsync() => _loaded ? Task.CompletedTask : LoadAsync(refresh: false);

    [RelayCommand]
    private Task RefreshAsync() => LoadAsync(refresh: true);

    private async Task LoadAsync(bool refresh)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            _all = await catalog.GetAsync(refresh);
            _loaded = true;
            var old = _cards;
            var now = time.GetUtcNow();
            _cards = _all.ToDictionary(c => c.CourseId, c => new CourseCardViewModel(c, now, time.LocalTimeZone, openUrl, pages, OpenPlan));
            Apply();
            // Expanded cards stay expanded across a refresh (and load their plans again).
            foreach (var (id, card) in old)
                if (card.IsExpanded && _cards.TryGetValue(id, out var fresh)) fresh.IsExpanded = true;
        }
        catch (Exception e)
        {
            ErrorMessage = Texts.Error(e);
        }
        finally
        {
            IsLoading = false;
        }
    }

    /// <summary>Back to the unloaded state (after sign-out).</summary>
    public void Reset()
    {
        _all = [];
        _cards = [];
        _loaded = false;
        Courses.Clear();
        ErrorMessage = null;
        EmptyText = null;
        CountText = "";
        SearchText = "";
    }

    partial void OnSearchTextChanged(string value) => Apply();

    partial void OnShowAllChanged(bool value) => Apply();

    partial void OnEmptyTextChanged(string? value) => OnPropertyChanged(nameof(IsEmpty));

    private void Apply()
    {
        var now = time.GetUtcNow();
        var semester = CourseCodes.CurrentSemester(DateOnly.FromDateTime(time.GetLocalNow().DateTime));
        var search = SearchText.Trim();

        var visible = _all
            .Where(c => ShowAll || IsCurrent(c, semester, now))
            .Where(c => search.Length == 0
                        || c.Title.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                        || (c.CourseCode?.Contains(search, StringComparison.CurrentCultureIgnoreCase) ?? false))
            .OrderByDescending(c => c.LastUpdatedUtc ?? DateTimeOffset.MinValue)
            .ThenBy(c => c.Title, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        Courses.Clear();
        foreach (var c in visible) Courses.Add(_cards[c.CourseId]);

        CountText = visible.Count == 1 ? Strings.CourseCountOne : string.Format(CultureInfo.CurrentCulture, Strings.CourseCount, visible.Count);
        EmptyText = visible.Count > 0 || !_loaded ? null
            : search.Length > 0 ? string.Format(CultureInfo.CurrentCulture, Strings.NoCoursesMatch, search)
            : _all.Count == 0 ? Strings.NoCourses
            : Strings.NoCurrentCourses;
    }

    /// <summary>
    /// Current semester: the course code ends in the running semester (E26/F26), or the course has no semester code and was
    /// active recently (e.g. study-programme communities).
    /// </summary>
    internal static bool IsCurrent(PersonCourse c, string currentSemester, DateTimeOffset now) =>
        CourseCodes.Semester(c.CourseCode) is { } s
            ? s == currentSemester
            : c.LastUpdatedUtc is { } updated && now - updated <= TimeSpan.FromDays(RecentDays);
}

/// <summary>
/// One course (a grid card or a list row): collapsed it shows the course name; <see cref="IsExpanded"/> shows <see cref="ActivePlans"/> (loaded the first
/// time it expands) and the teachers. Rebuilt when the courses are loaded again.
/// </summary>
public sealed partial class CourseCardViewModel : ObservableObject
{
    private readonly IOpenUrl _openUrl;
    private readonly CoursePageFactory _pages;
    private readonly Action<CourseCardViewModel, PlanItemViewModel> _openPlan;
    private bool _plansLoaded;

    internal CourseCardViewModel(PersonCourse c, DateTimeOffset now, TimeZoneInfo zone, IOpenUrl openUrl, CoursePageFactory pages,
        Action<CourseCardViewModel, PlanItemViewModel> openPlan)
    {
        _openUrl = openUrl;
        _pages = pages;
        _openPlan = openPlan;
        CourseId = c.CourseId;
        Title = c.Title;
        Code = c.CourseCode ?? "";
        var semester = CourseCodes.Semester(c.CourseCode);
        SemesterText = semester is null ? "" : string.Format(CultureInfo.CurrentCulture, Strings.Semester, semester);
        var teachers = c.TeachersInCourse.Select(t => t.DisplayName).Where(n => n.Length > 0).ToList();
        TeachersText = Texts.Teachers(teachers);
        TeacherNames = string.Join(", ", teachers);
        LastActivityText = Texts.LastActivity(c.LastUpdatedUtc, now, zone);
        NotificationCount = c.NewNotificationsCount;
        BulletinCount = c.NewBulletinsCount;
        var badges = new List<string>();
        if (c.NewNotificationsCount > 0) badges.Add(string.Format(CultureInfo.CurrentCulture, Strings.BadgeNotifications, c.NewNotificationsCount));
        if (c.NewBulletinsCount > 0)
            badges.Add(c.NewBulletinsCount == 1 ? Strings.BadgeBulletinsOne : string.Format(CultureInfo.CurrentCulture, Strings.BadgeBulletins, c.NewBulletinsCount));
        BadgeText = string.Join(" · ", badges);
        AccentColor = c.CourseColor ?? "#72838D";
        FillColor = c.CourseFillColor ?? "#D9E3E8";
        Url = Uri.TryCreate(c.Url, UriKind.Absolute, out var url) ? url : null;
    }

    public int CourseId { get; }
    public string Title { get; }
    public string Code { get; }
    public string SemesterText { get; }
    public string TeachersText { get; }

    /// <summary>All teachers, comma-separated (empty if none are listed).</summary>
    public string TeacherNames { get; }

    public bool HasTeachers => TeacherNames.Length > 0;

    /// <summary>Current plans: a single-day plan dated today, or a period that includes today.</summary>
    public CourseSection<PlanItemViewModel> ActivePlans { get; } = new(Strings.CourseActivePlans, Strings.CourseNoActivePlans);

    /// <summary>False by default (only the name shows). Expanding loads <see cref="ActivePlans"/> the first time.</summary>
    [ObservableProperty]
    public partial bool IsExpanded { get; set; }

    partial void OnIsExpandedChanged(bool value)
    {
        if (value && !_plansLoaded) _ = RefreshPlansAsync();
    }

    /// <summary>Loads <see cref="ActivePlans"/> (again).</summary>
    public Task RefreshPlansAsync()
    {
        _plansLoaded = true;
        return ActivePlans.LoadAsync(() => _pages.ActivePlansAsync(CourseId, CancellationToken.None), plans => plans, CancellationToken.None);
    }

    /// <summary>Opens an active plan on its own page.</summary>
    [RelayCommand]
    private void OpenPlan(PlanItemViewModel plan) => _openPlan(this, plan);
    public string LastActivityText { get; }
    public int NotificationCount { get; }
    public int BulletinCount { get; }

    /// <summary>"3 new · 1 announcement", or empty.</summary>
    public string BadgeText { get; }

    public bool HasBadges => BadgeText.Length > 0;

    /// <summary>Course colour as hex (accent and light background).</summary>
    public string AccentColor { get; }

    public string FillColor { get; }

    public Uri? Url { get; }

    [RelayCommand]
    private void OpenInItslearning()
    {
        if (Url is not null) _openUrl.Open(Url);
    }
}
