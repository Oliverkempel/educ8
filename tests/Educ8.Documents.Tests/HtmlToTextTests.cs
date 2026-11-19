namespace Educ8.Documents.Tests;

public class HtmlToTextTests
{
    [Theory]
    [InlineData("<p>Bring a <b>laptop</b></p>", "Bring a laptop")]
    [InlineData("<p>One</p><p>Two&nbsp;words</p>", "One\n\nTwo words")]
    [InlineData("<ul><li>First</li><li>Second</li></ul>", "• First\n• Second")]
    [InlineData("<a href=\"https://x.dk\">Read this</a>", "Read this")]
    [InlineData("line1<br>line2", "line1\nline2")]
    [InlineData("<oembed>https://youtu.be/x</oembed>", "https://youtu.be/x")]
    [InlineData("<script>x()</script>ok", "ok")]
    [InlineData(null, "")]
    public void Converts(string? html, string expected) => Assert.Equal(expected, HtmlToText.Convert(html));
}
