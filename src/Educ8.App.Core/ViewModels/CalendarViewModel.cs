using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.ViewModels;

/// <summary>A course choice in the calendar's filter. <see cref="CourseId"/> null means all courses.</summary>
public sealed record CourseFilterOption(int? CourseId, string Title);

/// <summary>
/// Week agenda: the days of the selected week with their events, week navigation, a course filter and the selected event's details.
/// The week starts on the culture's first day of the week; week numbers are ISO-8601.
/// </summary>
public sealed partial class CalendarViewModel : ObservableObject
{
    private readonly Func<ItslearningClient> _client;
    private readonly CourseCatalog _catalog;
    private readonly TimeProvider _time;
    private List<CalendarEvent> _events = [];
    private CancellationTokenSource? _loading;
    private bool _loaded;

    public CalendarViewModel(Func<ItslearningClient> client, CourseCatalog catalog, TimeProvider time, EventDetailsViewModel details)
    {
        _client = client;
        _catalog = catalog;
        _time = time;
        Details = details;
        WeekStart = StartOfWeek(Today);
        CourseFilters.Add(AllCourses);
        SelectedCourseFilter = AllCourses;
    }

    private static readonly CourseFilterOption AllCourses = new(null, Strings.AllCourses);

    public ObservableCollection<CalendarDayViewModel> Days { get; } = [];

    public ObservableCollection<CourseFilterOption> CourseFilters { get; } = [];

    /// <summary>Details of <see cref="SelectedEvent"/>.</summary>
    public EventDetailsViewModel Details { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WeekTitle), nameof(WeekRangeText), nameof(IsCurrentWeek))]
    public partial DateOnly WeekStart { get; private set; }

    /// <summary>"Week 47".</summary>
    public string WeekTitle => string.Format(CultureInfo.CurrentCulture, Strings.WeekNumber, ISOWeek.GetWeekOfYear(WeekStart.AddDays(3).ToDateTime(TimeOnly.MinValue)));

    /// <summary>"16 – 22 November 2026" (en-GB), "November 16 – November 22, 2026" (en-US): in the culture's date order.</summary>
    public string WeekRangeText
    {
        get
        {
            var start = WeekStart.ToDateTime(TimeOnly.MinValue);
            var end = WeekStart.AddDays(6).ToDateTime(TimeOnly.MinValue);
            if (start.Year != end.Year) return $"{Texts.FullDate(start)} – {Texts.FullDate(end)}"; // across New Year: both years
            return start.Month == end.Month && Texts.DayFirst
                ? $"{start.Day} – {Texts.FullDate(end)}"
                : $"{Texts.MonthDay(start)} – {Texts.FullDate(end)}";
        }
    }

    public bool IsCurrentWeek => WeekStart == StartOfWeek(Today);

    [ObservableProperty]
    public partial CourseFilterOption SelectedCourseFilter { get; set; }

    [ObservableProperty]
    public partial CalendarEventItemViewModel? SelectedEvent { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>"Nothing in the calendar this week." when the (filtered) week is empty.</summary>
    [ObservableProperty]
    public partial string? EmptyText { get; private set; }

    private DateOnly Today => DateOnly.FromDateTime(_time.GetLocalNow().DateTime);

    public Task EnsureLoadedAsync() => _loaded ? Task.CompletedTask : LoadAsync();

    [RelayCommand]
    private Task PreviousWeekAsync() => GoToWeekAsync(WeekStart.AddDays(-7));

    [RelayCommand]
    private Task NextWeekAsync() => GoToWeekAsync(WeekStart.AddDays(7));

    [RelayCommand]
    private Task TodayAsync() => GoToWeekAsync(StartOfWeek(Today));

    /// <summary>Show the week containing <paramref name="date"/> (from a date picker).</summary>
    [RelayCommand]
    public Task GoToDateAsync(DateOnly date) => GoToWeekAsync(StartOfWeek(date));

    [RelayCommand]
    private Task RefreshAsync() => LoadAsync();

    private Task GoToWeekAsync(DateOnly weekStart)
    {
        WeekStart = weekStart;
        SelectedEvent = null;
        return LoadAsync();
    }

    /// <summary>Back to the unloaded state (after sign-out).</summary>
    public void Reset()
    {
        _loading?.Cancel();
        _events = [];
        _loaded = false;
        Days.Clear();
        SelectedEvent = null;
        Details.Clear();
        ErrorMessage = null;
        EmptyText = null;
        WeekStart = StartOfWeek(Today);
        while (CourseFilters.Count > 1) CourseFilters.RemoveAt(1);
        SelectedCourseFilter = AllCourses;
    }

    private async Task LoadAsync()
    {
        _loading?.Cancel();
        var cts = _loading = new CancellationTokenSource();
        IsLoading = true;
        ErrorMessage = null;
        try
        {
            var zone = _time.LocalTimeZone;
            var from = AtMidnight(WeekStart, zone);
            var to = AtMidnight(WeekStart.AddDays(7), zone);
            var eventsTask = _client().GetCalendarEventsAsync(from, to, cts.Token);
            var coursesTask = _catalog.GetAsync(ct: cts.Token);
            await Task.WhenAll(eventsTask, coursesTask);
            if (cts.IsCancellationRequested) return; // a newer load replaced this one
            _events = eventsTask.Result;
            UpdateCourseFilters(coursesTask.Result);
            _loaded = true;
            BuildDays();
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            // superseded
        }
        catch (Exception e)
        {
            ErrorMessage = Texts.Error(e);
        }
        finally
        {
            if (_loading == cts) IsLoading = false;
        }
    }

    partial void OnSelectedCourseFilterChanged(CourseFilterOption value)
    {
        if (_loaded) BuildDays();
    }

    partial void OnSelectedEventChanged(CalendarEventItemViewModel? value)
    {
        if (value is null) Details.Clear();
        else _ = Details.LoadAsync(value.EventId);
    }

    private void UpdateCourseFilters(IReadOnlyList<PersonCourse> courses)
    {
        // Courses that have events this week, plus the current filter, sorted by title.
        var selected = SelectedCourseFilter;
        var ids = _events.Select(e => e.LocationId).Concat(selected.CourseId is { } id ? [id] : []).ToHashSet();
        var options = courses.Where(c => ids.Contains(c.CourseId))
            .OrderBy(c => c.Title, StringComparer.CurrentCultureIgnoreCase)
            .Select(c => new CourseFilterOption(c.CourseId, c.Title))
            .ToList();
        while (CourseFilters.Count > 1) CourseFilters.RemoveAt(1);
        foreach (var o in options) CourseFilters.Add(o);
        SelectedCourseFilter = CourseFilters.FirstOrDefault(o => o.CourseId == selected.CourseId) ?? AllCourses;
    }

    private void BuildDays()
    {
        var zone = _time.LocalTimeZone;
        var today = Today;
        var filtered = SelectedCourseFilter.CourseId is { } courseId ? _events.Where(e => e.LocationId == courseId) : _events;

        Days.Clear();
        for (var i = 0; i < 7; i++)
        {
            var date = WeekStart.AddDays(i);
            var dayStart = AtMidnight(date, zone);
            var dayEnd = AtMidnight(date.AddDays(1), zone);
            var events = filtered.Where(e => e.FromDate < dayEnd && e.ToDate > dayStart)
                .OrderBy(e => e.FromDate)
                .Select(e => new CalendarEventItemViewModel(e, zone, _catalog.Find(e.LocationId)))
                .ToList();
            Days.Add(new CalendarDayViewModel(date, date == today, date < today, events));
        }
        EmptyText = Days.All(d => d.Events.Count == 0) ? Strings.NoEventsThisWeek : null;
    }

    internal static DateOnly StartOfWeek(DateOnly date)
    {
        var first = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        var diff = ((int)date.DayOfWeek - (int)first + 7) % 7;
        return date.AddDays(-diff);
    }

    private static DateTimeOffset AtMidnight(DateOnly day, TimeZoneInfo zone)
    {
        var local = day.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified);
        return new DateTimeOffset(local, zone.GetUtcOffset(local));
    }
}

