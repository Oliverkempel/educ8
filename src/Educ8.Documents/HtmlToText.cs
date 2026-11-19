using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Educ8.Documents;

/// <summary>
/// HTML → readable plain text for native UI labels (no Markdown symbols): paragraphs as blank lines, list items as "• ",
/// links as their text, embedded media as their URL.
/// </summary>
public static partial class HtmlToText
{
    private static readonly HtmlParser Parser = new();

    public static string Convert(string? html)
    {
        if (string.IsNullOrWhiteSpace(html)) return "";
        using var doc = Parser.ParseDocument("<!doctype html><html><body>" + html + "</body></html>");
        var sb = new StringBuilder();
        Walk(doc.Body!, sb);
        var lines = sb.ToString().Replace("\r", "").Split('\n').Select(l => Spaces().Replace(l, " ").Trim());
        return BlankLines().Replace(string.Join('\n', lines), "\n\n").Trim();
    }

    private static void Walk(INode node, StringBuilder sb)
    {
        foreach (var child in node.ChildNodes)
        {
            switch (child)
            {
                case IText t:
                    sb.Append(t.Data.Replace(' ', ' ').Replace('\n', ' '));
                    break;
                case IElement e:
                    switch (e.LocalName)
                    {
                        case "script" or "style" or "template": break;
                        case "br": sb.Append('\n'); break;
                        case "li": sb.Append("\n• "); Walk(e, sb); break;
                        case "oembed": sb.Append('\n').Append(e.GetAttribute("url") ?? e.TextContent.Trim()).Append('\n'); break;
                        case "img": if (e.GetAttribute("alt") is { Length: > 0 } alt) sb.Append(alt); break;
                        case "p" or "div" or "h1" or "h2" or "h3" or "h4" or "h5" or "h6" or "ul" or "ol" or "table" or "tr" or "section" or "blockquote" or "pre":
                            sb.Append("\n\n"); Walk(e, sb); sb.Append("\n\n"); break;
                        case "td" or "th": Walk(e, sb); sb.Append('\t'); break;
                        default: Walk(e, sb); break;
                    }
                    break;
            }
        }
    }

    [GeneratedRegex(@"[ \t]+")]
    private static partial Regex Spaces();

    [GeneratedRegex(@"\n{3,}")]
    private static partial Regex BlankLines();
}
