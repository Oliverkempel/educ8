namespace Educ8.Documents;

/// <summary>
/// Author HTML (itslearning pages, assignment descriptions, feedback) as a small structure every native UI can render with its own
/// text widgets: blocks of styled text runs. Made by <see cref="HtmlToRichText"/>. Styling beyond bold/italic/… (colours, fonts,
/// layout) is dropped; content a native UI can't show (video, embeds, complex tables) becomes <see cref="RichUnsupported"/>.
/// </summary>
public sealed record RichDocument(IReadOnlyList<RichBlock> Blocks)
{
    public static readonly RichDocument Empty = new([]);

    public bool IsEmpty => Blocks.Count == 0;
}

public abstract record RichBlock;

/// <summary>A paragraph. A "\n" inside a span is a line break.</summary>
public sealed record RichParagraph(IReadOnlyList<RichSpan> Spans) : RichBlock;

/// <summary>A heading, level 1–6.</summary>
public sealed record RichHeading(int Level, IReadOnlyList<RichSpan> Spans) : RichBlock;

public sealed record RichList(bool Ordered, IReadOnlyList<RichListItem> Items) : RichBlock;

/// <summary>One list item: its marker ("•", "3.") and content (paragraphs, nested lists, …).</summary>
public sealed record RichListItem(string Marker, IReadOnlyList<RichBlock> Blocks);

public sealed record RichQuote(IReadOnlyList<RichBlock> Blocks) : RichBlock;

/// <summary>Preformatted text (code), whitespace kept.</summary>
public sealed record RichCode(string Text) : RichBlock;

/// <summary>An image. <see cref="Source"/> may be a signed URL: never log it.</summary>
public sealed record RichImage(Uri? Source, string? Alt) : RichBlock;

/// <summary>A simple table: rows of cells, each cell a run of spans. Tables with merged cells or block content are <see cref="RichUnsupported"/>.</summary>
public sealed record RichTable(IReadOnlyList<IReadOnlyList<IReadOnlyList<RichSpan>>> Rows, bool HeaderRow) : RichBlock;

public sealed record RichRule : RichBlock;

/// <summary>Content a native UI doesn't show (a video, an embedded page, a complex table): a placeholder with a link where there is one.</summary>
public sealed record RichUnsupported(RichUnsupportedKind Kind, Uri? Link) : RichBlock;

public enum RichUnsupportedKind
{
    Video,
    Audio,
    Embed,
    Interactive,
    Table,
    Other,
}

[Flags]
public enum RichStyle
{
    None = 0,
    Bold = 1,
    Italic = 2,
    Underline = 4,
    Strikethrough = 8,
    Code = 16,
    Subscript = 32,
    Superscript = 64,
}

/// <summary>A run of text with one style, optionally a link (http, https or mailto only).</summary>
public sealed record RichSpan(string Text, RichStyle Style = RichStyle.None, Uri? Link = null);
