namespace Educ8.Documents;

public enum DocumentKind
{
    Unsupported,
    Pdf,
    Word,
    Excel,
    PowerPoint,
    Text,
    Image,
    Archive,
}

/// <summary>
/// A file turned into parts the caller can page through: PDF pages, slides, sheets, or sections of text.
/// Text is Markdown. Images are only collected when requested (they're large).
/// </summary>
public sealed record ExtractedDocument(
    string FileName,
    DocumentKind Kind,
    IReadOnlyList<DocumentPart> Parts,
    IReadOnlyList<string> Notes)
{
    /// <summary>What one part is called: "page", "slide", "sheet", "section", "entry".</summary>
    public string PartNoun => Kind switch
    {
        DocumentKind.Pdf => "page",
        DocumentKind.PowerPoint => "slide",
        DocumentKind.Excel => "sheet",
        DocumentKind.Archive => "listing",
        _ => "section",
    };

    public string KindLabel => Kind switch
    {
        DocumentKind.Pdf => "PDF",
        DocumentKind.Word => "Word document",
        DocumentKind.Excel => "Excel workbook",
        DocumentKind.PowerPoint => "PowerPoint presentation",
        DocumentKind.Text => "Text file",
        DocumentKind.Image => "Image",
        DocumentKind.Archive => "Zip archive",
        _ => "Unsupported file",
    };
}

/// <param name="Number">1-based position, used for page ranges.</param>
/// <param name="Label">e.g. "Page 3", "Slide 2: Agenda", "Sheet: Data", "Lines 1–200".</param>
/// <param name="ImageCount">Images found in this part (whether or not they were collected).</param>
public sealed record DocumentPart(int Number, string Label, string Markdown, int ImageCount, IReadOnlyList<DocumentImage> Images);

/// <param name="MimeType">image/png, image/jpeg, image/gif or image/webp.</param>
public sealed record DocumentImage(string MimeType, byte[] Data, string Description);

public sealed class ExtractOptions
{
    /// <summary>Collect images (PDF/Office embedded images). Image files always include their image.</summary>
    public bool IncludeImages { get; init; }

    /// <summary>Only collect images for these part numbers (null = all). Text is always extracted for all parts.</summary>
    public ISet<int>? ImageParts { get; init; }

    /// <summary>For a zip archive: the entry to read instead of listing the archive.</summary>
    public string? ZipEntry { get; init; }

    /// <summary>Images smaller than this (bytes) are skipped as icons/decoration.</summary>
    public int MinImageBytes { get; init; } = 3_000;

    /// <summary>Images larger than this (bytes) are skipped; MCP clients reject very large images.</summary>
    public int MaxImageBytes { get; init; } = 3_750_000;

    /// <summary>At most this many images are collected in total.</summary>
    public int MaxImages { get; init; } = 10;

    internal bool WantImagesFor(int part) => IncludeImages && (ImageParts is null || ImageParts.Contains(part));
}
