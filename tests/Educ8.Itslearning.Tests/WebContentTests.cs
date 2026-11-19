using System.Net;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.Itslearning.Tests;

public class WebPageParserTests
{
    [Fact]
    public void Finds_signed_tool_iframe_on_view_page()
    {
        var url = WebPageParsers.ExtensionIframeUrl(Fixtures.Read("view_page_2266446.html"), new Uri("https://sdu.itslearning.com/LearningToolElement/ViewLearningToolElement.aspx"));
        Assert.NotNull(url);
        Assert.Equal("platform.itslearning.com", url.Host);
        Assert.Equal("/LoginSSO.aspx", url.AbsolutePath);
        Assert.Contains("ElementId=2266446", url.Query);
        Assert.DoesNotContain("&amp;", url.ToString());
    }

    [Fact]
    public void Parses_page_blocks()
    {
        var page = WebPageParsers.Page(Fixtures.Read("page_guides_2262797.html"));
        Assert.Equal(5, page.Blocks.Count);
        Assert.All(page.Blocks, b => Assert.Equal("text", b.Kind));
        Assert.Equal("Personal data in itslearning", page.Blocks[0].Title);
        Assert.Contains("https://mitsdu.dk/en/itsl-persondata", page.Blocks[0].Html);
    }

    [Fact]
    public void Keeps_embedded_media_in_page_html()
    {
        var page = WebPageParsers.Page(Fixtures.Read("page_video_2381176.html"));
        var block = Assert.Single(page.Blocks);
        Assert.StartsWith("Supplementary videos Topic 2", block.Title);
        Assert.Contains("<oembed>https://www.youtube.com/watch?v=8wVq5aGzSqY</oembed>", block.Html);
    }

    [Fact]
    public void Untitled_block_has_null_title()
    {
        var page = WebPageParsers.Page(Fixtures.Read("page_coursedesc_2266446.html"));
        Assert.Null(Assert.Single(page.Blocks).Title);
    }

    [Fact]
    public void Finds_file_download_link_and_name()
    {
        var link = WebPageParsers.File(Fixtures.Read("file_2371764.html"), new Uri("https://resource.itslearning.com/File/View.aspx?x=1"));
        Assert.Equal("OS&Linux.pdf", link.FileName);
        Assert.Equal("https://resource.itslearning.com/Proxy/DownloadRedirect.ashx?LearningObjectId=486294690&LearningObjectInstanceId=645330988", link.DownloadUrl!.ToString());
    }

    [Fact]
    public void Office_file_page_without_link_falls_back_to_download_proxy()
    {
        // Office files only offer "Open in PowerPoint for the web"; the proxy still serves them by learning object ids.
        var toolUrl = new Uri("https://resource.itslearning.com/File/View.aspx?mode=FileOnly&LearningObjectId=466315359&LearningObjectInstanceId=623435343&Signature=SIG");
        var link = WebPageParsers.File(Fixtures.Read("file_office_2165906.html"), toolUrl);
        Assert.Equal("https://resource.itslearning.com/Proxy/DownloadRedirect.ashx?LearningObjectId=466315359&LearningObjectInstanceId=623435343", link.DownloadUrl!.ToString());
    }

    [Fact]
    public void Parses_link_target()
    {
        var link = WebPageParsers.Link(Fixtures.Read("link_2371739.html"));
        Assert.Equal(new Uri("https://digilent.com/blog/demystifiying-the-linux-kernel/"), link.Url);
    }

    [Theory]
    [InlineData("LearningToolElement", 5, ResourceKind.Page)]
    [InlineData("LearningToolElement", 5009, ResourceKind.File)]
    [InlineData("LearningToolElement", 5010, ResourceKind.Link)]
    [InlineData("LearningToolElement", 5006, ResourceKind.Assignment)]
    [InlineData("LearningToolElement", 13221, ResourceKind.Other)]
    [InlineData("Folder", 0, ResourceKind.Folder)]
    [InlineData("CustomActivity", 0, ResourceKind.Activity)]
    public void Maps_element_kinds(string type, int tool, ResourceKind expected) =>
        Assert.Equal(expected, LearningTools.KindOf(type, tool));
}

public class LearningToolContentReaderTests
{
    private const string ViewPath = "/LearningToolElement/ViewLearningToolElement.aspx";

