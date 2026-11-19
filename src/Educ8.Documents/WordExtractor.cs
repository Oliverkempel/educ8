using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;

namespace Educ8.Documents;

internal static class WordExtractor
{
    /// <summary>Split long documents into sections of roughly this many characters (at paragraph boundaries).</summary>
    private const int SectionChars = 8_000;

    public static ExtractedDocument Extract(string fileName, byte[] bytes, ExtractOptions options)
    {
        var notes = new List<string>();
        var parts = new List<DocumentPart>();
        using var doc = WordprocessingDocument.Open(new MemoryStream(bytes), false);
        var main = doc.MainDocumentPart;
        var body = main?.Document?.Body;
        if (main is null || body is null)
            return new ExtractedDocument(fileName, DocumentKind.Word, parts, ["The document has no body."]);

        var images = new ImageCollector(options);
        var sb = new StringBuilder();
        var sectionImages = new List<DocumentImage>();
        var imageCount = 0;
        var imageNumber = 0;

        void Flush()
        {
            var text = sb.ToString().Trim();
            if (text.Length == 0 && sectionImages.Count == 0 && imageCount == 0) return;
            var n = parts.Count + 1;
            parts.Add(new DocumentPart(n, $"Section {n}", text, imageCount, sectionImages.ToList()));
            sb.Clear();
            sectionImages.Clear();
            imageCount = 0;
        }

        void Block(OpenXmlElement element)
        {
            switch (element)
            {
                case Paragraph p:
                    var heading = HeadingLevel(p);
                    if (heading > 0 && sb.Length > SectionChars / 2) Flush(); // start sections at headings when large
                    var line = ParagraphText(p);
                    var blips = p.Descendants<A.Blip>().ToList();
                    foreach (var blip in blips)
                    {
                        imageCount++;
                        imageNumber++;
                        if (options.WantImagesFor(parts.Count + 1) && blip.Embed?.Value is { } rel && main.GetPartById(rel) is ImagePart part)
                            images.TryAdd(sectionImages, ReadAll(part), $"Image {imageNumber}");
                    }
                    if (line.Length == 0 && blips.Count > 0) line = "[Image]";
                    if (line.Length == 0) break;
                    if (heading > 0) sb.Append('\n').Append(new string('#', heading)).Append(' ').Append(line).Append("\n\n");
                    else if (p.ParagraphProperties?.NumberingProperties is { } num)
                        sb.Append(new string(' ', 2 * (num.NumberingLevelReference?.Val?.Value ?? 0))).Append("- ").Append(line).Append('\n');
                    else sb.Append(line).Append("\n\n");
                    break;
                case Table t:
                    sb.Append('\n').Append(TableMarkdown(t)).Append("\n\n");
                    break;
                case SdtBlock sdt:
                    foreach (var child in sdt.SdtContentBlock?.ChildElements ?? Enumerable.Empty<OpenXmlElement>()) Block(child);
                    break;
            }
            if (sb.Length > SectionChars) Flush();
        }

        foreach (var element in body.ChildElements) Block(element);
        Flush();

        if (parts.Count == 0) notes.Add("The document is empty.");
        notes.AddRange(images.Notes());
        return new ExtractedDocument(fileName, DocumentKind.Word, parts, notes);
    }

    private static int HeadingLevel(Paragraph p)
    {
        var style = p.ParagraphProperties?.ParagraphStyleId?.Val?.Value;
        if (style is null) return 0;
        if (style.Equals("Title", StringComparison.OrdinalIgnoreCase)) return 1;
        // English and localized ids: "Heading1", "Overskrift1", "berschrift1"...
        var digit = style.Length > 0 && char.IsDigit(style[^1]) ? style[^1] - '0' : 0;
        return digit is >= 1 and <= 6 && (style.StartsWith("Heading", StringComparison.OrdinalIgnoreCase) || style.StartsWith("Overskrift", StringComparison.OrdinalIgnoreCase) || style.Contains("berschrift", StringComparison.OrdinalIgnoreCase))
            ? Math.Min(digit + 1, 6)
            : 0;
    }

    private static string ParagraphText(OpenXmlElement p)
    {
        var sb = new StringBuilder();
        foreach (var node in p.Descendants())
        {
            switch (node)
            {
                case Text t: sb.Append(t.Text); break;
                case TabChar: sb.Append('\t'); break;
                case Break or CarriageReturn: sb.Append('\n'); break;
            }
        }
        return sb.ToString().Trim();
    }

    private static string TableMarkdown(Table table)
    {
        var rows = table.Elements<TableRow>()
            .Select(r => r.Elements<TableCell>()
                .Select(c => string.Join(' ', c.Elements<Paragraph>().Select(ParagraphText).Where(s => s.Length > 0)).Replace("|", "\\|").Replace("\n", " "))
                .ToList())
            .Where(r => r.Count > 0)
            .ToList();
        return MarkdownTable.Render(rows);
    }

    internal static byte[] ReadAll(OpenXmlPart part)
    {
        using var s = part.GetStream();
        using var ms = new MemoryStream();
        s.CopyTo(ms);
        return ms.ToArray();
    }
}

internal static class MarkdownTable
{
    /// <summary>Renders rows as a Markdown table; the first row is the header.</summary>
    public static string Render(List<List<string>> rows)
    {
        if (rows.Count == 0) return "";
        var width = rows.Max(r => r.Count);
        var sb = new StringBuilder();
        for (var i = 0; i < rows.Count; i++)
        {
            var cells = rows[i].Concat(Enumerable.Repeat("", width - rows[i].Count));
            sb.Append("| ").Append(string.Join(" | ", cells)).Append(" |\n");
            if (i == 0) sb.Append('|').Append(string.Concat(Enumerable.Repeat("---|", width))).Append('\n');
        }
        return sb.ToString().TrimEnd('\n');
    }
}
