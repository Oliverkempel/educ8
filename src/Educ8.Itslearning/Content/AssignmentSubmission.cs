using System.Net;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;

namespace Educ8.Itslearning.Content;

/// <summary>Why an assignment can't take a hand-in right now (things itslearning would refuse anyway).</summary>
public enum SubmissionBlock
{
    None,

    /// <summary>The assignment is hidden or switched off.</summary>
    Inactive,

    /// <summary>Its active period is over.</summary>
    Closed,

    /// <summary>Its active period hasn't started.</summary>
    NotOpenYet,

    /// <summary>The deadline passed and late hand-ins aren't allowed.</summary>
    DeadlinePassed,
}

/// <summary>
/// Handing in an assignment: the rules, the text format and the upload-then-submit sequence. Shared by the MCP tool
/// (<c>submit_assignment</c>) and the desktop app, which each add their own confirmation step in front of it.
/// </summary>
public static class AssignmentSubmission
{
    public const int MaxFiles = 20;
    public const long MaxFileBytes = 100L * 1024 * 1024;
    public const long MaxTotalBytes = 250L * 1024 * 1024;

    public static SubmissionBlock Check(AssignmentDetails a, DateTimeOffset now)
    {
        if (!a.IsActive) return SubmissionBlock.Inactive;
        if (a.ActiveTo is { } to && to < now) return SubmissionBlock.Closed;
        if (a.ActiveFrom is { } from && from > now) return SubmissionBlock.NotOpenYet;
        if (a.Deadline is { } deadline && deadline < now && !a.IsSubmissionAfterDeadlineAllowed) return SubmissionBlock.DeadlinePassed;
        return SubmissionBlock.None;
    }

    /// <summary>True when handing in now is after the deadline (allowed, but marked late).</summary>
    public static bool IsLate(AssignmentDetails a, DateTimeOffset now) => a.Deadline is { } deadline && deadline < now;

    public static bool IsGroup(AssignmentDetails a) => string.Equals(a.SubmissionType, "Group", StringComparison.OrdinalIgnoreCase);

    /// <summary>Plain text → simple HTML (paragraphs on blank lines, line breaks kept, everything escaped).</summary>
    public static string? TextToHtml(string? text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var paragraphs = text.Replace("\r\n", "\n").Trim().Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(p => "<p>" + string.Join("<br />", p.Split('\n').Select(WebUtility.HtmlEncode)) + "</p>");
        return string.Concat(paragraphs);
    }

    private static readonly Dictionary<string, string> KnownTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        [".pdf"] = "application/pdf", [".zip"] = "application/zip", [".txt"] = "text/plain", [".md"] = "text/markdown",
        [".csv"] = "text/csv", [".json"] = "application/json", [".png"] = "image/png", [".jpg"] = "image/jpeg", [".jpeg"] = "image/jpeg",
        [".gif"] = "image/gif", [".docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        [".xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        [".pptx"] = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
    };

    public static string MimeType(string fileName) => KnownTypes.GetValueOrDefault(Path.GetExtension(fileName), "application/octet-stream");

    /// <summary>
    /// Uploads the files to itslearning's temporary storage, then submits the answer. Cancelling only works during the uploads:
    /// the submit request itself is never cut off half-way. Returns the answer id. Callers check the rules first.
    /// </summary>
    /// <param name="uploaded">Reports how many files are uploaded so far.</param>
    public static async Task<int> SubmitAsync(this ItslearningClient client, int elementId, string? text, IReadOnlyList<string> filePaths,
        IProgress<int>? uploaded = null, CancellationToken uploadCt = default)
    {
        var files = new List<UploadedTempFile>();
        foreach (var path in filePaths)
        {
            uploadCt.ThrowIfCancellationRequested();
            await using var stream = File.OpenRead(path);
            var name = Path.GetFileName(path);
            files.Add(await client.UploadTempFileAsync(name, stream, MimeType(name), uploadCt).ConfigureAwait(false));
            uploaded?.Report(files.Count);
        }
        uploadCt.ThrowIfCancellationRequested();
        return await client.SubmitAnswerAsync(elementId, TextToHtml(text), files, CancellationToken.None).ConfigureAwait(false);
    }
}