    private static void RouteApi(TestServices t, List<string> ssoCalls) =>
        t.Http.Respond = (req, _) =>
        {
            if (req.RequestUri!.AbsolutePath == "/restapi/personal/sso/url/v1")
            {
                ssoCalls.Add(req.RequestUri.Query);
                return FakeItslearning.Json("""{"Url":"https://sdu.itslearning.com/sso-login?ticket=t"}""");
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };

    private static Func<HttpRequestMessage, string?, HttpResponseMessage> Web(Func<bool> sessionValid) => (req, _) =>
    {
        var u = req.RequestUri!;
        return (u.Host, u.AbsolutePath) switch
        {
            // SSO link: signs in and redirects to the element view.
            ("sdu.itslearning.com", "/sso-login") => Fixtures.Html(Fixtures.Read("view_page_2266446.html"), "https://sdu.itslearning.com" + ViewPath + "?LearningToolElementId=2266446"),
            ("sdu.itslearning.com", ViewPath) => sessionValid()
                ? Fixtures.Html(Fixtures.Read("view_page_2266446.html"), u.ToString())
                : Fixtures.Html("<html>logged out</html>", "https://sdu.itslearning.com/log_out.aspx?SessionExpired=0"),
            ("platform.itslearning.com", "/LoginSSO.aspx") => Fixtures.Html(Fixtures.Read("page_coursedesc_2266446.html"), "https://page.itslearning.com/PageInstance.aspx?PageId=1"),
            _ => new HttpResponseMessage(HttpStatusCode.NotFound),
        };
    };

    [Fact]
    public async Task Reads_page_via_sso_then_reuses_web_session()
    {
        using var t = new TestServices();
        t.SignedIn();
        var ssoCalls = new List<string>();
        RouteApi(t, ssoCalls);
        t.Web.Respond = Web(() => true);
        using var sp = t.Build();
        var reader = sp.GetRequiredService<LearningToolContentReader>();

        var page = await reader.ReadPageAsync(2266446);
        Assert.Contains("odin.sdu.dk", Assert.Single(page.Blocks).Html);
        Assert.Single(ssoCalls);
        Assert.Contains(Uri.EscapeDataString("LearningToolElementId=2266446"), ssoCalls[0]);

        await reader.ReadPageAsync(2266446);
        Assert.Single(ssoCalls); // second read used the existing web session
    }

    [Fact]
    public async Task Re_establishes_expired_web_session()
    {
        using var t = new TestServices();
        t.SignedIn();
        var ssoCalls = new List<string>();
        RouteApi(t, ssoCalls);
        var valid = true;
        t.Web.Respond = Web(() => valid);
        using var sp = t.Build();
        var reader = sp.GetRequiredService<LearningToolContentReader>();

        await reader.ReadPageAsync(2266446);
        valid = false; // itslearning web session expired
        var page = await reader.ReadPageAsync(2266446);

        Assert.Single(page.Blocks);
        Assert.Equal(2, ssoCalls.Count);
    }

    [Fact]
    public async Task Downloads_file_once_then_serves_it_from_cache()
    {
        using var t = new TestServices();
        t.SignedIn();
        RouteApi(t, []);
        var downloads = 0;
        t.Web.Respond = (req, _) =>
        {
            var u = req.RequestUri!;
            if (u.AbsolutePath == "/sso-login" || u.AbsolutePath.Contains("ViewLearningToolElement"))
                return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), "https://sdu.itslearning.com" + ViewPath);
            if (u.Host == "platform.itslearning.com")
                return Fixtures.Html(Fixtures.Read("file_2371764.html"), "https://resource.itslearning.com/File/View.aspx?mode=FileOnly");
            if (u.AbsolutePath == "/Proxy/DownloadRedirect.ashx")
            {
                downloads++;
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent("%PDF-1.4 test"u8.ToArray()) };
                response.Content.Headers.ContentType = new("application/pdf");
                response.Content.Headers.ContentDisposition = new("attachment") { FileName = "\"OS&Linux.pdf\"" };
                return response;
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };
        using var sp = t.Build();
        var reader = sp.GetRequiredService<LearningToolContentReader>();

        var file = await reader.DownloadFileAsync(2371764);
        Assert.Equal("OS&Linux.pdf", file.FileName);
        Assert.Equal("application/pdf", file.ContentType);
        Assert.Equal("%PDF-1.4 test", System.Text.Encoding.ASCII.GetString(file.Content));

        await reader.DownloadFileAsync(2371764);
        Assert.Equal(1, downloads);
    }

    [Fact]
    public async Task Unrecognised_view_is_reported()
    {
        using var t = new TestServices();
        t.SignedIn();
        RouteApi(t, []);
        t.Web.Respond = (_, _) => Fixtures.Html("<html>no access</html>", "https://sdu.itslearning.com/Index.aspx");
        using var sp = t.Build();

        await Assert.ThrowsAsync<ContentUnavailableException>(() => sp.GetRequiredService<LearningToolContentReader>().ReadPageAsync(1));
    }
}
