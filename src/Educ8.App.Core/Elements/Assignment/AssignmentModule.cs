using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Documents;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>Assignments (learning tool 5006) open in the app: description, files, status and feedback, the student's answer, and hand-in.</summary>
public sealed class AssignmentModule(LearningToolContentReader content, MaterialFiles files, TimeProvider time, AssignmentHandIn handIn) : IElementModule
{
    public bool Handles(ElementRef element) => element.IsTool(LearningTools.Assignment);

    public OpenStyle Style => OpenStyle.InApp;

    public Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, IProgress<double>? progress, CancellationToken ct) =>
        Task.FromResult<ElementPageViewModel?>(new AssignmentViewModel(element, navigator, content, files, time, handIn));
}

/// <summary>A label and value shown in an assignment's facts ("Hand-in" · "Individual").</summary>
public sealed record AssignmentFact(string Label, string Value);

/// <summary>
/// An assignment for the signed-in student, ready to show: deadline, facts, description, the teacher's files, the student's answer,
/// status/score/feedback (from the web view) and whether it can be handed in from the app (see <see cref="HandInDisabledText"/>).
/// </summary>
public sealed partial class AssignmentViewModel : ElementPageViewModel
{
    private readonly LearningToolContentReader _content;
    private readonly MaterialFiles _files;
    private readonly TimeProvider _time;
    private readonly AssignmentHandIn _handIn;
    private AssignmentInfo? _info;

    internal AssignmentViewModel(ElementRef element, ElementNavigator navigator, LearningToolContentReader content, MaterialFiles files,
        TimeProvider time, AssignmentHandIn handIn)
        : base(element, navigator)
    {
        _content = content;
        _files = files;
        _time = time;
        _handIn = handIn;
    }

    /// <summary>Raised with the hand-in form when the user starts a hand-in (the platform shows it as a dialog).</summary>
    public event EventHandler<HandInViewModel>? HandInStarted;

    /// <summary>Raised after a successful hand-in (the platform can show a toast); the page reloads itself.</summary>
    public event EventHandler? HandedIn;

    public AssignmentInfo? Info => _info;

    public string CourseTitle => _info?.Element?.CourseTitle ?? "";

    /// <summary>"Due Fri 20 Nov, 23:59 · in 2 days", "Due … · passed · late hand-ins allowed", or "No deadline".</summary>
    public string DeadlineText { get; private set; } = "";

    public IReadOnlyList<AssignmentFact> Facts { get; private set; } = [];
    public RichDocument Description { get; private set; } = RichDocument.Empty;
    public IReadOnlyList<AssignmentFileItemViewModel> TeacherFiles { get; private set; } = [];

    /// <summary>"Handed in Mon 16 Nov, 10:00", "Draft saved …", or "Not handed in yet.".</summary>
    public string SubmissionText { get; private set; } = "";
    public RichDocument AnswerText { get; private set; } = RichDocument.Empty;
    public IReadOnlyList<AssignmentFileItemViewModel> AnswerFiles { get; private set; } = [];

    public string? StatusText { get; private set; }
    public string? ScoreText { get; private set; }
    public string? AssessedByText { get; private set; }
    public RichDocument Feedback { get; private set; } = RichDocument.Empty;
    public IReadOnlyList<AssignmentFileItemViewModel> FeedbackFiles { get; private set; } = [];

    /// <summary>"Not assessed yet." when there's no assessment to show; null when there is (or the view couldn't be read).</summary>
    public string? NotAssessedText { get; private set; }

    public IReadOnlyList<string> GradingScale { get; private set; } = [];

    /// <summary>Set when status, score and feedback couldn't be read (the rest of the page still shows).</summary>
    public string? ViewUnavailableText { get; private set; }

    /// <summary>Why the app won't hand in this assignment (null when it can); shown instead of the hand-in button.</summary>
    public string? HandInDisabledText { get; private set; }

    public bool CanHandIn => _info is not null && HandInDisabledText is null;

    protected override bool HasContent => _info is not null;

