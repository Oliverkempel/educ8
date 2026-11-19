using System.ComponentModel;
using Educ8.Documents;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Educ8.Itslearning.Models;
using Educ8.Mcp.Formatting;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class FileTools
{
    [McpServerTool(Name = "read_file", Title = "Read a course file", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Downloads a course file and returns its content: PDF (per page), Word, PowerPoint (per slide, with speaker notes), " +
                 "Excel (per sheet), text/code files, Jupyter notebooks, and images (returned as images you can see). " +
                 "Zip archives are listed; pass zipEntry to read a file inside. Large files are returned in chunks: use `pages` to continue. " +
                 "Set includeImages=true to also get figures, diagrams and scanned pages as images (use a small page range).")]
    public static Task<CallToolResult> ReadFile(
        LoginService login, ItslearningClient client, LearningToolContentReader content,
        [Description("Element id of the file (from a listing, search or overview).")] int elementId,
        [Description("Which pages/slides/sheets/sections to read, e.g. \"1-10\", \"3\", \"12-\", \"2,5,7-9\". Default: from the start until the response is full.")] string? pages = null,
        [Description("Also return embedded images (figures, diagrams, scanned pages) for the selected pages. Images are large: keep the page range small.")] bool includeImages = false,
        [Description("For zip archives: path of the file inside the archive to read (as listed).")] string? zipEntry = null,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var item = await client.GetResourceAsync(elementId, ct);
            if (item.Kind != ResourceKind.File)
                throw new McpException($"Element {elementId} is a {ResourceFormatter.KindLabel(item.Kind, item.LearningToolId)}, not a file. Use `read_material` for it.");
            return await ReadAsync(content, item, pages, includeImages, zipEntry, ct);
        }, ct);

    /// <summary>Shared by <c>read_file</c> and <c>read_material</c> (which delegates file elements here).</summary>
    internal static async Task<CallToolResult> ReadAsync(
        LearningToolContentReader content, CourseResourceWithCourse item, string? pages, bool includeImages, string? zipEntry, CancellationToken ct)
    {
        var file = await content.DownloadFileAsync(item.ElementId, ct);
        return Render(file, FileFormatter.Context(item), pages, includeImages, zipEntry);
    }

    /// <summary>Extracts a downloaded file and formats the requested pages (+ images) as a tool result.</summary>
    internal static CallToolResult Render(DownloadedFile file, string context, string? pages, bool includeImages, string? zipEntry)
    {
        // Extract once without images to learn the part count, then resolve the range and collect images only for it.
        var probe = DocumentExtractor.Extract(file.FileName, file.ContentType, file.Content, new ExtractOptions { ZipEntry = zipEntry });
        var selected = Ranges.Parse(pages, Math.Max(probe.Parts.Count, 1));
        var doc = includeImages && probe.Parts.Sum(p => p.ImageCount) > 0
            ? DocumentExtractor.Extract(file.FileName, file.ContentType, file.Content,
                new ExtractOptions { ZipEntry = zipEntry, IncludeImages = true, ImageParts = selected ?? DefaultImageParts(probe) })
            : probe;
        return FileFormatter.Build(context, file.Content.Length, doc, selected, includeImages, zipEntry);
    }

    /// <summary>Without an explicit range, only collect images for the first few parts.</summary>
    private static SortedSet<int> DefaultImageParts(ExtractedDocument doc) => new(doc.Parts.Select(p => p.Number).Take(5));
}
