using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Elements;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Documents;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.ViewModels;

/// <summary>Creates course pages and their parts. Element rows come from the <see cref="ElementNavigator"/>, which decides how they open.</summary>
public sealed class CoursePageFactory(Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, ElementNavigator navigator)
{
    public CourseViewModel Course(int courseId, string title, string code, Uri? url) => new(client, time, openUrl, navigator, courseId, title, code, url);

    public CoursePlansViewModel Plans(int courseId) => new(client, time, navigator, courseId);

    public PlanPageViewModel PlanPage(int courseId, string courseTitle, PlanItemViewModel plan, Action openCourse) =>
        new(courseId, courseTitle, plan, openCourse);

    /// <summary>
    /// The course's plans that run today (local date): a single-day plan dated today, or a period that includes today. With
    /// their material, like the course overview's plans.
    /// </summary>
    internal async Task<List<PlanItemViewModel>> ActivePlansAsync(int courseId, CancellationToken ct)
    {
        var c = client();
        var now = time.GetUtcNow();
        var zone = time.LocalTimeZone;
        var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(now, zone).DateTime);
        // The planner returns plans overlapping the window; tomorrow as the end so plans later today are included.
        var plans = (await c.GetCoursePlansAsync(courseId, today, today.AddDays(1), ct)).Where(p => RunsOn(p, today, zone)).ToList();
        if (plans.Count == 0) return [];
        IReadOnlyList<CourseTopic>? topics;
        try { topics = await c.GetCourseTopicsAsync(courseId, ct); }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException) { topics = null; }
        var materials = await c.GetPlanMaterialsAsync(courseId, topics, plans, ct);
        var topicNames = topics?.ToDictionary(t => t.TopicId, t => t.TopicName) ?? [];
        return plans.Select(p => new PlanItemViewModel(p.PlanName, topicNames.GetValueOrDefault(p.TopicId), p.PlanDescription,
            materials.GetValueOrDefault(p.PlanId), Texts.Span(p.Start, p.Stop, now, zone), navigator)).ToList();
    }

    /// <summary>True if the plan has a date and <paramref name="day"/> falls within it. An end at midnight belongs to the day before.</summary>
    internal static bool RunsOn(PlanPreview p, DateOnly day, TimeZoneInfo zone)
    {
        if (p.Start is not { } start) return false;
        var first = TimeZoneInfo.ConvertTime(start, zone);
        var last = p.Stop is { } stop && stop > start ? TimeZoneInfo.ConvertTime(stop, zone) : first;
        if (last > first && last.TimeOfDay == TimeSpan.Zero) last = last.AddTicks(-1);
        return DateOnly.FromDateTime(first.DateTime) <= day && day <= DateOnly.FromDateTime(last.DateTime);
    }

    internal MaterialItemViewModel Item(CourseResource r, string? detail = null) => navigator.Item(ElementRef.From(r), detail);
}

/// <summary>
/// One section of the course overview. Sections load independently: one failing (e.g. no access to the planner) shows its own
/// message and doesn't hide the others.
/// </summary>
public sealed partial class CourseSection<T>(string title, string emptyText) : ObservableObject
{
    public string Title { get; } = title;
    public ObservableCollection<T> Items { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial bool IsLoading { get; internal set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial string? ErrorMessage { get; internal set; }

    /// <summary>What to show instead of items: the error, the empty text, or null (there are items, or still loading).</summary>
    public string? EmptyText => ErrorMessage ?? (!IsLoading && Items.Count == 0 ? emptyText : null);

    internal async Task LoadAsync<TData>(Func<Task<TData>> fetch, Func<TData, IEnumerable<T>> map, CancellationToken ct)
    {
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var data = await fetch();
            if (ct.IsCancellationRequested) return;
            Items.Clear();
            foreach (var item in map(data)) Items.Add(item);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            if (!ct.IsCancellationRequested) ErrorMessage = Texts.Error(e);
        }
        finally
        {
            if (!ct.IsCancellationRequested) IsLoading = false;
            OnPropertyChanged(nameof(EmptyText));
        }
    }
}

/// <summary>A course announcement (light bulletin).</summary>
public sealed partial class AnnouncementItemViewModel(string text, string metaText, bool pinned)
{
    /// <summary>The whole announcement, line breaks kept.</summary>
    public string Text { get; } = text;

    /// <summary>
    /// The announcement as one paragraph (line breaks and runs of spaces collapsed), for a mail-style preview row: platforms
    /// show its first lines and the whole <see cref="Text"/> when expanded.
    /// </summary>
    public string PreviewText { get; } = Whitespace().Replace(text, " ").Trim();

