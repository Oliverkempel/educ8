namespace Educ8.Documents;

/// <summary>Recognises image formats an LLM client can display, from the bytes.</summary>
public static class ImageSniffer
{
    public static string? MimeType(ReadOnlySpan<byte> data)
    {
        if (data.Length >= 8 && data[..8].SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })) return "image/png";
        if (data.Length >= 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF) return "image/jpeg";
        if (data.Length >= 6 && (data[..6].SequenceEqual("GIF87a"u8) || data[..6].SequenceEqual("GIF89a"u8))) return "image/gif";
        if (data.Length >= 12 && data[..4].SequenceEqual("RIFF"u8) && data[8..12].SequenceEqual("WEBP"u8)) return "image/webp";
        return null;
    }

    /// <summary>Width × height for PNG/JPEG/GIF if cheaply readable.</summary>
    public static (int Width, int Height)? Dimensions(ReadOnlySpan<byte> d)
    {
        switch (MimeType(d))
        {
            case "image/png" when d.Length >= 24:
                return (BigEndian(d[16..20]), BigEndian(d[20..24]));
            case "image/gif" when d.Length >= 10:
                return (d[6] | d[7] << 8, d[8] | d[9] << 8);
            case "image/jpeg":
                for (var i = 2; i + 9 < d.Length;)
                {
                    if (d[i] != 0xFF) { i++; continue; }
                    var marker = d[i + 1];
                    if (marker is >= 0xC0 and <= 0xCF and not 0xC4 and not 0xC8 and not 0xCC)
                        return ((d[i + 7] << 8) | d[i + 8], (d[i + 5] << 8) | d[i + 6]);
                    i += 2 + ((d[i + 2] << 8) | d[i + 3]);
                }
                return null;
            default:
                return null;
        }
    }

    private static int BigEndian(ReadOnlySpan<byte> b) => b[0] << 24 | b[1] << 16 | b[2] << 8 | b[3];
}

/// <summary>Collects images across a document while enforcing the size/count limits.</summary>
internal sealed class ImageCollector(ExtractOptions options)
{
    private int _collected;
    private readonly List<string> _tooLarge = [];
    private readonly List<string> _overLimit = [];

    public bool HasRoom => _collected < options.MaxImages;

    /// <summary>Adds the image to <paramref name="target"/> if it's displayable and within limits.</summary>
    public void TryAdd(List<DocumentImage> target, byte[] data, string description, int? minBytes = null)
    {
        var mime = ImageSniffer.MimeType(data);
        if (mime is null || data.Length < (minBytes ?? options.MinImageBytes)) return; // unsupported format or decoration
        if (data.Length > options.MaxImageBytes) { _tooLarge.Add($"{description} ({data.Length / 1024} KB)"); return; }
        if (_collected >= options.MaxImages) { _overLimit.Add(description); return; }
        var dims = ImageSniffer.Dimensions(data);
        target.Add(new DocumentImage(mime, data, dims is { } d ? $"{description} ({d.Width}×{d.Height})" : description));
        _collected++;
    }

    /// <summary>Summary notes about images that were left out.</summary>
    public IEnumerable<string> Notes()
    {
        if (_tooLarge.Count > 0) yield return $"Images too large to send: {string.Join("; ", _tooLarge)}.";
        if (_overLimit.Count > 0) yield return $"{_overLimit.Count} more image(s) not included (limit {options.MaxImages} per call); request a smaller pages range.";
    }
}
