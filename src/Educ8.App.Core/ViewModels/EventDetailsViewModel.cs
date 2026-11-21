using System.Collections.ObjectModel;
using System.Globalization;
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

/// <summary>Details pane for a calendar event: time, place, lesson plan with its material, linked material.</summary>
public sealed partial class EventDetailsViewModel(Func<ItslearningClient> client, TimeProvider time, IOpenUrl openUrl, ElementNavigator navigator) : ObservableObject
{
    private CancellationTokenSource? _loading;
    private Uri? _eventUrl;

    [ObservableProperty]
    public partial bool HasEvent { get; private set; }

    [ObservableProperty]
    public partial bool IsLoading { get; private set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    [ObservableProperty]
    public partial string Title { get; private set; } = "";

    /// <summary>"Thursday 19 November 2026, 12:00–14:00".</summary>
    [ObservableProperty]
    public partial string WhenText { get; private set; } = "";

    /// <summary>Room and teachers from the timetable import.</summary>
    [ObservableProperty]
    public partial string PlaceText { get; private set; } = "";

    [ObservableProperty]
    public partial string CourseTitle { get; private set; } = "";

    [ObservableProperty]
    public partial string DescriptionText { get; private set; } = "";

    public ObservableCollection<PlanItemViewModel> Plans { get; } = [];

    public ObservableCollection<MaterialItemViewModel> LinkedMaterial { get; } = [];

    public string HintText => Strings.SelectEventHint;

    public async Task LoadAsync(int eventId)
    {
        _loading?.Cancel();
        var cts = _loading = new CancellationTokenSource();
        Clear();
        IsLoading = true;
        try
        {
            var c = client();
            var e = await c.GetCalendarEventAsync(eventId, cts.Token);
            if (cts.IsCancellationRequested) return;
            var zone = time.LocalTimeZone;
            Title = e.EventTitle;
            var start = TimeZoneInfo.ConvertTime(e.FromDate, zone);
            WhenText = $"{start.ToString("D", System.Globalization.CultureInfo.CurrentCulture)}, {Texts.TimeRange(e.FromDate, e.ToDate, zone)}";
            PlaceText = string.Join(", ", new[] { e.ImportDescription?.Trim() }.Concat(e.Bookings.Select(b => b.Name)).Where(s => !string.IsNullOrEmpty(s)));
            CourseTitle = e.LocationTitle ?? "";
            DescriptionText = HtmlToText.Convert(e.Description);
            _eventUrl = Uri.TryCreate(e.EventUrl, UriKind.Absolute, out var url) ? url : null;
            foreach (var l in e.ElementLinks) LinkedMaterial.Add(navigator.Item(ElementRef.From(l, e.LocationId)));
            HasEvent = true;

            var plans = await c.GetEventPlansAsync(e, cts.Token);
            if (cts.IsCancellationRequested) return;
            foreach (var p in plans) Plans.Add(new PlanItemViewModel(p, navigator));
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            // superseded by another selection
        }
        catch (Exception ex)
        {
            ErrorMessage = Texts.Error(ex);
        }
        finally
        {
            if (_loading == cts) IsLoading = false;
        }
    }

    public void Clear()
    {
        HasEvent = false;
        ErrorMessage = null;
        Title = WhenText = PlaceText = CourseTitle = DescriptionText = "";
        _eventUrl = null;
        Plans.Clear();
        LinkedMaterial.Clear();
    }

    [RelayCommand]
    private void OpenInItslearning()
    {
        if (_eventUrl is not null) openUrl.Open(_eventUrl);
    }
}

/// <summary>A lesson plan linked from the event, with its description and material.</summary>
public sealed class PlanItemViewModel
{
    internal PlanItemViewModel(LinkedPlan plan, ElementNavigator navigator)
        : this(plan.Link.PlanName, plan.Link.TopicName, plan.Plan?.PlanDescription, plan.Materials, "", navigator)
    {
    }

    internal PlanItemViewModel(string name, string? topic, string? descriptionHtml, PlanMaterials? materials, string whenText, ElementNavigator navigator)
    {
        Name = name;
        TopicText = topic ?? "";
        DescriptionText = HtmlToText.Convert(descriptionHtml);
        WhenText = whenText;
        Material = materials?.Items.Select(r => navigator.Item(ElementRef.From(r))).ToList() ?? [];
        EmptyText = Material.Count == 0 ? Strings.EventNoMaterial : null;
    }

    public string Name { get; }

    /// <summary>When the plan runs ("Tue 17 Nov, 14:00–16:00"); empty where the context already says (event details).</summary>
    public string WhenText { get; }
    public string TopicText { get; }
    public string DescriptionText { get; }
    public IReadOnlyList<MaterialItemViewModel> Material { get; }
    public string? EmptyText { get; }
}