    [System.Text.RegularExpressions.GeneratedRegex(@"\s+")]
    private static partial System.Text.RegularExpressions.Regex Whitespace();

    /// <summary>"Anna Schollain · Today, 09:12".</summary>
    public string MetaText { get; } = metaText;
    public bool IsPinned { get; } = pinned;
}

/// <summary>
/// The course page. Overview: announcements, lesson plans (last week to three weeks ahead) with their material, open tasks and
/// recent changes. Resources: the course's folders (<see cref="Resources"/>), loaded when first shown.
/// </summary>
public sealed partial class CourseViewModel : ObservableObject
{
    internal const int PlansDaysBack = 7, PlansDaysAhead = 21, ChangesDays = 14, MaxChanges = 15, MaxAnnouncements = 5;

    private readonly Func<ItslearningClient> _client;
    private readonly TimeProvider _time;
    private readonly IOpenUrl _openUrl;
    private readonly ElementNavigator _navigator;
    private CancellationTokenSource? _loading;
    private bool _loaded;

    internal CourseViewModel(Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, ElementNavigator navigator,
        int courseId, string title, string code, Uri? url)
    {
        _client = client;
        _time = time;
        _openUrl = openUrl;
        _navigator = navigator;
        CourseId = courseId;
        Title = title;
        Subtitle = code;
        Url = url;
        Resources = FolderViewModel.CourseRoot(navigator, courseId, url);
        AllPlans = new CoursePlansViewModel(client, time, navigator, courseId);
    }

    public int CourseId { get; }
    public string Title { get; }
    public string Subtitle { get; }
    public Uri? Url { get; }

    public CourseSection<AnnouncementItemViewModel> Announcements { get; } = new(Strings.CourseAnnouncements, Strings.CourseNoAnnouncements);
    public CourseSection<PlanItemViewModel> Plans { get; } = new(Strings.CoursePlans, Strings.CourseNoPlans);
    public CourseSection<MaterialItemViewModel> Tasks { get; } = new(Strings.CourseTasks, Strings.CourseNoTasks);
    public CourseSection<MaterialItemViewModel> Changes { get; } = new(Strings.CourseChanges, Strings.CourseNoChanges);

    /// <summary>Every plan in the course, by topic. Call its <see cref="CoursePlansViewModel.EnsureLoadedAsync"/> when the tab is shown.</summary>
    public CoursePlansViewModel AllPlans { get; }

    /// <summary>The course's top-level folder. Call its <see cref="FolderViewModel.EnsureLoadedAsync"/> when the tab is shown.</summary>
    public FolderViewModel Resources { get; }

    public string OverviewTitle => Strings.CourseOverview;

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        _loaded = true;
        await LoadAsync();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        _loaded = true;
        await Task.WhenAll(LoadAsync(), Resources.RefreshCommand.ExecuteAsync(null), AllPlans.RefreshIfLoadedAsync());
    }

    [RelayCommand]
    private void OpenInItslearning()
    {
        if (Url is not null) _openUrl.Open(Url);
    }

