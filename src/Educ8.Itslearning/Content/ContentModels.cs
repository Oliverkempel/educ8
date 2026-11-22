namespace Educ8.Itslearning.Content;

/// <summary>A Page element's content. Each block's <see cref="PageBlock.Html"/> is the author's HTML, unchanged.</summary>
public sealed record PageContent(IReadOnlyList<PageBlock> Blocks);

/// <param name="Kind">Block type from the CSS class, e.g. "text", "files", "images", "rss", "poll".</param>
public sealed record PageBlock(string? Title, string Kind, string Html);

/// <summary>A Link element: the external URL plus an optional description (HTML).</summary>
public sealed record LinkContent(Uri? Url, string? DescriptionHtml);

/// <summary>The File tool page: where to download the file, its name and optional description (HTML).</summary>
public sealed record FileLink(Uri? DownloadUrl, string? FileName, string? DescriptionHtml);

/// <summary>
/// What the student sees on the assignment page: status, score, feedback. The REST API doesn't return these to students.
/// All parts are optional: unsubmitted assignments have no feedback; feedback can come without a score.
/// </summary>
/// <param name="Status">e.g. "Not submitted", "Completed", "Submitted".</param>
/// <param name="Score">Score badge text, e.g. "5 of 5 (100%)" or a grade label.</param>
/// <param name="AssessedBy">"Assessed on 29. marts 2026 12:00 by …" as shown.</param>
/// <param name="FeedbackHtml">The teacher's written feedback (HTML).</param>
/// <param name="SubmittedLine">"Submitted: 2. marts 2026 15:53 by …" as shown.</param>
public sealed record AssignmentView(string? Status, string? Score, string? AssessedBy, string? FeedbackHtml, string? SubmittedLine)
{
    public bool HasAssessment => Score is not null || FeedbackHtml is not null || AssessedBy is not null;
}

/// <summary>A downloaded file. <see cref="Content"/> is the whole file in memory (size-capped).</summary>
public sealed record DownloadedFile(string FileName, string? ContentType, byte[] Content);

/// <summary>The content of an element could not be read through the web view (layout changed, no access, unsupported tool...).</summary>
public sealed class ContentUnavailableException(string message) : Exception(message);

/// <summary>Downloads course files to disk (implemented by <see cref="LearningToolContentReader"/>).</summary>
public interface ICourseFileSource
{
    /// <summary>
    /// Saves File element <paramref name="elementId"/> into <paramref name="directory"/> and returns its path. Reports the share
    /// downloaded (0–1) to <paramref name="progress"/> when the size is known.
    /// </summary>
    Task<string> SaveFileAsync(int elementId, string directory, IProgress<double>? progress = null, CancellationToken ct = default);
}