/// <summary>One day in the week agenda.</summary>
public sealed class CalendarDayViewModel(DateOnly date, bool isToday, bool isPast, IReadOnlyList<CalendarEventItemViewModel> events)
{
    public DateOnly Date { get; } = date;

    /// <summary>"Monday".</summary>
    public string DayName { get; } = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek);

    /// <summary>"16 November".</summary>
    public string DateText { get; } = Texts.MonthDay(date.ToDateTime(TimeOnly.MinValue));

    public bool IsToday { get; } = isToday;
    public bool IsPast { get; } = isPast;
    public IReadOnlyList<CalendarEventItemViewModel> Events { get; } = events;
    public bool HasEvents => Events.Count > 0;
    public string NoEventsText => Strings.NoEvents;
}

/// <summary>One event row in the agenda.</summary>
public sealed class CalendarEventItemViewModel(CalendarEvent e, TimeZoneInfo zone, PersonCourse? course)
{
    public int EventId { get; } = e.EventId;
    public string Title { get; } = e.EventTitle;

    /// <summary>"12:00–14:00" (culture short time), "All day", or a span with dates.</summary>
    public string TimeText { get; } = Texts.TimeRange(e.FromDate, e.ToDate, zone);

    /// <summary>Room and teachers from the timetable import, else a short description.</summary>
    public string Subtitle { get; } = !string.IsNullOrWhiteSpace(e.ImportDescription)
        ? e.ImportDescription.Trim()
        : Documents.HtmlToMarkdown.Truncate(Documents.HtmlToText.Convert(e.Description).Replace('\n', ' '), 120);

    public string CourseTitle { get; } = e.LocationTitle ?? "";
    public int CourseId { get; } = e.LocationId;
    public string AccentColor { get; } = course?.CourseColor ?? "#72838D";
    public DateTimeOffset Start { get; } = e.FromDate;
    public DateTimeOffset End { get; } = e.ToDate;
}