    private async Task LoadAsync()
    {
        _loading?.Cancel();
        var cts = _loading = new CancellationTokenSource();
        var ct = cts.Token;
        IsLoading = true;
        try
        {
            var c = _client();
            var now = _time.GetUtcNow();
            var zone = _time.LocalTimeZone;
            var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(now, zone).DateTime);

            await Task.WhenAll(
                Announcements.LoadAsync(() => c.GetCourseBulletinsAsync(CourseId, MaxAnnouncements, ct),
                    list => list.EntityArray.Select(b => new AnnouncementItemViewModel(
                        b.Text.Trim(), Meta(b.PublishedBy?.FullName, b.PublishedDate, now, zone), b.Pinned)), ct),
                Plans.LoadAsync(async () =>
                    {
                        var plans = await c.GetCoursePlansAsync(CourseId, today.AddDays(-PlansDaysBack), today.AddDays(PlansDaysAhead), ct);
                        var topics = await TryAsync(() => c.GetCourseTopicsAsync(CourseId, ct));
                        var materials = await c.GetPlanMaterialsAsync(CourseId, topics, plans, ct);
                        var topicNames = topics?.ToDictionary(t => t.TopicId, t => t.TopicName) ?? [];
                        return plans.Select(p => new PlanItemViewModel(p.PlanName, topicNames.GetValueOrDefault(p.TopicId), p.PlanDescription,
                            materials.GetValueOrDefault(p.PlanId), Texts.Span(p.Start, p.Stop, now, zone), _navigator)).ToList();
                    },
                    plans => plans, ct),
                Tasks.LoadAsync(() => c.GetCourseTasksAsync(CourseId, "Active", ct),
                    tasks => tasks.OrderBy(t => t.Deadline ?? DateTimeOffset.MaxValue).Select(t =>
                        _navigator.Item(ElementRef.From(t),
                            t.Deadline is { } d ? string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.DueOn, Texts.Date(d, now, zone)) : null)), ct),
                Changes.LoadAsync(() => c.GetCourseStreamAsync(CourseId, now.AddDays(-ChangesDays), MaxChanges, ct),
                    items => items.Select(i => _navigator.Item(ElementRef.From(i, HtmlToText.Convert(i.Text)),
                        Meta(i.PublishedBy?.FullName, i.PublishedDate, now, zone)
                        + (i.IsGroup ? $" · {string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.MoreItems, i.ElementsCount)}" : ""))), ct));
        }
        finally
        {
            if (_loading == cts) IsLoading = false;
        }
    }

    private static string Meta(string? who, DateTimeOffset? when, DateTimeOffset now, TimeZoneInfo zone) =>
        string.Join(" · ", new[] { who?.Trim(), when is { } w ? Texts.Date(w, now, zone) : null }.Where(s => !string.IsNullOrEmpty(s)));

    private static async Task<T?> TryAsync<T>(Func<Task<T>> fetch) where T : class
    {
        try { return await fetch(); }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException) { return null; }
    }
}

/// <summary>
/// The course page's Plans tab: every plan in the course in itslearning's course order (across topics), each with its material.
/// A plan's topic is shown with it when it differs from the plan's name (many courses have one topic per week, named like the plan).
/// </summary>
public sealed partial class CoursePlansViewModel : ObservableObject
{
    private readonly Func<ItslearningClient> _client;
    private readonly TimeProvider _time;
    private readonly ElementNavigator _navigator;
    private CancellationTokenSource? _loading;
    private bool _loaded;

    internal CoursePlansViewModel(Func<ItslearningClient> client, TimeProvider time, ElementNavigator navigator, int courseId)
    {
        _client = client;
        _time = time;
        _navigator = navigator;
        CourseId = courseId;
    }

    public int CourseId { get; }
    public string Title => Strings.CoursePlansTab;
    public ObservableCollection<PlanItemViewModel> Plans { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>"This course has no plans." once loaded with none; otherwise null.</summary>
    public string? EmptyText => _loaded && !IsLoading && ErrorMessage is null && Plans.Count == 0 ? Strings.CourseNoPlansAtAll : null;

    public async Task EnsureLoadedAsync()
    {
        if (!_loaded) await RefreshAsync();
    }

    internal Task RefreshIfLoadedAsync() => _loaded ? RefreshAsync() : Task.CompletedTask;

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
            var c = _client();
            var groups = await c.GetAllPlansAsync(CourseId, cts.Token);
            if (cts.IsCancellationRequested) return;
            var now = _time.GetUtcNow();
            var zone = _time.LocalTimeZone;
            Plans.Clear();
            var all = groups.SelectMany(g => g.Plans.Select(p => (Topic: g.Topic?.Name, Plan: p))).OrderBy(x => x.Plan.OrderNumber);
            foreach (var (topic, p) in all)
            {
                var topicText = topic is not null && !topic.Trim().Equals(p.Title.Trim(), StringComparison.OrdinalIgnoreCase) ? topic : null;
                Plans.Add(new PlanItemViewModel(p.Title, topicText, p.Description, new PlanMaterials(null, c.MaterialsOf(CourseId, p)),
                    p.Start is null ? Strings.PlanNoDate : Texts.Span(p.Start, p.Stop, now, zone), _navigator));
            }
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
}

/// <summary>
/// One lesson plan on its own page (opened from a course card's active plans): when it runs, its description and material.
/// The material opens through the element modules like everywhere else.
/// </summary>
public sealed partial class PlanPageViewModel
{
    private readonly Action _openCourse;

    internal PlanPageViewModel(int courseId, string courseTitle, PlanItemViewModel plan, Action openCourse)
    {
        _openCourse = openCourse;
        CourseId = courseId;
        CourseTitle = courseTitle;
        Plan = plan;
    }

    public int CourseId { get; }
    public string CourseTitle { get; }
    public PlanItemViewModel Plan { get; }

    /// <summary>Opens the plan's course page.</summary>
    [RelayCommand]
    private void OpenCourse() => _openCourse();
}
