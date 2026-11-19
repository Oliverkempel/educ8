using System.Text;
using Educ8.Documents;
using Educ8.Itslearning.Models;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Formatting;

/// <summary>Turns an extracted document into an MCP tool result: Markdown text (within a size budget) plus image blocks.</summary>
internal static class FileFormatter
{
    /// <summary>Text budget per call; the model pages on with <c>pages=</c>.</summary>
    public const int TextBudget = 40_000;

    /// <summary>Total image bytes per call (MCP messages must stay reasonably small).</summary>
    public const long ImageBudgetBytes = 12L * 1024 * 1024;

    /// <summary>Where a course file element lives, for the header line.</summary>
    public static string Context(CourseResourceWithCourse item)
    {
        var path = ResourceFormatter.CleanPath(item.Path);
        return $"element {item.ElementId} in {item.CourseTitle} (course {item.CourseId})" + (path.Length > 0 ? $" · location: {path}" : "");
    }

    /// <param name="context">Where the file comes from, e.g. "element 123 in Course (course 1)" or "attached to assignment …".</param>
    public static CallToolResult Build(string context, int fileBytes, ExtractedDocument doc, SortedSet<int>? selected, bool includeImages, string? zipEntry)
    {
        var sb = new StringBuilder();
        sb.Append($"# {doc.FileName}\n\n");
        sb.Append($"{doc.KindLabel} · {DocumentExtractor.Size(fileBytes)}");
        if (doc.Parts.Count > 1 || doc.Kind is DocumentKind.Pdf or DocumentKind.PowerPoint or DocumentKind.Excel)
            sb.Append($" · {doc.Parts.Count} {doc.PartNoun}{(doc.Parts.Count == 1 ? "" : "s")}");
        sb.Append($" · {context}\n\n");

        var images = new List<ContentBlock>();
        long imageBytes = 0;
        var shown = new List<int>();
        int? stoppedAt = null;
        var skippedImages = 0;

        var candidates = doc.Parts.Where(p => selected is null || selected.Contains(p.Number)).ToList();
        foreach (var part in candidates)
        {
            var block = new StringBuilder();
            if (doc.Parts.Count > 1 || doc.Kind is DocumentKind.Pdf or DocumentKind.PowerPoint or DocumentKind.Excel)
                block.Append($"## {part.Label}\n\n");
            var text = part.Markdown.Length > 0 ? part.Markdown : "_(no text)_";
            if (part.ImageCount > 0 && doc.Kind != DocumentKind.Image)
                text += $"\n\n_[{part.ImageCount} image{(part.ImageCount == 1 ? "" : "s")} on this {doc.PartNoun}]_";
            block.Append(text).Append("\n\n");

            var remaining = TextBudget - sb.Length;
            if (block.Length > remaining)
            {
                if (shown.Count > 0) { stoppedAt = part.Number; break; }
                // A single huge part: show what fits.
                sb.Append(HtmlToMarkdown_Truncate(block.ToString(), Math.Max(remaining, 2_000)));
                sb.Append($"\n\n_This {doc.PartNoun} was cut short to keep the response small._\n\n");
            }
            else sb.Append(block);
            shown.Add(part.Number);

            foreach (var img in part.Images)
            {
                if (imageBytes + img.Data.Length > ImageBudgetBytes) { skippedImages++; continue; }
                images.Add(ImageContentBlock.FromBytes(img.Data, img.MimeType));
                imageBytes += img.Data.Length;
                sb.Append($"_Attached image: {img.Description}_\n\n");
            }
        }

        if (doc.Parts.Count == 0 && doc.Kind != DocumentKind.Image)
            sb.Append("_Nothing could be read from this file._\n\n");

        // Footer: navigation and hints.
        if (doc.Parts.Count > 1 && shown.Count > 0)
            sb.Append($"Shown: {doc.PartNoun}s {Ranges.Format(shown)} of {doc.Parts.Count}.");
        if (stoppedAt is { } next)
            sb.Append($" Stopped to keep the response small; continue with pages=\"{next}-\".");
        if (doc.Parts.Count > 1) sb.Append('\n');

        var figureParts = doc.Parts.Where(p => shown.Contains(p.Number) && p.ImageCount > 0).Select(p => p.Number).ToList();
        if (!includeImages && figureParts.Count > 0 && doc.Kind != DocumentKind.Image)
            sb.Append(doc.Kind == DocumentKind.Pdf
                ? $"\nPages with figures, diagrams or pictures: {Ranges.Format(figureParts)}. Text alone may miss what they show: to see pages exactly as they look, call again with includeImages=true and pages=\"…\" (up to 10 pages per call).\n"
                : $"\n{char.ToUpperInvariant(doc.PartNoun[0])}{doc.PartNoun[1..]}s with images: {Ranges.Format(figureParts)}. To see the images, call again with includeImages=true and pages=\"…\".\n");
        if (!includeImages && doc.Kind == DocumentKind.Pdf && shown.Count > 0)
            sb.Append("\nIf a page's text looks fragmented (diagrams drawn with shapes, tables, formulas), render that page with includeImages=true.\n");
        if (skippedImages > 0)
            sb.Append($"\n{skippedImages} image(s) left out to keep the response size down; request fewer pages.\n");

        foreach (var note in doc.Notes) sb.Append($"\nNote: {note}");
        if (zipEntry is null && doc.Kind == DocumentKind.Archive) sb.Append("\n");

        var content = new List<ContentBlock> { new TextContentBlock { Text = sb.ToString().TrimEnd() } };
        content.AddRange(images);
        return new CallToolResult { Content = content };
    }

    private static string HtmlToMarkdown_Truncate(string text, int max) => HtmlToMarkdown.Truncate(text, max);
}
