using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Documents;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;

namespace Educ8.App.Core.Elements;

/// <summary>
/// Handing in assignments from the app: when it's allowed, and the submit with a fresh check right before it. Conservative on
/// purpose (2026-11-21): only a first hand-in on an individual assignment whose state could be read, until a live test shows how
/// itslearning treats resubmissions and group hand-ins (open questions 7a/7b).
/// </summary>
public sealed class AssignmentHandIn(Func<ItslearningClient> client, LearningToolContentReader content, IPickFiles picker)
{
    internal IPickFiles Picker => picker;

    /// <summary>Why the app won't hand in this assignment now, or null when it can.</summary>
    public string? WhyNot(AssignmentInfo info, DateTimeOffset now)
    {
        var d = info.Details;
        switch (AssignmentSubmission.Check(d, now))
        {
            case SubmissionBlock.Inactive or SubmissionBlock.Closed: return Strings.HandInClosed;
            case SubmissionBlock.NotOpenYet: return Strings.HandInNotOpenYet;
            case SubmissionBlock.DeadlinePassed: return Strings.HandInDeadlinePassed;
        }
        if (info.Answer is { IsDraft: true }) return Strings.HandInDraftExists;
        if (info.Answer is not null || info.View?.SubmittedLine is not null) return Strings.HandInAlreadyDone;
        // The answer lookup answers "nothing handed in" and "error" the same way (500); only the web view can tell them apart.
        if (info.ViewUnavailable) return Strings.HandInStatusUnknown;
        if (AssignmentSubmission.IsGroup(d)) return Strings.HandInGroup;
        return null;
    }

    /// <summary>
    /// Re-reads the assignment and re-checks <see cref="WhyNot"/> (throws <see cref="HandInStoppedException"/> if something
    /// changed), then uploads the files and submits. Cancelling only stops the uploads.
    /// </summary>
    internal async Task SubmitAsync(int elementId, string? text, IReadOnlyList<string> files, TimeProvider time, IProgress<int> uploaded,
        CancellationToken uploadCt)
    {
        var fresh = await client().LoadAssignmentAsync(content, elementId, uploadCt);
        if (WhyNot(fresh, time.GetUtcNow()) is { } reason) throw new HandInStoppedException(reason);
        await client().SubmitAsync(elementId, text, files, uploaded, uploadCt);
    }

    /// <summary>After an unclear failure: whether an answer exists now (null when that can't be checked either).</summary>
    internal async Task<bool?> AnswerExistsAsync(int elementId)
    {
        try
        {
            var me = await client().GetCurrentPersonAsync();
            return await client().GetAnswerAsync(elementId, me.PersonId) is not null;
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or TaskCanceledException)
        {
            return null;
        }
    }
}

/// <summary>The fresh check right before a hand-in found a reason not to send it.</summary>
public sealed class HandInStoppedException(string reason) : Exception(reason);

/// <summary>What the user confirms before a hand-in; tied to the exact draft it describes (<see cref="Version"/>).</summary>
public sealed record HandInConfirmation(string Title, string Body, string ConfirmText, string CancelText, int Version);

/// <summary>
/// The hand-in form: answer text and files, then Review (a confirmation the platform shows as a dialog), then Submit with that
/// confirmation. Submit refuses a confirmation for an older draft, re-checks itslearning, uploads, and submits.
/// </summary>
public sealed partial class HandInViewModel : ObservableObject
{
    private readonly AssignmentInfo _assignment;
    private readonly AssignmentHandIn _handIn;
    private readonly TimeProvider _time;
    private int _version;

    internal HandInViewModel(AssignmentInfo assignment, AssignmentHandIn handIn, TimeProvider time)
    {
        _assignment = assignment;
        _handIn = handIn;
        _time = time;
        Title = string.Format(CultureInfo.CurrentCulture, Strings.HandInTitle, assignment.Details.Title);
        Files.CollectionChanged += (_, _) => Changed();
    }

    public string Title { get; }

    public ObservableCollection<HandInFileViewModel> Files { get; } = [];

    [ObservableProperty]
    public partial string Draft { get; set; } = "";

    partial void OnDraftChanged(string value) => Changed();

    /// <summary>"The deadline has passed: this hand-in will be late." (null otherwise).</summary>
    public string? WarningText => AssignmentSubmission.IsLate(_assignment.Details, _time.GetUtcNow()) ? Strings.HandInLate : null;

    [ObservableProperty]
    public partial string? ErrorMessage { get; private set; }

