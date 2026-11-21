using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Educ8.Documents;

/// <summary>
/// Converts itslearning author HTML to a <see cref="RichDocument"/> for native rendering: keeps structure (paragraphs, headings,
/// lists, quotes, code, simple tables), inline styles (bold, italic, …, also from <c>style</c> attributes) and safe links; turns
/// media and embeds into placeholders. Text without tags is treated as plain text (blank lines separate paragraphs).
/// </summary>
public static partial class HtmlToRichText
{
    private const int MaxTableColumns = 6;
    private static readonly HtmlParser Parser = new();

    public static RichDocument Convert(string? html, Uri? baseUri = null)
    {
        if (string.IsNullOrWhiteSpace(html)) return RichDocument.Empty;
        if (!LooksLikeHtml().IsMatch(html)) return PlainText(html);
        using var doc = Parser.ParseDocument("<!doctype html><html><body>" + html + "</body></html>");
        var root = new Context();
        new Walker(baseUri).Children(doc.Body!, root, RichStyle.None, null);
        root.Flush();
        return new RichDocument(root.Blocks);
    }

    private static RichDocument PlainText(string text)
    {
        var paragraphs = BlankLines().Split(text.Replace("\r\n", "\n").Trim())
            .Select(p => p.Trim())
            .Where(p => p.Length > 0)
            .Select(p => (RichBlock)new RichParagraph([new RichSpan(InlineSpaces().Replace(p.Replace(' ', ' '), " "))]))
            .ToList();
        return new RichDocument(paragraphs);
    }

    /// <summary>Blocks being built, plus the paragraph currently collecting text.</summary>
    private sealed class Context
    {
        public List<RichBlock> Blocks { get; } = [];
        public List<RichSpan> Spans { get; } = [];

        public void Text(string text, RichStyle style, Uri? link)
        {
            if (text.Length == 0) return;
            var last = Spans.Count > 0 ? Spans[^1].Text : "";
            // Collapsed whitespace: no space at a paragraph start, after a line break, or after another space.
            if (text[0] == ' ' && (last.Length == 0 || last[^1] is ' ' or '\n')) text = text.TrimStart(' ');
            if (text.Length == 0) return;
            if (Spans.Count > 0 && Spans[^1].Style == style && Spans[^1].Link == link)
                Spans[^1] = Spans[^1] with { Text = last + text };
            else
                Spans.Add(new RichSpan(text, style, link));
        }

        public void LineBreak(RichStyle style, Uri? link)
        {
            if (Spans.Count > 0 && Spans[^1].Text.EndsWith(' ')) Spans[^1] = Spans[^1] with { Text = Spans[^1].Text.TrimEnd(' ') };
            Text("\n", style, link);
        }

        /// <summary>The collected text as spans with outer whitespace trimmed (empty when there's no visible text).</summary>
        public List<RichSpan> TakeSpans()
        {
            var spans = Spans.ToList();
            Spans.Clear();
            while (spans.Count > 0 && spans[0].Text.TrimStart(' ', '\n').Length == 0) spans.RemoveAt(0);
            while (spans.Count > 0 && spans[^1].Text.TrimEnd(' ', '\n').Length == 0) spans.RemoveAt(spans.Count - 1);
            if (spans.Count == 0) return spans;
            spans[0] = spans[0] with { Text = spans[0].Text.TrimStart(' ', '\n') };
            spans[^1] = spans[^1] with { Text = spans[^1].Text.TrimEnd(' ', '\n') };
            return spans;
        }

        public void Flush()
        {
            var spans = TakeSpans();
            if (spans.Count > 0) Blocks.Add(new RichParagraph(spans));
        }

        public void Add(RichBlock block)
        {
            Flush();
            Blocks.Add(block);
        }
    }

    private sealed class Walker(Uri? baseUri)
    {
        public void Children(INode node, Context ctx, RichStyle style, Uri? link)
        {
            foreach (var child in node.ChildNodes)
            {
                switch (child)
                {
                    case IText text:
                        ctx.Text(Whitespace().Replace(text.Data.Replace(' ', ' '), " "), style, link);
                        break;
                    case IElement el:
                        Element(el, ctx, style, link);
                        break;
                }
            }
        }

