using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Documents;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.ViewModels;

/// <summary>Creates course and folder pages (they need the same services, and folders create further folders).</summary>
public sealed class CoursePageFactory(Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, MaterialFiles files)
{
    public CourseViewModel Course(int courseId, string title, string code, Uri? url) => new(this, client, time, openUrl, files, courseId, title, code, url);

    public FolderViewModel Folder(int courseId, int? folderId, string title) => new(this, client, openUrl, files, courseId, folderId, title);

    public CoursePlansViewModel Plans(int courseId) => new(client, time, openUrl, files, courseId);

    internal MaterialItemViewModel Item(CourseResource r, string? detail = null) =>
        new(r.ElementId, r.Title, r.Kind, r.Url, openUrl, files, detail);
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
    private readonly MaterialFiles _files;
    private readonly CoursePageFactory _pages;
    private CancellationTokenSource? _loading;
    private bool _loaded;

    internal CourseViewModel(CoursePageFactory pages, Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, MaterialFiles files,
        int courseId, string title, string code, Uri? url)
    {
        _pages = pages;
        _client = client;
        _time = time;
        _openUrl = openUrl;
        _files = files;
        CourseId = courseId;
        Title = title;
        Subtitle = code;
        Url = url;
        Resources = pages.Folder(courseId, null, Strings.CourseResources);
        AllPlans = pages.Plans(courseId);
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

    /// <summary>Raised with a folder's page when a folder in the overview or the Plans tab is opened (the platform pushes it).</summary>
    public event EventHandler<FolderViewModel>? FolderOpened;

    /// <summary>Opens material shown on the course page: folders as an in-app page, everything else like other material.</summary>
    [RelayCommand]
    private void OpenItem(MaterialItemViewModel item)
    {
        if (item.Kind == ResourceKind.Folder)
            FolderOpened?.Invoke(this, _pages.Folder(CourseId, item.ElementId, item.Title));
        else if (!item.OpenCommand.IsRunning)
            item.OpenCommand.Execute(null);
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
                            materials.GetValueOrDefault(p.PlanId), Texts.Span(p.Start, p.Stop, now, zone), _openUrl, _files)).ToList();
                    },
                    plans => plans, ct),
                Tasks.LoadAsync(() => c.GetCourseTasksAsync(CourseId, "Active", ct),
                    tasks => tasks.OrderBy(t => t.Deadline ?? DateTimeOffset.MaxValue).Select(t =>
                        new MaterialItemViewModel(t.ElementId, t.Title, t.Kind, t.Url, _openUrl, _files,
                            t.Deadline is { } d ? string.Format(System.Globalization.CultureInfo.CurrentCulture, Strings.DueOn, Texts.Date(d, now, zone)) : null)), ct),
                Changes.LoadAsync(() => c.GetCourseStreamAsync(CourseId, now.AddDays(-ChangesDays), MaxChanges, ct),
                    items => items.Select(i => new MaterialItemViewModel(i.ElementId, HtmlToText.Convert(i.Text), i.Kind, i.Url, _openUrl, _files,
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
/// One course folder: its items, in itslearning's order. Activating a folder raises <see cref="FolderOpened"/> with the
/// subfolder's page (the platform pushes it); anything else opens like other material (files in the default app).
/// </summary>
public sealed partial class FolderViewModel : ObservableObject
{
    private readonly CoursePageFactory _pages;
    private readonly Func<ItslearningClient> _client;
    private readonly IOpenUrl _openUrl;
    private readonly MaterialFiles _files;
    private CancellationTokenSource? _loading;
    private bool _loaded;

    internal FolderViewModel(CoursePageFactory pages, Func<ItslearningClient> client, IOpenUrl openUrl, MaterialFiles files, int courseId, int? folderId, string title)
    {
        _pages = pages;
        _client = client;
        _openUrl = openUrl;
        _files = files;
        CourseId = courseId;
        FolderId = folderId;
        Title = title;
    }

    public int CourseId { get; }

    /// <summary>Null for the course's top level.</summary>
    public int? FolderId { get; }
    public string Title { get; }
    public ObservableCollection<MaterialItemViewModel> Items { get; } = [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EmptyText))]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>"This folder is empty." when loaded with no items; otherwise null.</summary>
    public string? EmptyText => !IsLoading && ErrorMessage is null && Items.Count == 0 && _loaded ? Strings.FolderEmpty : null;

    /// <summary>Raised with the subfolder's page when a folder is opened.</summary>
    public event EventHandler<FolderViewModel>? FolderOpened;

    public async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        await RefreshAsync();
    }

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
            var items = await _client().GetFolderResourcesAsync(CourseId, FolderId, cts.Token);
            if (cts.IsCancellationRequested) return;
            Items.Clear();
            foreach (var r in items.Where(r => r.Active)) Items.Add(_pages.Item(r));
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

    /// <summary>Opens an item: folders as a new page (<see cref="FolderOpened"/>), everything else like other material.</summary>
    [RelayCommand]
    private void Open(MaterialItemViewModel item)
    {
        if (item.Kind == ResourceKind.Folder)
            FolderOpened?.Invoke(this, _pages.Folder(CourseId, item.ElementId, item.Title));
        else if (!item.OpenCommand.IsRunning)
            item.OpenCommand.Execute(null);
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
    private readonly IOpenUrl _openUrl;
    private readonly MaterialFiles _files;
    private CancellationTokenSource? _loading;
    private bool _loaded;

    internal CoursePlansViewModel(Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, MaterialFiles files, int courseId)
    {
        _client = client;
        _time = time;
        _openUrl = openUrl;
        _files = files;
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
                    p.Start is null ? Strings.PlanNoDate : Texts.Span(p.Start, p.Stop, now, zone), _openUrl, _files));
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
