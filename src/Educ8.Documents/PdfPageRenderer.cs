using PDFtoImage;

namespace Educ8.Documents;

/// <summary>
/// Renders whole PDF pages to images, so the model sees slides, diagrams and scanned pages as a person would
/// (extracting embedded images gives fragments). Uses native PDFium; if it can't load, rendering is reported unavailable.
/// </summary>
internal static class PdfPageRenderer
{
    private const int Dpi = 110;
    private const int MaxPngBytes = 2_000_000;
    private static bool _unavailable;

    /// <summary>PNG (or JPEG if the PNG is very large) of a 1-based page, or null if rendering isn't possible.</summary>
    public static (byte[] Data, string MimeType)? Render(byte[] pdf, int pageNumber)
    {
        if (_unavailable || !(OperatingSystem.IsWindows() || OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())) return null;
        try
        {
            var options = new RenderOptions(Dpi: Dpi, WithAnnotations: true, WithFormFill: true);
            using var png = new MemoryStream();
            Conversion.SavePng(png, pdf, page: pageNumber - 1, options: options);
            if (png.Length <= MaxPngBytes) return (png.ToArray(), "image/png");

            using var jpeg = new MemoryStream();
            Conversion.SaveJpeg(jpeg, pdf, page: pageNumber - 1, options: options);
            return (jpeg.ToArray(), "image/jpeg");
        }
        catch (Exception e) when (e is DllNotFoundException or TypeInitializationException or EntryPointNotFoundException or BadImageFormatException)
        {
            _unavailable = true; // native library missing on this platform
            return null;
        }
        catch (Exception)
        {
            return null; // this page failed to render
        }
    }
}
