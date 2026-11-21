using AngleSharp.Html.Parser;

namespace Educ8.Itslearning.Content;

/// <summary>
/// Parses itslearning web pages (undocumented markup, observed 2026-11-18). Everything that depends on the
/// web UI's HTML lives here so a layout change is fixed in one place. Tested against saved pages in
/// tests/Educ8.Itslearning.Tests/Fixtures.
/// </summary>
public static class WebPageParsers
{
    private static readonly HtmlParser Parser = new();

    /// <summary>
    /// From <c>ViewLearningToolElement.aspx</c>: the signed iframe URL of the tool showing the element
    /// (<c>iframe#ctl00_ContentPlaceHolder_ExtensionIframe</c>). Null if absent.
    /// </summary>
    public static Uri? ExtensionIframeUrl(string viewPageHtml, Uri baseUri)
    {
        using var doc = Parser.ParseDocument(viewPageHtml);
        var src = doc.QuerySelector("iframe[id$='ExtensionIframe']")?.GetAttribute("src");
        return string.IsNullOrWhiteSpace(src) ? null : new Uri(baseUri, src);
    }

    /// <summary>From <c>page.itslearning.com/PageInstance.aspx</c>: the content blocks (<c>li.il-widget</c>).</summary>
    public static PageContent Page(string pageHtml)
    {
        using var doc = Parser.ParseDocument(pageHtml);
        var blocks = new List<PageBlock>();
        foreach (var widget in doc.QuerySelectorAll("li.il-widget"))
        {
            var title = widget.QuerySelector(".il-widget-titleBlock h2, .il-widget-header h2")?.TextContent.Trim();
            var kind = widget.ClassList.FirstOrDefault(c => c.StartsWith("ilw-cb-", StringComparison.Ordinal))?["ilw-cb-".Length..] ?? "unknown";
            var content = widget.QuerySelector(".il-widget-content .h-userinput") ?? widget.QuerySelector(".il-widget-content");
            blocks.Add(new PageBlock(string.IsNullOrWhiteSpace(title) ? null : title, kind, content?.InnerHtml.Trim() ?? ""));
        }
        return new PageContent(blocks);
    }

    /// <summary>From <c>resource.itslearning.com/Link/View.aspx</c>: the target URL and description.</summary>
    public static LinkContent Link(string linkHtml)
    {
        using var doc = Parser.ParseDocument(linkHtml);
        var href = doc.QuerySelector("a[id$='ResourceContent_Link']")?.GetAttribute("href");
        var description = doc.QuerySelector("section.file-link-description")?.InnerHtml.Trim();
        return new LinkContent(
            Uri.TryCreate(href, UriKind.Absolute, out var url) ? url : null,
            string.IsNullOrWhiteSpace(description) ? null : description);
    }

    /// <summary>
    /// From <c>resource.itslearning.com/File/View.aspx</c>: the download link
    /// (<c>a#…DownloadLinkForViewType</c> → <c>/Proxy/DownloadRedirect.ashx?…</c>), file name and description.
    /// </summary>
    public static FileLink File(string fileHtml, Uri baseUri)
    {
        using var doc = Parser.ParseDocument(fileHtml);
        var anchor = doc.QuerySelector("a[id$='DownloadLinkForViewType']") ?? doc.QuerySelector("a[href*='DownloadRedirect.ashx']");
        var href = anchor?.GetAttribute("href");
        var name = anchor?.GetAttribute("download");
        var description = doc.QuerySelector("section.file-link-description")?.InnerHtml.Trim();

        // Office files show "Open in … for the web" instead of a download link, but the same download proxy
        // serves them; it only needs the learning object ids, which are in the tool page's URL.
        if (string.IsNullOrWhiteSpace(href)
            && QueryValue(baseUri, "LearningObjectId") is { } lo && QueryValue(baseUri, "LearningObjectInstanceId") is { } loi)
            href = $"/Proxy/DownloadRedirect.ashx?LearningObjectId={lo}&LearningObjectInstanceId={loi}";

        return new FileLink(
            string.IsNullOrWhiteSpace(href) ? null : new Uri(baseUri, href),
            string.IsNullOrWhiteSpace(name) ? null : name.Trim(),
            string.IsNullOrWhiteSpace(description) ? null : description);
    }

    /// <summary>
    /// From <c>assignment.itslearning.com/view.aspx</c> (the student's view of an assignment): status
    /// (<c>.as-submission-status-title</c>), assessment section (<c>#…AssessmentOrFeedbackSectionContent</c>: score badge
    /// <c>.ccl-assess-badge</c>, "Assessed on … by …" line, feedback in <c>.h-userinput</c>) and the "Submitted: …" line.
    /// </summary>
    public static AssignmentView Assignment(string html)
    {
        using var doc = Parser.ParseDocument(html);
        static string? Text(AngleSharp.Dom.IElement? e) =>
            e is null ? null : System.Text.RegularExpressions.Regex.Replace(e.TextContent, @"\s+", " ").Trim() is { Length: > 0 } t ? t : null;

        var status = Text(doc.QuerySelector(".as-submission-status-title"));
        var section = doc.QuerySelector("[id$='AssessmentOrFeedbackSectionContent']");
        string? score = null, assessed = null, feedback = null;
        if (section is not null)
        {
            score = Text(section.QuerySelector(".ccl-assess-badge"));
            assessed = section.QuerySelectorAll("div").Select(Text).FirstOrDefault(t => t?.StartsWith("Assessed", StringComparison.OrdinalIgnoreCase) == true);
            var fb = section.QuerySelector(".h-userinput")?.InnerHtml.Trim();
            feedback = string.IsNullOrWhiteSpace(fb) ? null : fb;
        }
        var submitted = Text(doc.QuerySelector("[id$='AnswerSectionContent'] .h-color-grey"));
        return new AssignmentView(status, score, assessed, feedback, submitted);
    }

    /// <summary>A numeric query parameter (case-insensitive name), or null.</summary>
    private static string? QueryValue(Uri uri, string name)
    {
        foreach (var pair in uri.Query.TrimStart('?').Split('&'))
        {
            var eq = pair.IndexOf('=');
            if (eq > 0 && pair[..eq].Equals(name, StringComparison.OrdinalIgnoreCase) && long.TryParse(pair[(eq + 1)..], out _))
                return pair[(eq + 1)..];
        }
        return null;
    }

    /// <summary>
    /// The element a link on the itslearning site points at, if any: <c>ViewLearningToolElement.aspx?LearningToolElementId=</c>,
    /// <c>ContentArea.aspx?…ElementID=</c>, <c>main.aspx?ElementID=</c>, and folders (<c>…folder…?FolderID=</c>; a folder's id is
    /// its element id). Links to other hosts give null.
    /// </summary>
    public static int? ElementIdFromUrl(Uri url, Uri? site)
    {
        if (site is null || !url.IsAbsoluteUri || !string.Equals(url.Host, site.Host, StringComparison.OrdinalIgnoreCase)) return null;
        var id = QueryValue(url, "LearningToolElementId") ?? QueryValue(url, "ElementID")
            ?? (url.AbsolutePath.Contains("folder", StringComparison.OrdinalIgnoreCase) ? QueryValue(url, "FolderID") : null);
        return int.TryParse(id, out var elementId) && elementId > 0 ? elementId : null;
    }

    /// <summary>True if the response is the itslearning web view (not a login/logout page).</summary>
    public static bool IsElementView(Uri finalUri) =>
        finalUri.AbsolutePath.Contains("ViewLearningToolElement", StringComparison.OrdinalIgnoreCase);
}
