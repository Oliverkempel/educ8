using System.IO.Compression;

namespace Educ8.Documents;

/// <summary>Entry point: detects the file type and extracts it into Markdown parts (+ images on request).</summary>
public static class DocumentExtractor
{
    /// <summary>Largest zip entry we unpack (bytes), to guard against zip bombs.</summary>
    public const long MaxZipEntryBytes = 50L * 1024 * 1024;

    private const int MaxZipListing = 500;

    public static ExtractedDocument Extract(string fileName, string? contentType, byte[] bytes, ExtractOptions? options = null) =>
        Extract(fileName, contentType, bytes, options ?? new ExtractOptions(), depth: 0);

    private static ExtractedDocument Extract(string fileName, string? contentType, byte[] bytes, ExtractOptions options, int depth)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        try
        {
            switch (ext)
            {
                case ".pdf":
                    return PdfExtractor.Extract(fileName, bytes, options);
                case ".docx" or ".docm" or ".dotx":
                    return WordExtractor.Extract(fileName, bytes, options);
                case ".xlsx" or ".xlsm" or ".xltx":
                    return ExcelExtractor.Extract(fileName, bytes, options);
                case ".pptx" or ".pptm" or ".ppsx" or ".potx":
                    return PowerPointExtractor.Extract(fileName, bytes, options);
                case ".zip":
                    return Zip(fileName, bytes, options, depth);
                case ".doc" or ".xls" or ".ppt":
                    return Unsupported(fileName, $"This is an old binary Office file ({ext}), which can't be read here. If the user can save it as {ext}x, that version can be read.");
                case ".rar" or ".7z" or ".tar" or ".gz" or ".tgz":
                    return Unsupported(fileName, "This archive format can't be opened here (only .zip).");
                case ".mp4" or ".mov" or ".avi" or ".mkv" or ".webm" or ".mp3" or ".wav" or ".m4a":
                    return Unsupported(fileName, "This is an audio/video file; its content can't be read here.");
            }

            if (ImageSniffer.MimeType(bytes) is { } mime) return Image(fileName, bytes, mime);
            if (bytes.AsSpan().StartsWith("%PDF"u8)) return PdfExtractor.Extract(fileName, bytes, options);
            if (bytes.AsSpan().StartsWith("PK\x03\x04"u8)) return OfficeOrZip(fileName, bytes, options, depth);
            if (TextExtractor.Handles(fileName) || contentType?.StartsWith("text/", StringComparison.OrdinalIgnoreCase) == true || TextExtractor.LooksLikeText(bytes))
                return TextExtractor.Extract(fileName, bytes);
            return Unsupported(fileName, $"Unrecognised file type{(ext.Length > 0 ? $" ({ext})" : "")}; its content can't be read here.");
        }
        catch (Exception e) when (e is InvalidDataException or IOException or DocumentFormat.OpenXml.Packaging.OpenXmlPackageException or FormatException)
        {
            return Unsupported(fileName, $"The file looks damaged or isn't really a {ext.TrimStart('.')} file ({e.GetType().Name}).");
        }
    }

    private static ExtractedDocument OfficeOrZip(string fileName, byte[] bytes, ExtractOptions options, int depth)
    {
        using var zip = new ZipArchive(new MemoryStream(bytes), ZipArchiveMode.Read);
        if (zip.GetEntry("word/document.xml") is not null) return WordExtractor.Extract(fileName, bytes, options);
        if (zip.GetEntry("xl/workbook.xml") is not null) return ExcelExtractor.Extract(fileName, bytes, options);
        if (zip.GetEntry("ppt/presentation.xml") is not null) return PowerPointExtractor.Extract(fileName, bytes, options);
        return Zip(fileName, bytes, options, depth);
    }

    private static ExtractedDocument Image(string fileName, byte[] bytes, string mime)
    {
        var dims = ImageSniffer.Dimensions(bytes);
        var description = dims is { } d ? $"{fileName} ({d.Width}×{d.Height})" : fileName;
        var notes = new List<string>();
        var images = new List<DocumentImage>();
        if (bytes.Length <= new ExtractOptions().MaxImageBytes) images.Add(new DocumentImage(mime, bytes, description));
        else notes.Add($"The image is too large to send ({bytes.Length / 1024} KB).");
        return new ExtractedDocument(fileName, DocumentKind.Image, [new DocumentPart(1, "Image", $"Image file {description}.", 1, images)], notes);
    }

    private static ExtractedDocument Zip(string fileName, byte[] bytes, ExtractOptions options, int depth)
    {
        using var zip = new ZipArchive(new MemoryStream(bytes), ZipArchiveMode.Read);
        var files = zip.Entries.Where(e => !e.FullName.EndsWith('/')).ToList();

        if (options.ZipEntry is { Length: > 0 } wanted)
        {
            var entry = files.FirstOrDefault(e => e.FullName == wanted)
                        ?? files.FirstOrDefault(e => e.FullName.Equals(wanted, StringComparison.OrdinalIgnoreCase))
                        ?? (files.Where(e => e.Name.Equals(wanted, StringComparison.OrdinalIgnoreCase)).ToList() is { Count: 1 } single ? single[0] : null);
            if (entry is null) return Unsupported(fileName, $"No file \"{wanted}\" in the archive. Read the archive without zipEntry to list its files.", DocumentKind.Archive);
            if (entry.Length > MaxZipEntryBytes) return Unsupported(fileName, $"\"{entry.FullName}\" is too large to unpack ({entry.Length / (1024 * 1024)} MB).", DocumentKind.Archive);
            if (depth >= 2) return Unsupported(fileName, "Archives nested this deep aren't opened.", DocumentKind.Archive);

            using var stream = entry.Open();
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            var inner = Extract(entry.Name, null, ms.ToArray(), new ExtractOptions
            {
                IncludeImages = options.IncludeImages, ImageParts = options.ImageParts, MaxImages = options.MaxImages,
                MinImageBytes = options.MinImageBytes, MaxImageBytes = options.MaxImageBytes,
            }, depth + 1);
            return inner with { FileName = $"{fileName} › {entry.FullName}" };
        }

        var rows = new List<List<string>> { new() { "File", "Size" } };
        rows.AddRange(files.Take(MaxZipListing).Select(e => new List<string> { e.FullName.Replace("|", "\\|"), Size(e.Length) }));
        var md = $"{files.Count} files.\n\n" + MarkdownTable.Render(rows);
        var notes = new List<string> { "To read a file inside the archive, call again with zipEntry set to its path." };
        if (files.Count > MaxZipListing) notes.Add($"Only the first {MaxZipListing} entries are listed.");
        return new ExtractedDocument(fileName, DocumentKind.Archive, [new DocumentPart(1, "Contents", md, 0, [])], notes);
    }

    private static ExtractedDocument Unsupported(string fileName, string why, DocumentKind kind = DocumentKind.Unsupported) =>
        new(fileName, kind, [], [why]);

    public static string Size(long bytes) => bytes switch
    {
        < 1024 => $"{bytes} B",
        < 1024 * 1024 => $"{bytes / 1024.0:0.#} KB",
        _ => $"{bytes / (1024.0 * 1024):0.#} MB",
    };
}
