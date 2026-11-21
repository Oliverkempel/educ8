using System.Net;
using Educ8.App.Core.Elements;
using Educ8.Documents;
using Educ8.Itslearning.Tests;

namespace Educ8.App.Core.Tests;

/// <summary>The element modules on a fake itslearning: REST plus the SSO web chain serving the real (scrubbed) fixture pages.</summary>
public class ElementModuleTests
{
    private const string Site = "https://school.itslearning.com";

    /// <summary>Routes the web chain: SSO → element view → tool page from <paramref name="toolPage"/> (fixture name, final URL).</summary>
    private static AppTestHost Host(Func<(string Fixture, string FinalUrl)> toolPage, Func<HttpRequestMessage, HttpResponseMessage?>? api = null)
    {
        var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath == "/restapi/personal/sso/url/v1"
            ? FakeItslearning.Json($$"""{"Url":"{{Site}}/sso-login"}""")
            : api?.Invoke(req) ?? new HttpResponseMessage(HttpStatusCode.NotFound);
        h.T.Web.Respond = (req, _) =>
        {
            var u = req.RequestUri!;
            if (u.AbsolutePath is "/sso-login" || u.AbsolutePath.Contains("ViewLearningToolElement"))
                return Fixtures.Html(Fixtures.Read("view_page_2266446.html"), $"{Site}/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId=1");
            if (u.Host == "platform.itslearning.com")
            {
                var (fixture, finalUrl) = toolPage();
                return Fixtures.Html(fixture.EndsWith(".html") ? Fixtures.Read(fixture) : fixture, finalUrl);
            }
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        };
        return h;
    }

    private static async Task<T> OpenAsync<T>(AppTestHost h, ElementRef element) where T : ElementPageViewModel
    {
        var row = h.Elements.Item(element);
        Assert.True(row.OpensInApp);
        await row.OpenCommand.ExecuteAsync(null);
        Assert.Null(row.ErrorMessage);
        var page = Assert.IsType<T>(h.Pages.Last());
        await page.EnsureLoadedAsync();
        return page;
    }

    private static ElementRef PageElement(int id = 2262797) =>
        new(id, "Guides", "LearningToolElement", 5, 10, new Uri($"{Site}/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId={id}"));

    [Fact]
    public async Task A_page_opens_in_the_app_as_titled_rich_text_sections()
    {
        using var h = Host(() => ("page_guides_2262797.html", "https://page.itslearning.com/PageInstance.aspx?PageId=1"));
        var page = await OpenAsync<ContentPageViewModel>(h, PageElement());

        Assert.Equal(ElementPageState.Content, page.State);
        Assert.Equal(5, page.Sections.Count);
        Assert.All(page.Sections, s => Assert.True(s.IsSupported));
        Assert.Contains(page.Sections, s => s.Title == "Guides to itslearning and other technical support");
        var links = page.Sections.SelectMany(s => s.Content.Blocks).OfType<RichParagraph>().SelectMany(p => p.Spans).Where(s => s.Link is not null).ToList();
        Assert.NotEmpty(links);
        Assert.Contains(links, s => s.Style.HasFlag(RichStyle.Bold) && s.Text.Contains("personal data"));
    }

    [Fact]
    public async Task Videos_in_a_page_become_placeholders_with_their_link()
    {
        using var h = Host(() => ("page_video_2381176.html", "https://page.itslearning.com/PageInstance.aspx?PageId=2"));
        var page = await OpenAsync<ContentPageViewModel>(h, PageElement(2381176));

        var videos = page.Sections.SelectMany(s => s.Content.Blocks).OfType<RichUnsupported>().ToList();
        Assert.Equal(2, videos.Count);
        Assert.All(videos, v => Assert.Equal((RichUnsupportedKind.Video, "www.youtube.com"), (v.Kind, v.Link?.Host)));
    }

    [Fact]
    public async Task A_page_itslearning_changed_shows_the_problem_with_a_way_out()
    {
        using var h = Host(() => ("<html><body>Something else</body></html>", "https://page.itslearning.com/PageInstance.aspx?PageId=3"));
        var page = await OpenAsync<ContentPageViewModel>(h, PageElement(3));

        Assert.Equal(ElementPageState.Message, page.State);
        Assert.NotNull(page.MessageText);
        Assert.True(page.OpenInItslearningCommand.CanExecute(null));
        page.OpenInItslearningCommand.Execute(null);
        Assert.Equal(PageElement(3).Url, Assert.Single(h.OpenUrl.Opened));
    }

