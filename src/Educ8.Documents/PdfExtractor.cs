using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
using UglyToad.PdfPig.Exceptions;

namespace Educ8.Documents;

internal static partial class PdfExtractor
{
    /// <summary>Text lines / images on at least this share of pages are template boilerplate (headers, footers, logos).</summary>
    private const double RepeatedShare = 0.5;

    private const int MinPagesForRepeatDetection = 4;

    public static ExtractedDocument Extract(string fileName, byte[] bytes, ExtractOptions options)
    {
        var notes = new List<string>();
        var parts = new List<DocumentPart>();
        var images = new ImageCollector(options);
        try
        {
            using var pdf = PdfDocument.Open(bytes, new ParsingOptions { UseLenientParsing = true, SkipMissingFonts = true });

            // Pass 1: text and image fingerprints per page.
            var pages = new List<(int Number, string Text, List<(string Hash, IPdfImage Image)> Images)>();
            foreach (var page in pdf.GetPages())
            {
                string text;
                try
                {
                    text = Clean(ContentOrderTextExtractor.GetText(page, addDoubleNewline: true));
                }
                catch (Exception)
                {
                    text = "";
                    notes.Add($"Page {page.Number}: text couldn't be extracted.");
                }

                var pageImages = new List<(string, IPdfImage)>();
                try
                {
                    foreach (var image in page.GetImages())
                    {
                        if (image.WidthInSamples < 48 && image.HeightInSamples < 48) continue; // icons, bullets
                        pageImages.Add((Convert.ToHexString(SHA1.HashData(image.RawMemory.Span)), image));
                    }
                }
                catch (Exception)
                {
                    // malformed image streams: keep the text
                }
                pages.Add((page.Number, text, pageImages));
            }

            // Template boilerplate: lines and images repeated on most pages.
            var repeatedLines = new HashSet<string>();
            var repeatedImages = new HashSet<string>();
            if (pages.Count >= MinPagesForRepeatDetection)
            {
                var threshold = Math.Max(3, (int)Math.Ceiling(pages.Count * RepeatedShare));
                repeatedLines = pages
                    .SelectMany(p => p.Text.Split('\n').Select(l => l.Trim()).Where(l => l.Length >= 12).Distinct())
                    .GroupBy(l => l).Where(g => g.Count() >= threshold).Select(g => g.Key).ToHashSet();
                repeatedImages = pages
                    .SelectMany(p => p.Images.Select(i => i.Hash).Distinct())
                    .GroupBy(h => h).Where(g => g.Count() >= threshold).Select(g => g.Key).ToHashSet();
                if (repeatedLines.Count > 0)
                    notes.Add($"Removed {repeatedLines.Count} line(s) repeated on most pages (template text, headers/footers).");
                if (repeatedImages.Count > 0)
                    notes.Add($"Ignored {repeatedImages.Count} image(s) repeated on most pages (logos, backgrounds).");
            }

            // Pass 2: build parts.
            var textless = new List<int>();
            foreach (var (number, rawText, pageImageList) in pages)
            {
                var text = repeatedLines.Count == 0
                    ? rawText
                    : Clean(string.Join('\n', rawText.Split('\n').Where(l => !repeatedLines.Contains(l.Trim()))));
                var content = pageImageList.Where(i => !repeatedImages.Contains(i.Hash)).ToList();
                var collected = new List<DocumentImage>();
                if (options.WantImagesFor(number) && images.HasRoom && PdfPageRenderer.Render(bytes, number) is { } rendered)
                {
                    images.TryAdd(collected, rendered.Data, $"Page {number} (whole page)", minBytes: 0);
                }
                else if (options.WantImagesFor(number))
                {
                    // Rendering unavailable: fall back to the page's embedded images.
                    var n = 0;
                    foreach (var (_, image) in content)
                    {
                        n++;
                        try
                        {
                            var data = image.TryGetPng(out var png) ? png : image.RawBytes.ToArray();
                            images.TryAdd(collected, data, $"Page {number}, image {n}");
                        }
                        catch (Exception)
                        {
                            // undecodable image
                        }
                    }
                }
                if (text.Length == 0) textless.Add(number);
                parts.Add(new DocumentPart(number, $"Page {number}", text, content.Count, collected));
            }

            if (textless.Count > 0)
                notes.Add(textless.Count == parts.Count
                    ? "No page has a text layer: this looks like a scanned PDF. Use includeImages=true to look at the pages as images."
                    : $"Pages without text (scanned or image-only): {Ranges.Format(textless)}. Use includeImages=true for those pages to look at them.");
        }
        catch (PdfDocumentEncryptedException)
        {
            notes.Add("The PDF is password-protected and can't be read.");
        }
        catch (Exception e) when (parts.Count == 0)
        {
            notes.Add($"The PDF couldn't be read ({e.GetType().Name}).");
        }
        notes.AddRange(images.Notes());
        return new ExtractedDocument(fileName, DocumentKind.Pdf, parts, notes);
    }

    /// <summary>Trims lines, drops repeated blank lines and stray control characters.</summary>
    internal static string Clean(string text)
    {
        // Typographic ligatures (ﬁ, ﬂ, …) break words for search and quoting.
        text = text.Replace("ﬀ", "ff").Replace("ﬁ", "fi").Replace("ﬂ", "fl").Replace("ﬃ", "ffi").Replace("ﬄ", "ffl").Replace("ﬅ", "st").Replace("ﬆ", "st");
        var lines = text.Replace("\r", "").Split('\n').Select(l => ControlChars().Replace(l, "").TrimEnd());
        return BlankLines().Replace(string.Join('\n', lines), "\n\n").Trim();
    }

    [GeneratedRegex(@"\n{3,}")]
    private static partial Regex BlankLines();

    [GeneratedRegex(@"[\x00-\x08\x0B\x0C\x0E-\x1F]")]
    private static partial Regex ControlChars();
}
