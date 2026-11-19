using System.Text;
using DocumentFormat.OpenXml.Packaging;
using A = DocumentFormat.OpenXml.Drawing;
using P = DocumentFormat.OpenXml.Presentation;

namespace Educ8.Documents;

internal static class PowerPointExtractor
{
    public static ExtractedDocument Extract(string fileName, byte[] bytes, ExtractOptions options)
    {
        var notes = new List<string>();
        var parts = new List<DocumentPart>();
        using var doc = PresentationDocument.Open(new MemoryStream(bytes), false);
        var pres = doc.PresentationPart;
        var slideIds = pres?.Presentation?.SlideIdList?.Elements<P.SlideId>().ToList() ?? [];
        var images = new ImageCollector(options);

        foreach (var slideId in slideIds)
        {
            var n = parts.Count + 1;
            if (slideId.RelationshipId?.Value is not { } rel || pres!.GetPartById(rel) is not SlidePart slidePart)
            {
                parts.Add(new DocumentPart(n, $"Slide {n}", "_(unreadable slide)_", 0, []));
                continue;
            }

            var sb = new StringBuilder();
            string? title = null;
            var tree = slidePart.Slide?.CommonSlideData?.ShapeTree;
            if (tree is not null)
            {
                foreach (var shape in tree.Descendants<P.Shape>())
                {
                    var placeholder = shape.NonVisualShapeProperties?.ApplicationNonVisualDrawingProperties?.GetFirstChild<P.PlaceholderShape>();
                    var isTitle = placeholder?.Type?.Value is { } t && (t == P.PlaceholderValues.Title || t == P.PlaceholderValues.CenteredTitle);
                    if (placeholder?.Type?.Value is { } pt && (pt == P.PlaceholderValues.SlideNumber || pt == P.PlaceholderValues.Footer || pt == P.PlaceholderValues.DateAndTime)) continue;
                    var paragraphs = shape.TextBody?.Elements<A.Paragraph>().ToList() ?? [];
                    if (isTitle)
                    {
                        title = string.Join(' ', paragraphs.Select(Text).Where(s => s.Length > 0));
                        continue;
                    }
                    foreach (var p in paragraphs)
                    {
                        var text = Text(p);
                        if (text.Length == 0) continue;
                        var level = p.ParagraphProperties?.Level?.Value ?? 0;
                        sb.Append(new string(' ', 2 * level)).Append("- ").Append(text).Append('\n');
                    }
                    sb.Append('\n');
                }
                foreach (var table in tree.Descendants<A.Table>())
                {
                    var rows = table.Elements<A.TableRow>()
                        .Select(r => r.Elements<A.TableCell>().Select(c => string.Join(' ', c.Descendants<A.Paragraph>().Select(Text).Where(s => s.Length > 0)).Replace("|", "\\|")).ToList())
                        .ToList();
                    sb.Append(MarkdownTable.Render(rows)).Append("\n\n");
                }
            }

            var slideImages = new List<DocumentImage>();
            var blips = slidePart.Slide?.Descendants<A.Blip>().ToList() ?? [];
            var k = 0;
            foreach (var blip in blips)
            {
                k++;
                if (options.WantImagesFor(n) && blip.Embed?.Value is { } embed && slidePart.GetPartById(embed) is ImagePart img)
                    images.TryAdd(slideImages, WordExtractor.ReadAll(img), $"Slide {n}, image {k}");
            }

            var notesText = slidePart.NotesSlidePart?.NotesSlide?.Descendants<P.Shape>()
                .Where(s => s.NonVisualShapeProperties?.ApplicationNonVisualDrawingProperties?.GetFirstChild<P.PlaceholderShape>()?.Type?.Value is not { } t
                            || t == P.PlaceholderValues.Body)
                .SelectMany(s => s.TextBody?.Elements<A.Paragraph>() ?? [])
                .Select(Text).Where(s => s.Length > 0).ToList() ?? [];
            if (notesText.Count > 0) sb.Append("Speaker notes: ").Append(string.Join(' ', notesText)).Append('\n');

            var hidden = slidePart.Slide?.Show?.Value == false ? " (hidden)" : "";
            var label = string.IsNullOrWhiteSpace(title) ? $"Slide {n}{hidden}" : $"Slide {n}{hidden}: {title}";
            var body = sb.ToString().Trim();
            if (body.Length == 0 && blips.Count > 0) body = "_(images only)_";
            parts.Add(new DocumentPart(n, label, body, blips.Count, slideImages));
        }

        if (parts.Count == 0) notes.Add("The presentation has no slides.");
        notes.AddRange(images.Notes());
        return new ExtractedDocument(fileName, DocumentKind.PowerPoint, parts, notes);
    }

    private static string Text(A.Paragraph p) =>
        string.Concat(p.Elements().Select(e => e switch
        {
            A.Run r => r.Text?.Text ?? "",
            A.Field f => f.Text?.Text ?? "",
            A.Break => " ",
            _ => "",
        })).Trim();
}