        private void Element(IElement el, Context ctx, RichStyle style, Uri? link)
        {
            switch (el.LocalName)
            {
                case "script" or "style" or "noscript" or "input" or "button" or "select" or "textarea" or "template" or "head":
                    return;
                case "br":
                    ctx.LineBreak(style, link);
                    return;
                case "hr":
                    ctx.Add(new RichRule());
                    return;
                case "strong" or "b":
                    Children(el, ctx, style | RichStyle.Bold, link);
                    return;
                case "em" or "i" or "cite":
                    Children(el, ctx, style | RichStyle.Italic, link);
                    return;
                case "u" or "ins":
                    Children(el, ctx, style | RichStyle.Underline, link);
                    return;
                case "s" or "strike" or "del":
                    Children(el, ctx, style | RichStyle.Strikethrough, link);
                    return;
                case "sub":
                    Children(el, ctx, style | RichStyle.Subscript, link);
                    return;
                case "sup":
                    Children(el, ctx, style | RichStyle.Superscript, link);
                    return;
                case "code" or "kbd" or "samp" or "tt":
                    Children(el, ctx, style | RichStyle.Code, link);
                    return;
                case "a":
                    Children(el, ctx, StyleOf(el, style), SafeLink(el.GetAttribute("href")) ?? link);
                    return;
                case "img":
                    ctx.Add(new RichImage(Resolve(el.GetAttribute("src")), NullIfBlank(el.GetAttribute("alt"))));
                    return;
                case "h1" or "h2" or "h3" or "h4" or "h5" or "h6":
                    Heading(el, ctx, link);
                    return;
                case "ul" or "ol":
                    ctx.Add(List(el));
                    return;
                case "blockquote":
                    var quote = new Context();
                    Children(el, quote, RichStyle.None, null);
                    quote.Flush();
                    if (quote.Blocks.Count > 0) ctx.Add(new RichQuote(quote.Blocks));
                    return;
                case "pre":
                    var code = el.TextContent.Replace("\r\n", "\n").Trim('\n');
                    if (code.Trim().Length > 0) ctx.Add(new RichCode(code));
                    return;
                case "table":
                    ctx.Add(Table(el));
                    return;
                case "oembed":
                    ctx.Add(new RichUnsupported(RichUnsupportedKind.Video, SafeLink(el.GetAttribute("url") ?? el.TextContent.Trim())));
                    return;
                case "iframe":
                    var src = SafeLink(el.GetAttribute("src"));
                    var interactive = src?.ToString().Contains("h5p", StringComparison.OrdinalIgnoreCase) == true;
                    ctx.Add(new RichUnsupported(interactive ? RichUnsupportedKind.Interactive : RichUnsupportedKind.Embed, src));
                    return;
                case "video" or "audio":
                    var media = SafeLink(el.GetAttribute("src") ?? el.QuerySelector("source")?.GetAttribute("src"));
                    ctx.Add(new RichUnsupported(el.LocalName == "video" ? RichUnsupportedKind.Video : RichUnsupportedKind.Audio, media));
                    return;
                case "object" or "embed" or "canvas" or "svg" or "math":
                    ctx.Add(new RichUnsupported(RichUnsupportedKind.Other, null));
                    return;
                case "p" or "div" or "section" or "article" or "header" or "footer" or "main" or "aside" or "nav" or "figure"
                    or "figcaption" or "center" or "address" or "dl" or "dt" or "dd" or "details" or "summary" or "form" or "fieldset":
                    ctx.Flush();
                    Children(el, ctx, StyleOf(el, style), link);
                    ctx.Flush();
                    return;
                default: // span, font, label, … and unknown inline elements
                    Children(el, ctx, StyleOf(el, style), link);
                    return;
            }
        }

        private void Heading(IElement el, Context ctx, Uri? link)
        {
            ctx.Flush();
            var inner = new Context();
            Children(el, inner, RichStyle.None, link);
            var spans = inner.TakeSpans();
            if (spans.Count > 0) ctx.Blocks.Add(new RichHeading(el.LocalName[1] - '0', spans));
            ctx.Blocks.AddRange(inner.Blocks); // block content inside a heading (rare): kept after it
        }

