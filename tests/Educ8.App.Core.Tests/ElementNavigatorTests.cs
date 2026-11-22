using System.Net;
using Educ8.App.Core.Elements;
using Educ8.App.Core.Presentation;
using Educ8.Itslearning.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.App.Core.Tests;

/// <summary>How course elements open: the module registry, its browser fallback, and the rows built from it.</summary>
public class ElementNavigatorTests
{
    /// <summary>Every (element type, learning tool) pair seen live or documented, with how it opens today.</summary>
    public static TheoryData<string, int, OpenStyle> Known => new()
    {
        { "Folder", 0, OpenStyle.InApp },
        { "LearningToolElement", 5009, OpenStyle.LocalFile },   // file
        { "LearningToolElement", 5, OpenStyle.InApp },           // page
        { "LearningToolElement", 5010, OpenStyle.Browser },      // link
        { "LearningToolElement", 5006, OpenStyle.InApp },        // assignment
        { "LearningToolElement", 5002, OpenStyle.Browser },      // test: no API, stays in the browser
        { "LearningToolElement", 5050, OpenStyle.Browser },      // survey: no API, stays in the browser
        { "LearningToolElement", 13221, OpenStyle.Browser },     // external tool
        { "LearningToolElement", 5015, OpenStyle.Browser },      // unknown tool (seen live once)
        { "Discussion", 0, OpenStyle.Browser },                  // no API
        { "LearningPath", 0, OpenStyle.Browser },
        { "CustomActivity", 0, OpenStyle.Browser },
        { "SomethingNew", 0, OpenStyle.Browser },
    };

    [Theory]
    [MemberData(nameof(Known))]
    public void Each_kind_opens_the_expected_way(string elementType, int tool, OpenStyle expected)
    {
        using var h = new AppTestHost();
        var element = new ElementRef(1, "x", elementType, tool, 10, new Uri("https://school.itslearning.com/e/1"));
        Assert.Equal(expected, h.Elements.StyleOf(element));
        var row = h.Elements.Item(element);
        Assert.Equal(expected == OpenStyle.InApp ? AppIcon.OpenInApp : AppIcon.OpenExternal, row.OpenIcon);
    }

    [Fact]
    public void No_two_modules_claim_the_same_element()
    {
        using var h = new AppTestHost();
        var modules = h.Provider.GetServices<IElementModule>().ToList();
        Assert.NotEmpty(modules);
        foreach (var (type, tool, _) in Known.Select(row => ((string)row[0], (int)row[1], (OpenStyle)row[2])))
            Assert.True(modules.Count(m => m.Handles(new ElementRef(1, "x", type, tool, 10, null))) <= 1, $"{type}/{tool}");
    }

    [Fact]
    public async Task Without_a_module_the_browser_opens_and_without_a_link_the_row_is_inert()
    {
        using var h = new AppTestHost();
        var test = h.Elements.Item(new ElementRef(5, "Quiz", "LearningToolElement", 5002, 10, new Uri("https://school.itslearning.com/quiz")));
        Assert.True(test.CanOpen);
        await test.OpenCommand.ExecuteAsync(null);
        Assert.Equal("https://school.itslearning.com/quiz", Assert.Single(h.OpenUrl.Opened).ToString());
        Assert.Empty(h.Pages);

        Assert.False(h.Elements.Item(new ElementRef(6, "Odd", "SomethingNew", 0, 10, null)).CanOpen);
    }

    [Fact]
    public async Task A_folder_from_event_details_looks_up_its_course_and_opens_in_the_app()
    {
        using var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (req, _) => req.RequestUri!.AbsolutePath == "/restapi/personal/courses/resources/300/v1"
            ? FakeItslearning.Json("""{"ElementId":300,"ElementType":"Folder","LearningToolId":0,"Title":"Week 1","CourseId":10,"Active":true}""")
            : new HttpResponseMessage(HttpStatusCode.NotFound);

        await h.Elements.Item(new ElementRef(300, "Week 1", "Folder", 0, null, null)).OpenCommand.ExecuteAsync(null);

        var folder = Assert.IsType<FolderViewModel>(Assert.Single(h.Pages));
        Assert.Equal((10, (int?)300, "Week 1"), (folder.CourseId, folder.FolderId, folder.Title));
    }

    [Fact]
    public async Task A_failing_module_reports_it_and_opens_the_browser_instead()
    {
        using var h = new AppTestHost();
        h.T.SignedIn();
        h.T.Http.Respond = (_, _) => new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var folder = h.Elements.Item(new ElementRef(300, "Week 1", "Folder", 0, null, new Uri("https://school.itslearning.com/folder")));

        await folder.OpenCommand.ExecuteAsync(null); // the course lookup fails

        Assert.StartsWith("Couldn't open it here", folder.ErrorMessage);
        Assert.Equal("https://school.itslearning.com/folder", Assert.Single(h.OpenUrl.Opened).ToString());
        Assert.Empty(h.Pages);
    }

    [Fact]
    public async Task A_file_can_be_saved_where_the_user_chooses_and_reuses_the_download()
    {
        using var h = new AppTestHost();
        var file = h.Elements.Item(new ElementRef(72, "slides.pdf", "LearningToolElement", 5009, 10, null));
        var page = h.Elements.Item(new ElementRef(5, "Guides", "LearningToolElement", 5, 10, null));
        Assert.True(file.CanSaveCopy);
        Assert.False(page.CanSaveCopy);

        var downloads = Path.Combine(h.CacheDirectory, "Downloads");
        Directory.CreateDirectory(downloads);
        h.SaveLocation.Folder = downloads;
        await file.SaveCopyCommand.ExecuteAsync(null);

        Assert.Equal(["element-72.pdf"], h.SaveLocation.Suggested);
        Assert.Equal("%PDF-1.4", await File.ReadAllTextAsync(Path.Combine(downloads, "element-72.pdf")));
        Assert.Equal("Saved to Downloads", file.NoticeText);
        Assert.Empty(h.OpenFile.Opened); // saving doesn't open it

        await file.OpenCommand.ExecuteAsync(null); // opening afterwards uses the same download
        Assert.Equal([72], h.Files.Downloads);
        Assert.Null(file.NoticeText);

        h.SaveLocation.Folder = null; // the user cancels the dialog
        await file.SaveCopyCommand.ExecuteAsync(null);
        Assert.Null(file.NoticeText);
        Assert.Null(file.ErrorMessage);
    }
}
