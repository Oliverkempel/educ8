using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Educ8.Documents;

/// <summary>
/// Converts itslearning author HTML (pages, descriptions) to compact Markdown for an LLM: keeps structure,
/// links, lists, tables and embedded-media URLs; drops styling.
/// </summary>
public static partial class HtmlToMarkdown
{
    private static readonly HtmlParser Parser = new();

    public static string Convert(string? html)
    {
        if (string.IsNullOrWhiteSpace(html)) return "";
        using var doc = Parser.ParseDocument("<!doctype html><html><body>" + html + "</body></html>");
        var raw = Children(doc.Body!, 0);
        return Normalize(raw);
    }

    /// <summary>Converts and truncates to <paramref name="maxChars"/> (at a line break where possible).</summary>
    public static string ConvertExcerpt(string? html, int maxChars)
    {
        var md = Convert(html);
        return Truncate(md, maxChars);
    }

    public static string Truncate(string text, int maxChars)
    {
        if (text.Length <= maxChars) return text;
        var cut = text.LastIndexOf('\n', maxChars);
        if (cut < maxChars / 2) cut = maxChars;
        return text[..cut].TrimEnd() + " …";
    }

    private static string Children(INode node, int listDepth)
    {
        var sb = new StringBuilder();
        foreach (var child in node.ChildNodes) sb.Append(Render(child, listDepth));
        return sb.ToString();
    }

    private static string Render(INode node, int listDepth)
    {
        switch (node)
        {
            case IText text:
                return Whitespace().Replace(text.Data.Replace('\u00A0', ' '), " ");
            case IElement el:
                return Element(el, listDepth);
            default:
                return "";
        }
    }

    private static string Element(IElement el, int listDepth)
    {
        var tag = el.LocalName;
        switch (tag)
        {
            case "script" or "style" or "noscript" or "input" or "button" or "select" or "textarea" or "template":
                return "";
            case "br":
                return "\n";
            case "hr":
                return "\n\n---\n\n";
            case "h1" or "h2" or "h3" or "h4" or "h5" or "h6":
                var heading = Inline(Children(el, listDepth));
                return heading.Length == 0 ? "" : $"\n\n{new string('#', tag[1] - '0')} {heading}\n\n";
            case "strong" or "b":
                // Authors nest bold freely (<strong><a><strong>…); emit the marker only at the outermost level.
                return HasAncestor(el, "strong", "b") ? Children(el, listDepth) : Wrap(Children(el, listDepth), "**");
            case "em" or "i":
                return HasAncestor(el, "em", "i") ? Children(el, listDepth) : Wrap(Children(el, listDepth), "*");
            case "code" when el.ParentElement?.LocalName != "pre":
                return Wrap(el.TextContent, "`");
            case "pre":
                return $"\n\n```\n{el.TextContent.Trim('\n')}\n```\n\n";
            case "a":
                return Link(el, Children(el, listDepth));
            case "img":
                var alt = el.GetAttribute("alt")?.Trim();
                return string.IsNullOrEmpty(alt) ? " [Image] " : $" [Image: {alt}] ";
            case "oembed":
                var media = el.GetAttribute("url") ?? el.TextContent.Trim();
                return string.IsNullOrEmpty(media) ? "" : $"\n\n[Embedded media: {media}]\n\n";
            case "iframe" or "video" or "audio" or "embed" or "object" or "source":
                var src = el.GetAttribute("src") ?? el.GetAttribute("data");
                return string.IsNullOrEmpty(src) ? "" : $"\n\n[Embedded content: {src}]\n\n";
            case "ul" or "ol":
                return "\n\n" + List(el, listDepth) + "\n\n";
            case "blockquote":
                var quoted = Normalize(Children(el, listDepth));
                return "\n\n" + string.Join('\n', quoted.Split('\n').Select(l => "> " + l)) + "\n\n";
            case "table":
                return "\n\n" + Table(el) + "\n\n";
            case "p" or "div" or "section" or "article" or "header" or "footer" or "main" or "figure"
                or "figcaption" or "center" or "form" or "dl" or "dt" or "dd" or "address" or "details" or "summary":
                return "\n\n" + Children(el, listDepth) + "\n\n";
            default:
                return Children(el, listDepth);
        }
    }