        private RichList List(IElement el)
        {
            var ordered = el.LocalName == "ol";
            var number = int.TryParse(el.GetAttribute("start"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var start) ? start : 1;
            var items = new List<RichListItem>();
            foreach (var li in el.Children.Where(c => c.LocalName == "li"))
            {
                var item = new Context();
                Children(li, item, RichStyle.None, null);
                item.Flush();
                items.Add(new RichListItem(ordered ? $"{number}." : "•", item.Blocks));
                number++;
            }
            return new RichList(ordered, items);
        }

        /// <summary>A simple table (no merged cells, no block content, few columns), or a placeholder.</summary>
        private RichBlock Table(IElement table)
        {
            var unsupported = new RichUnsupported(RichUnsupportedKind.Table, null);
            if (table.QuerySelector("table") is not null) return unsupported;
            var rows = table.QuerySelectorAll("tr").Where(r => r.Closest("table") == table).ToList();
            if (rows.Count == 0) return unsupported;
            var result = new List<IReadOnlyList<IReadOnlyList<RichSpan>>>();
            foreach (var row in rows)
            {
                var cells = row.Children.Where(c => c.LocalName is "td" or "th").ToList();
                if (cells.Count > MaxTableColumns) return unsupported;
                var rowCells = new List<IReadOnlyList<RichSpan>>();
                foreach (var cell in cells)
                {
                    if (Span(cell, "colspan") > 1 || Span(cell, "rowspan") > 1) return unsupported;
                    var ctx = new Context();
                    Children(cell, ctx, cell.LocalName == "th" ? RichStyle.Bold : RichStyle.None, null);
                    ctx.Flush();
                    if (ctx.Blocks.Any(b => b is not RichParagraph)) return unsupported;
                    // Paragraphs in a cell become lines.
                    var spans = new List<RichSpan>();
                    foreach (var p in ctx.Blocks.Cast<RichParagraph>())
                    {
                        if (spans.Count > 0) spans.Add(new RichSpan("\n"));
                        spans.AddRange(p.Spans);
                    }
                    rowCells.Add(spans);
                }
                if (rowCells.Count > 0) result.Add(rowCells);
            }
            if (result.Count == 0) return unsupported;
            var header = rows[0].Children.Any(c => c.LocalName == "th") || rows[0].Closest("thead") is not null;
            return new RichTable(result, header);
        }

        private static int Span(IElement cell, string attribute) =>
            int.TryParse(cell.GetAttribute(attribute), NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) ? n : 1;

        /// <summary>Adds bold/italic/underline/strike from an inline <c>style</c> attribute (authors' editors use spans for these).</summary>
        private static RichStyle StyleOf(IElement el, RichStyle style)
        {
            if (el.GetAttribute("style") is not { Length: > 0 } css) return style;
            if (BoldStyle().IsMatch(css)) style |= RichStyle.Bold;
            if (ItalicStyle().IsMatch(css)) style |= RichStyle.Italic;
            if (UnderlineStyle().IsMatch(css)) style |= RichStyle.Underline;
            if (StrikeStyle().IsMatch(css)) style |= RichStyle.Strikethrough;
            return style;
        }

        private Uri? Resolve(string? href)
        {
            if (string.IsNullOrWhiteSpace(href)) return null;
            href = href.Trim();
            if (Uri.TryCreate(href, UriKind.Absolute, out var absolute) && absolute.Scheme is "http" or "https" or "mailto") return absolute;
            if (baseUri is not null && !href.Contains(':', StringComparison.Ordinal) && Uri.TryCreate(baseUri, href, out var relative)) return relative;
            return null;
        }

        /// <summary>http(s) and mailto links only; page anchors, javascript: and unresolvable links are dropped (the text stays).</summary>
        private Uri? SafeLink(string? href) => href?.TrimStart().StartsWith('#') == true ? null : Resolve(href);
    }

    private static string? NullIfBlank(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    [GeneratedRegex(@"<[a-zA-Z/!][^>]*>")]
    private static partial Regex LooksLikeHtml();

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();

    [GeneratedRegex(@"[ \t]+")]
    private static partial Regex InlineSpaces();

    [GeneratedRegex(@"\n[ \t]*\n\s*")]
    private static partial Regex BlankLines();

    [GeneratedRegex(@"font-weight\s*:\s*(bold|bolder|[6-9]00)\b", RegexOptions.IgnoreCase)]
    private static partial Regex BoldStyle();

    [GeneratedRegex(@"font-style\s*:\s*(italic|oblique)", RegexOptions.IgnoreCase)]
    private static partial Regex ItalicStyle();

    [GeneratedRegex(@"text-decoration[^;]*underline", RegexOptions.IgnoreCase)]
    private static partial Regex UnderlineStyle();

    [GeneratedRegex(@"text-decoration[^;]*line-through", RegexOptions.IgnoreCase)]
    private static partial Regex StrikeStyle();
}