    protected override async Task LoadAsync(CancellationToken ct)
    {
        var info = await Navigator.Client().LoadAssignmentAsync(_content, Element.ElementId, ct);
        ct.ThrowIfCancellationRequested();
        Apply(info);
    }

    private void Apply(AssignmentInfo info)
    {
        _info = info;
        var d = info.Details;
        var now = _time.GetUtcNow();
        var zone = _time.LocalTimeZone;
        var site = Navigator.Client().BaseAddress;
        var cacheKey = Path.Combine("assignments", d.ElementId.ToString(CultureInfo.InvariantCulture));

        DeadlineText = Deadline(d, now, zone);
        Facts = FactsOf(d);
        Description = HtmlToRichText.Convert(d.RawText, site);
        var teacherFiles = d.Files.Select(f => File(f, ItslearningClient.AssignmentFilePath(d.ElementId, f.FileId), cacheKey, null)).ToList();
        if (d.TemplateFile is { } template && d.Files.All(f => f.FileId != template.FileId))
            teacherFiles.Add(File(template, ItslearningClient.AssignmentFilePath(d.ElementId, template.FileId), cacheKey, Strings.AssignmentTemplate));
        TeacherFiles = teacherFiles;

        var answer = info.Answer;
        SubmissionText = answer switch
        {
            { IsDraft: true } a => string.Format(CultureInfo.CurrentCulture, Strings.AssignmentDraft, When(a.Submitted, now, zone)),
            { } a => string.Format(CultureInfo.CurrentCulture, Strings.AssignmentHandedIn, When(a.Submitted, now, zone)),
            null when info.View?.SubmittedLine is { } line => line,
            _ => Strings.AssignmentNotHandedIn,
        };
        AnswerText = HtmlToRichText.Convert(answer?.Text, site);
        AnswerFiles = answer is null ? [] : answer.Files.Select(f => File(f, ItslearningClient.AnswerFilePath(d.ElementId, answer.AnswerId, f.FileId), cacheKey, null)).ToList();

        var view = info.View;
        StatusText = view?.Status;
        ScoreText = view?.Score;
        AssessedByText = view?.AssessedBy;
        Feedback = HtmlToRichText.Convert(view?.FeedbackHtml, site);
        FeedbackFiles = answer is null ? [] : info.FeedbackFiles.Select(f => File(f, ItslearningClient.FeedbackFilePath(d.ElementId, answer.AnswerId, f.FileId), cacheKey, null)).ToList();
        NotAssessedText = view is not null && !view.HasAssessment && FeedbackFiles.Count == 0 ? Strings.AssignmentNotAssessed : null;
        ViewUnavailableText = info.ViewUnavailable ? Strings.AssignmentViewUnavailable : null;

        GradingScale = info.Settings?.AssessmentScale?.AssessmentItems.Select(Grade).ToList() ?? [];
        HandInDisabledText = _handIn.WhyNot(info, now);

        OnPropertyChanged(new PropertyChangedEventArgs(null)); // everything above changed
        StartHandInCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanHandIn))]
    private void StartHandIn()
    {
        if (_info is null) return;
        var form = new HandInViewModel(_info, _handIn, _time);
        form.Submitted += async (_, _) =>
        {
            HandedIn?.Invoke(this, EventArgs.Empty);
            await RefreshCommand.ExecuteAsync(null);
        };
        HandInStarted?.Invoke(this, form);
    }

    private AssignmentFileItemViewModel File(AssignmentFile f, string path, string cacheKey, string? note) =>
        new(f, path, cacheKey, note, _files);

    private static string Deadline(AssignmentDetails d, DateTimeOffset now, TimeZoneInfo zone)
    {
        if (d.Deadline is not { } deadline) return Strings.AssignmentNoDeadline;
        var parts = new List<string> { string.Format(CultureInfo.CurrentCulture, Strings.DueOn, Texts.Date(deadline, now, zone)) };
        var left = deadline - now;
        if (left < TimeSpan.Zero)
        {
            parts.Add(Strings.AssignmentDeadlinePassed);
            if (d.IsSubmissionAfterDeadlineAllowed) parts.Add(Strings.AssignmentLateAllowed);
        }
        else if (left < TimeSpan.FromDays(2))
            parts.Add(string.Format(CultureInfo.CurrentCulture, Strings.AssignmentInHours, Math.Max(1, (int)Math.Ceiling(left.TotalHours))));
        else
            parts.Add(string.Format(CultureInfo.CurrentCulture, Strings.AssignmentInDays, (int)Math.Floor(left.TotalDays)));
        return string.Join(" · ", parts);
    }

    private static IReadOnlyList<AssignmentFact> FactsOf(AssignmentDetails d)
    {
        var facts = new List<AssignmentFact>();
        if (d.SubmissionType is { } type)
            facts.Add(new(Strings.AssignmentSubmissionType, type.Equals("Group", StringComparison.OrdinalIgnoreCase) ? Strings.AssignmentGroup : Strings.AssignmentIndividual));
        if (d.MaxScore is { } max) facts.Add(new(Strings.AssignmentMaxScore, max.ToString("0.##", CultureInfo.CurrentCulture)));
        if (d.IsMandatory) facts.Add(new(Strings.AssignmentMandatory, Strings.AssignmentYes));
        if (d.IsAnonymous) facts.Add(new(Strings.AssignmentAnonymous, Strings.AssignmentYes));
        if (d.PeersToAssessCount is > 0 and var peers)
            facts.Add(new(Strings.AssignmentPeerReview, string.Format(CultureInfo.CurrentCulture, Strings.AssignmentPeerReviewCount, peers)));
        return facts;
    }

    private static string When(DateTimeOffset? at, DateTimeOffset now, TimeZoneInfo zone) => at is { } a ? Texts.Date(a, now, zone) : "";

    private static string Grade(AssessmentScaleItem item) =>
        item.PercentFromAndIncl is { } from && item.PercentTo is { } to
            ? $"{item.Title} ({from.ToString("0.##", CultureInfo.CurrentCulture)}–{to.ToString("0.##", CultureInfo.CurrentCulture)} %)"
            : item.Title;
}