    private static string Link(IElement a, string inner)
    {
        var href = a.GetAttribute("href")?.Trim();
        var text = Inline(inner);
        if (string.IsNullOrEmpty(href) || href.StartsWith('#') || href.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
            return inner;
        if (text.Length == 0 || text == href) return $"<{href}>";
        // Keep surrounding spaces outside the link.
        var lead = inner.Length > 0 && char.IsWhiteSpace(inner[0]) ? " " : "";
        var trail = inner.Length > 0 && char.IsWhiteSpace(inner[^1]) ? " " : "";
        return $"{lead}[{text.Replace("[", "\\[").Replace("]", "\\]")}]({href.Replace(" ", "%20").Replace(")", "%29")}){trail}";
    }

    private static string List(IElement list, int depth)
    {
        var ordered = list.LocalName == "ol";
        var indent = new string(' ', depth * 2);
        var sb = new StringBuilder();
        var n = 1;
        foreach (var li in list.Children.Where(c => c.LocalName == "li"))
        {
            var marker = ordered ? $"{n++}. " : "- ";
            var body = Normalize(Children(li, depth + 1));
            var lines = body.Split('\n');
            sb.Append(indent).Append(marker).Append(lines[0]).Append('\n');
            foreach (var line in lines.Skip(1).Where(l => l.Length > 0))
                sb.Append(line.StartsWith(indent + "  ") || line.TrimStart().StartsWith("- ") ? line : indent + "  " + line).Append('\n');
        }
        return sb.ToString().TrimEnd('\n');
    }

    private static string Table(IElement table)
    {
        var rows = table.QuerySelectorAll("tr")
            .Select(tr => tr.Children.Where(c => c.LocalName is "td" or "th")
                .Select(c => Inline(Normalize(Children(c, 0))).Replace("|", "\\|").Replace("\n", " "))
                .ToList())
            .Where(r => r.Count > 0)
            .ToList();
        if (rows.Count == 0) return "";
        var width = rows.Max(r => r.Count);
        foreach (var r in rows) while (r.Count < width) r.Add("");
        var sb = new StringBuilder();
        sb.Append("| ").Append(string.Join(" | ", rows[0])).Append(" |\n");
        sb.Append('|').Append(string.Concat(Enumerable.Repeat("---|", width))).Append('\n');
        foreach (var r in rows.Skip(1)) sb.Append("| ").Append(string.Join(" | ", r)).Append(" |\n");
        return sb.ToString().TrimEnd('\n');
    }

    private static bool HasAncestor(IElement el, params string[] tags)
    {
        for (var p = el.ParentElement; p is not null; p = p.ParentElement)
            if (tags.Contains(p.LocalName)) return true;
        return false;
    }

    /// <summary>Wraps text in a marker, keeping leading/trailing spaces outside it. Multi-line text is wrapped per line.</summary>
    private static string Wrap(string inner, string marker)
    {
        var trimmed = inner.Trim();
        if (trimmed.Length == 0) return inner;
        if (trimmed.Contains('\n'))
            return string.Join('\n', inner.Split('\n').Select(line => line.Trim().Length == 0 ? line : Wrap(line, marker)));
        var lead = char.IsWhiteSpace(inner[0]) ? " " : "";
        var trail = char.IsWhiteSpace(inner[^1]) ? " " : "";
        return $"{lead}{marker}{trimmed}{marker}{trail}";
    }

    private static string Inline(string text) => Whitespace().Replace(text, " ").Trim();

    private static string Normalize(string text)
    {
        var lines = text.Replace("\r", "").Split('\n').Select(l => l.TrimEnd());
        var joined = string.Join('\n', lines.Select(l => l.StartsWith(' ') && !l.TrimStart().StartsWith("- ") && !OrderedItem().IsMatch(l.TrimStart()) ? l.TrimStart() : l));
        return BlankLines().Replace(joined, "\n\n").Trim('\n', ' ');
    }

    [GeneratedRegex(@"[ \t\r\n\f]+")]
    private static partial Regex Whitespace();

    [GeneratedRegex(@"\n{3,}")]
    private static partial Regex BlankLines();

    [GeneratedRegex(@"^\d+\. ")]
    private static partial Regex OrderedItem();
}