    [Fact]
    public async Task Links_in_a_page_open_elements_in_the_app_and_other_sites_in_the_browser()
    {
        using var h = Host(() => ("page_guides_2262797.html", "https://page.itslearning.com/PageInstance.aspx?PageId=1"),
            req => req.RequestUri!.AbsolutePath == "/restapi/personal/courses/resources/300/v1"
                ? FakeItslearning.Json("""{"ElementId":300,"ElementType":"Folder","LearningToolId":0,"Title":"Week 1","CourseId":10,"Active":true}""")
                : null);
        var page = await OpenAsync<ContentPageViewModel>(h, PageElement());

        await page.OpenLinkCommand.ExecuteAsync(new Uri($"{Site}/Folder/processfolder.aspx?FolderID=300"));
        Assert.Equal((10, (int?)300), (Assert.IsType<FolderViewModel>(h.Pages.Last()).CourseId, ((FolderViewModel)h.Pages.Last()).FolderId));

        await page.OpenLinkCommand.ExecuteAsync(new Uri("https://portal.example.edu/data"));
        Assert.Equal("https://portal.example.edu/data", Assert.Single(h.OpenUrl.Opened).ToString());
    }

    private static ElementRef LinkElement(int id = 2371739) =>
        new(id, "Linux kernel", "LearningToolElement", 5010, 10, new Uri($"{Site}/LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId={id}"));

    [Fact]
    public async Task A_link_without_description_opens_its_target_directly_and_is_read_once()
    {
        var reads = 0;
        using var h = Host(() => { reads++; return ("link_2371739.html", "https://resource.itslearning.com/Link/View.aspx"); });
        var row = h.Elements.Item(LinkElement());
        Assert.Equal((OpenStyle.Browser, Presentation.AppIcon.OpenExternal), (row.OpenStyle, row.OpenIcon));

        await row.OpenCommand.ExecuteAsync(null);
        await row.OpenCommand.ExecuteAsync(null);

        Assert.Equal(2, h.OpenUrl.Opened.Count);
        Assert.All(h.OpenUrl.Opened, u => Assert.Equal("https://digilent.com/blog/demystifiying-the-linux-kernel/", u.ToString()));
        Assert.Equal(1, reads);
        Assert.Empty(h.Pages);
        Assert.Null(row.ErrorMessage);
    }

    [Fact]
    public async Task A_link_with_a_description_shows_it_first()
    {
        var html = Fixtures.Read("link_2371739.html").Replace(
            "class=\"file-link-description\">", "class=\"file-link-description\"><p>Read <b>before</b> the lecture.</p>");
        using var h = Host(() => (html, "https://resource.itslearning.com/Link/View.aspx"));

        await h.Elements.Item(LinkElement()).OpenCommand.ExecuteAsync(null);

        var link = Assert.IsType<LinkViewModel>(Assert.Single(h.Pages));
        Assert.Empty(h.OpenUrl.Opened);
        Assert.Equal("digilent.com/blog/demystifiying-the-linux-kernel", link.TargetText);
        Assert.Equal("before", Assert.IsType<RichParagraph>(link.Description.Blocks.Single()).Spans[1].Text);
        Assert.Equal(ElementPageState.Content, link.State);
        link.OpenTargetCommand.Execute(null);
        Assert.Equal("https://digilent.com/blog/demystifiying-the-linux-kernel/", Assert.Single(h.OpenUrl.Opened).ToString());
    }

    [Fact]
    public async Task A_link_without_a_usable_target_opens_itslearning_and_says_why()
    {
        var html = Fixtures.Read("link_2371739.html").Replace("https://digilent.com/blog/demystifiying-the-linux-kernel/", "javascript:void(0)");
        using var h = Host(() => (html, "https://resource.itslearning.com/Link/View.aspx"));
        var row = h.Elements.Item(LinkElement());

        await row.OpenCommand.ExecuteAsync(null);

        Assert.Equal(LinkElement().Url, Assert.Single(h.OpenUrl.Opened));
        Assert.StartsWith("Couldn't open it here", row.ErrorMessage);
    }
}