    /// <summary>Progress while handing in ("Uploading 2 of 3…").</summary>
    [ObservableProperty]
    public partial string? StatusText { get; private set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReviewCommand), nameof(AddFilesCommand))]
    public partial bool IsSubmitting { get; private set; }

    /// <summary>Raised by <see cref="ReviewCommand"/> with what to confirm; the platform asks and, on yes, runs <see cref="SubmitCommand"/> with it.</summary>
    public event EventHandler<HandInConfirmation>? ConfirmationRequested;

    /// <summary>Raised when the hand-in went through (the platform closes the form).</summary>
    public event EventHandler? Submitted;

    private void Changed()
    {
        _version++;
        ReviewCommand.NotifyCanExecuteChanged();
    }

    private bool CanEdit() => !IsSubmitting;

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private async Task AddFilesAsync()
    {
        AddFiles(await _handIn.Picker.PickFilesAsync(Strings.HandInPickTitle));
    }

    /// <summary>Adds files after checking them (readable, not already added, within itslearning's limits).</summary>
    internal void AddFiles(IReadOnlyList<string> paths)
    {
        ErrorMessage = null;
        foreach (var path in paths)
        {
            if (Files.Any(f => f.Path == path)) continue;
            var info = new FileInfo(path);
            if (!info.Exists) { ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.HandInFileMissing, info.Name); continue; }
            if (Files.Count >= AssignmentSubmission.MaxFiles)
            {
                ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.HandInTooManyFiles, AssignmentSubmission.MaxFiles);
                break;
            }
            if (info.Length > AssignmentSubmission.MaxFileBytes)
            {
                ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.HandInFileTooLarge, info.Name, DocumentExtractor.Size(info.Length),
                    DocumentExtractor.Size(AssignmentSubmission.MaxFileBytes));
                continue;
            }
            if (Files.Sum(f => f.Size) + info.Length > AssignmentSubmission.MaxTotalBytes)
            {
                ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.HandInTotalTooLarge, DocumentExtractor.Size(AssignmentSubmission.MaxTotalBytes));
                continue;
            }
            Files.Add(new HandInFileViewModel(path, info.Length, f => { if (!IsSubmitting) Files.Remove(f); }));
        }
    }

    private bool CanReview() => !IsSubmitting && (Draft.Trim().Length > 0 || Files.Count > 0);

    [RelayCommand(CanExecute = nameof(CanReview))]
    private void Review()
    {
        ErrorMessage = null;
        var lines = new List<string>
        {
            Draft.Trim().Length > 0
                ? string.Format(CultureInfo.CurrentCulture, Strings.HandInWords, Draft.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length)
                : Strings.HandInNoText,
        };
        lines.AddRange(Files.Select(f => string.Format(CultureInfo.CurrentCulture, Strings.HandInFileLine, f.Name, f.SizeText)));
        if (WarningText is { } warning) lines.Add(warning);
        lines.Add(Strings.HandInVisible);
        ConfirmationRequested?.Invoke(this, new HandInConfirmation(
            string.Format(CultureInfo.CurrentCulture, Strings.HandInConfirmTitle, _assignment.Details.Title),
            string.Join("\n", lines), Strings.HandInConfirm, Strings.HandInCancel, _version));
    }

    /// <summary>Hands in the draft the confirmation describes. While files upload, <c>SubmitCancelCommand</c> stops it (nothing is handed in).</summary>
    [RelayCommand(IncludeCancelCommand = true)]
    private async Task SubmitAsync(HandInConfirmation confirmation, CancellationToken ct)
    {
        if (confirmation.Version != _version || IsSubmitting)
        {
            ErrorMessage = Strings.HandInChangedLocally;
            return;
        }
        IsSubmitting = true;
        ErrorMessage = null;
        var files = Files.Select(f => f.Path).ToList();
        var uploadsDone = files.Count == 0;
        var progress = new Progress<int>(n =>
        {
            uploadsDone = n >= files.Count;
            StatusText = uploadsDone ? Strings.HandInSending : string.Format(CultureInfo.CurrentCulture, Strings.HandInUploading, n + 1, files.Count);
        });
        StatusText = files.Count > 0 ? string.Format(CultureInfo.CurrentCulture, Strings.HandInUploading, 1, files.Count) : Strings.HandInSending;
        try
        {
            await _handIn.SubmitAsync(_assignment.Details.ElementId, Draft, files, _time, progress, ct);
            Submitted?.Invoke(this, EventArgs.Empty);
        }
        catch (HandInStoppedException e)
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.HandInChangedOnServer, e.Message);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            ErrorMessage = Strings.HandInCancelled;
        }
        catch (ItslearningApiException e) when (e.StatusCode != System.Net.HttpStatusCode.Unauthorized && !(uploadsDone && (int)e.StatusCode >= 500))
        {
            ErrorMessage = string.Format(CultureInfo.CurrentCulture, Strings.HandInRefused, (int)e.StatusCode);
        }
        catch (Exception e) when (e is HttpRequestException or TaskCanceledException or ItslearningApiException or NotAuthenticatedException or IOException)
        {
            // A failure once the submit request was sent is ambiguous: it may have arrived. Look before saying anything.
            if (uploadsDone && await _handIn.AnswerExistsAsync(_assignment.Details.ElementId) == true)
                Submitted?.Invoke(this, EventArgs.Empty);
            else
                ErrorMessage = uploadsDone
                    ? string.Format(CultureInfo.CurrentCulture, Strings.HandInUnclear, Texts.Error(e))
                    : string.Format(CultureInfo.CurrentCulture, Strings.HandInRefused, Texts.Error(e));
        }
        finally
        {
            IsSubmitting = false;
            StatusText = null;
        }
    }
}

/// <summary>A file chosen for a hand-in.</summary>
public sealed partial class HandInFileViewModel
{
    private readonly Action<HandInFileViewModel> _remove;

    internal HandInFileViewModel(string path, long size, Action<HandInFileViewModel> remove)
    {
        Path = path;
        Size = size;
        _remove = remove;
    }

    public string Path { get; }
    public string Name => System.IO.Path.GetFileName(Path);
    public long Size { get; }
    public string SizeText => DocumentExtractor.Size(Size);

    [RelayCommand]
    private void Remove() => _remove(this);
}
