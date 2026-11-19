using Educ8.Itslearning.Content;
using Educ8.Itslearning.Tests;
using Educ8.Documents;

namespace Educ8.Mcp.Tests;

public class HtmlToMarkdownTests
{
    [Theory]
    [InlineData("<p>Hello <strong>world</strong></p>", "Hello **world**")]
    [InlineData("<p>a&nbsp;&nbsp;b</p><p>&nbsp;</p><p>c</p>", "a b\n\nc")]
    [InlineData("<h2>Title</h2><p>x</p>", "## Title\n\nx")]
    [InlineData("<p><a href=\"https://x.dk/a\">Read <b>this</b></a>.</p>", "[Read **this**](https://x.dk/a).")]
    [InlineData("<a href=\"https://x.dk\">https://x.dk</a>", "<https://x.dk>")]
    [InlineData("<ul><li>one</li><li>two<ul><li>nested</li></ul></li></ul>", "- one\n- two\n  - nested")]
    [InlineData("<ol><li>first</li><li>second</li></ol>", "1. first\n2. second")]
    [InlineData("<oembed>https://www.youtube.com/watch?v=abc</oembed>", "[Embedded media: https://www.youtube.com/watch?v=abc]")]
    [InlineData("<img src=\"x.png\" alt=\"Diagram\">", "[Image: Diagram]")]
    [InlineData("<img src=\"x.png\"><b>Guides</b>", "[Image] **Guides**")]
    [InlineData("<strong><a href=\"https://x.dk\"><strong>personal data</strong>.</a></strong>", "**[personal data.](https://x.dk)**")]
    [InlineData("<p><b>Print<br>Other questions?</b> Contact</p>", "**Print**\n**Other questions?** Contact")]
    [InlineData("<em>a <i>b</i></em>", "*a b*")]
    [InlineData("line1<br>line2", "line1\nline2")]
    [InlineData("<pre><code>int x = 1;\nreturn x;</code></pre>", "```\nint x = 1;\nreturn x;\n```")]
    [InlineData("<p>use <code>ls -la</code></p>", "use `ls -la`")]
    [InlineData("<table><tr><th>A</th><th>B</th></tr><tr><td>1</td><td>2|3</td></tr></table>", "| A | B |\n|---|---|\n| 1 | 2\\|3 |")]
    [InlineData("<script>alert(1)</script><p>ok</p>", "ok")]
    [InlineData("", "")]
    public void Converts(string html, string expected) => Assert.Equal(expected, HtmlToMarkdown.Convert(html));

    [Fact]
    public void Converts_real_page_with_styled_spans_and_links()
    {
        var page = WebPageParsers.Page(Fixtures.Read("page_guides_2262797.html"));
        var md = HtmlToMarkdown.Convert(page.Blocks[0].Html);
        Assert.Contains("University collects and stores personal data about you in itslearning.", md);
        Assert.Contains("](https://portal.example.edu/en/itsl-persondata)", md);
        Assert.DoesNotContain("<", md.Replace("<https", ""));
        Assert.DoesNotContain("style", md);
    }

    [Fact]
    public void Truncates_at_line_break() =>
        Assert.Equal("aaaa …", HtmlToMarkdown.Truncate("aaaa\nbbbb\ncccc", 7));
}