/// <summary>A file on an assignment (teacher's, the student's own, or feedback): opens in the default application.</summary>
public sealed partial class AssignmentFileItemViewModel : ObservableObject
{
    private readonly MaterialFiles _files;
    private readonly string _path;
    private readonly string _cacheKey;

    internal AssignmentFileItemViewModel(AssignmentFile file, string path, string cacheKey, string? note, MaterialFiles files)
    {
        _files = files;
        _path = path;
        _cacheKey = cacheKey;
        Name = file.Name;
        DetailText = string.Join(" · ", new[] { note, file.Size > 0 ? DocumentExtractor.Size(file.Size) : null }.Where(s => !string.IsNullOrEmpty(s)));
    }

    public string Name { get; }

    /// <summary>"Template · 1.2 MB".</summary>
    public string DetailText { get; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>How far a running download got (0–1), or null when unknown.</summary>
    [ObservableProperty]
    public partial double? DownloadFraction { get; private set; }

    /// <summary>"Saved to Downloads" after a copy was saved.</summary>
    [ObservableProperty]
    public partial string? NoticeText { get; private set; }

    /// <summary>Downloads the file if needed and asks where to save a copy.</summary>
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task SaveCopyAsync(CancellationToken ct)
    {
        ErrorMessage = NoticeText = null;
        DownloadFraction = null;
        try
        {
            if (await _files.SaveCopyAsync(await _files.DownloadRestFileAsync(_path, _cacheKey, Name, new Progress<double>(p => DownloadFraction = p), ct), ct) is { } saved)
                NoticeText = Texts.SavedTo(saved);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.SaveFailed, Texts.Error(e));
        }
    }

    [RelayCommand(IncludeCancelCommand = true)]
    private async Task OpenAsync(CancellationToken ct)
    {
        ErrorMessage = NoticeText = null;
        DownloadFraction = null;
        try
        {
            await _files.OpenRestFileAsync(_path, _cacheKey, Name, new Progress<double>(p => DownloadFraction = p), ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            ErrorMessage = Texts.Error(e);
        }
    }
}
