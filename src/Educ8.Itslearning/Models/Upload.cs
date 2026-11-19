namespace Educ8.Itslearning.Models;

/// <summary>
/// A file in itslearning's temporary storage (<c>POST fileupload/temp/v1</c>). Single use; deleted after about 24 hours.
/// Pass it to e.g. an assignment answer.
/// </summary>
public sealed record UploadedTempFile
{
    public string FileRepoId { get; init; } = "";
    public string? SignedFileRepoId { get; init; }
    public string Name { get; init; } = "";
    public string? MimeType { get; init; }
    public int SourceType { get; init; }
    public bool IsTemplate { get; init; }
}

internal sealed record SubmitAnswerRequest(string? Text, IReadOnlyList<UploadedTempFile> Files);

internal sealed record SubmitAnswerResponse
{
    public int AnswerId { get; init; }
}
