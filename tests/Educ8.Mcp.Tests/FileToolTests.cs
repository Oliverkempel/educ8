using System.IO.Pipelines;
using System.Net;
using Educ8.Documents.Tests;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace Educ8.Mcp.Tests;

/// <summary>read_file / read_material on files through the MCP protocol: fake itslearning web chain serving generated documents.</summary>
public sealed class FileToolTests : IAsyncLifetime
{
    private readonly TestServices _t = new();
    private IHost _host = null!;
    private McpClient _client = null!;

    private const int PdfId = 9001, PngId = 9002, PptxId = 9003, PageId = 9004;

    private static readonly Dictionary<int, (string Name, string Type, byte[] Bytes, bool Office)> Files = new()
    {
        [PdfId] = ("slides.pdf", "application/pdf", TestFiles.Pdf(4, imagePages: [2]), false),
        [PngId] = ("diagram.png", "image/png", TestFiles.Png(200, 120), false),
        [PptxId] = ("lecture.pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", TestFiles.Pptx(), true),
    };

    private static string Resource(int id, int tool, string title) =>
        $$"""{"CourseTitle":"Course","ElementId":{{id}},"ElementType":"LearningToolElement","LearningToolId":{{tool}},"Title":"{{title}}","CourseId":1,"Active":true,"Path":" / Course / Week 1"}""";

    public async Task InitializeAsync()
    {
        _t.SignedIn();
        var lastElement = 0;
        _t.Http.Respond = (req, _) =>
        {
            var path = req.RequestUri!.AbsolutePath;
            if (path == "/restapi/personal/sso/url/v1")
            {
                lastElement = int.Parse(Uri.UnescapeDataString(req.RequestUri.Query).Split("LearningToolElementId=")[1].Split('&')[0]);
                return FakeItslearning.Json("""{"Url":"https://school.itslearning.com/sso-login"}""");
            }
            foreach (var (id, f) in Files)
                if (path == $"/restapi/personal/courses/resources/{id}/v1") return FakeItslearning.Json(Resource(id, 5009, f.Name));
            if (path == $"/restapi/personal/courses/resources/{PageId}/v1") return FakeItslearning.Json(Resource(PageId, 5, "A page"));
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };
        _t.Web.Respond = (req, _) =>
        {
            var u = req.RequestUri!;
            if (u.AbsolutePath == "/sso-login" || u.AbsolutePath.Contains("ViewLearningToolElement"))
                return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), "https://school.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx");
            if (u.Host == "platform.itslearning.com")
                return Files[lastElement].Office
                    ? Fixtures.Html(Fixtures.Read("file_office_2165906.html"), $"https://resource.itslearning.com/File/View.aspx?LearningObjectId={lastElement}&LearningObjectInstanceId=5")
                    : Fixtures.Html(Fixtures.Read("file_2371764.html"), $"https://resource.itslearning.com/File/View.aspx?LearningObjectId={lastElement}&LearningObjectInstanceId=5");
            if (u.AbsolutePath == "/Proxy/DownloadRedirect.ashx")
            {
                // The non-Office fixture always links to its own ids; serve whichever element was opened last.
                var f = Files[lastElement];
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(f.Bytes) };
                response.Content.Headers.ContentType = new(f.Type);
                response.Content.Headers.ContentDisposition = new("attachment") { FileName = $"\"{f.Name}\"" };
                return response;
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };

        var clientToServer = new Pipe();
        var serverToClient = new Pipe();
        var builder = Host.CreateEmptyApplicationBuilder(settings: null);
        foreach (var d in _t.Services) builder.Services.Add(d);
        builder.Services.AddMcpServer()
            .WithStreamServerTransport(clientToServer.Reader.AsStream(), serverToClient.Writer.AsStream())
            .AddEduc8Tools();
        _host = builder.Build();
        await _host.StartAsync();
        _client = await McpClient.CreateAsync(new StreamClientTransport(clientToServer.Writer.AsStream(), serverToClient.Reader.AsStream()));
    }

    public async Task DisposeAsync()
    {
        await _client.DisposeAsync();
        await _host.StopAsync();
        _host.Dispose();
        _t.Dispose();
    }

    private async Task<CallToolResult> Call(string tool, Dictionary<string, object?> args) => await _client.CallToolAsync(tool, args);

    private static string Text(CallToolResult r) => string.Join("\n", r.Content.OfType<TextContentBlock>().Select(c => c.Text));

    [Fact]
    public async Task Pdf_text_with_figure_hint()
    {
        var result = await Call("read_file", new() { ["elementId"] = PdfId });
        var text = Text(result);

        Assert.False(result.IsError == true, text);
        Assert.Contains("# slides.pdf", text);
        Assert.Contains("PDF ·", text);
        Assert.Contains("4 pages · element 9001 in Course (course 1) · location: Week 1", text);
        Assert.Contains("## Page 3\n\nUnique heading for page 3", text);
        Assert.Contains("Pages with figures, diagrams or pictures: 2.", text);
        Assert.Empty(result.Content.OfType<ImageContentBlock>());
    }

    [Fact]
    public async Task Pdf_pages_rendered_as_images_on_request()
    {
        var result = await Call("read_file", new() { ["elementId"] = PdfId, ["pages"] = "2", ["includeImages"] = true });
        var text = Text(result);

        Assert.Contains("## Page 2", text);
        Assert.DoesNotContain("## Page 3", text);
        Assert.Contains("Attached image: Page 2 (whole page)", text);
        Assert.Equal("image/png", Assert.Single(result.Content.OfType<ImageContentBlock>()).MimeType);
    }

    [Fact]
    public async Task Image_file_comes_back_as_image()
    {
        var result = await Call("read_file", new() { ["elementId"] = PngId });
        Assert.Contains("Image file diagram.png (200×120)", Text(result));
        Assert.Equal("image/png", Assert.Single(result.Content.OfType<ImageContentBlock>()).MimeType);
    }

    [Fact]
    public async Task Read_material_delegates_office_files_via_download_proxy()
    {
        var result = await Call("read_material", new() { ["elementId"] = PptxId });
        var text = Text(result);
        Assert.False(result.IsError == true, text);
        Assert.Contains("PowerPoint presentation", text);
        Assert.Contains("## Slide 1: Singleton pattern", text);
        Assert.Contains("Speaker notes: Mention thread safety", text);
        Assert.Contains("Slides with images: 1.", text);
    }

    [Fact]
    public async Task Errors_are_actionable()
    {
        var notFile = await Call("read_file", new() { ["elementId"] = PageId });
        Assert.True(notFile.IsError);
        Assert.Contains("is a Page, not a file. Use `read_material`", Text(notFile));

        var badRange = await Call("read_file", new() { ["elementId"] = PdfId, ["pages"] = "50" });
        Assert.True(badRange.IsError);
        Assert.Contains("valid numbers are 1–4", Text(badRange));
    }
}
