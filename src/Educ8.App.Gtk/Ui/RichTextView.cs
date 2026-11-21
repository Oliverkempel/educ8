using System.Text;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.Documents;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Ui;

/// <summary>
/// Renders a <see cref="RichDocument"/> with stock widgets: wrapped, selectable labels with Pango markup (every text and link is
/// escaped), Adwaita heading classes, and boxed-list rows for content shown as a placeholder (videos, embeds, images).
/// </summary>
internal static class RichTextView
{
    /// <param name="openLink">Called for links in the text (the default launcher is bypassed).</param>
    /// <param name="openInItslearning">For placeholders without a link of their own; null hides their button.</param>
    public static Widget Create(RichDocument document, Action<Uri> openLink, Action? openInItslearning)
    {
        var box = VBox(12);
        foreach (var block in document.Blocks) box.Append(Block(block, openLink, openInItslearning));
        return box;
    }

    private static Widget Block(RichBlock block, Action<Uri> openLink, Action? openInItslearning) => block switch
    {
        RichParagraph p => Text(p.Spans, openLink),
        RichHeading h => Text(h.Spans, openLink, h.Level <= 2 ? "title-2" : h.Level == 3 ? "title-3" : "heading"),
        RichList l => List(l, openLink, openInItslearning),
        RichQuote q => Quote(q, openLink, openInItslearning),
        RichCode c => Code(c),
        RichTable t => Table(t, openLink),
        RichRule => Separator.New(Orientation.Horizontal),
        RichImage i => Placeholder(AppIcon.Image, i.Alt ?? Strings.ContentImage, null, Strings.OpenInItslearning, openInItslearning),
        RichUnsupported u => Placeholder(AppIcons.For(u.Kind), Texts.Unsupported(u.Kind), u.Link?.Host,
            Texts.UnsupportedAction(u.Kind, u.Link is not null), u.Link is { } link ? () => openLink(link) : openInItslearning),
        _ => Label(""),
    };

    private static Label Text(IReadOnlyList<RichSpan> spans, Action<Uri> openLink, params string[] cssClasses)
    {
        var label = global::Gtk.Label.New(null);
        label.SetMarkup(Markup(spans));
        label.SetWrap(true);
        label.SetWrapMode(Pango.WrapMode.WordChar);
        label.SetXalign(0);
        label.SetSelectable(true);
        foreach (var c in cssClasses) label.AddCssClass(c);
        label.OnActivateLink += (_, args) =>
        {
            if (Uri.TryCreate(args.Uri, UriKind.Absolute, out var uri)) openLink(uri);
            return true; // handled: don't let GTK launch it
        };
        return label;
    }

    private static Widget List(RichList list, Action<Uri> openLink, Action? openInItslearning)
    {
        var box = VBox(6);
        foreach (var item in list.Items)
        {
            var marker = Label(item.Marker, "dim-label");
            marker.SetValign(Align.Start);
            marker.SetSizeRequest(list.Ordered ? 24 : 12, -1);
            var content = VBox(6);
            foreach (var block in item.Blocks) content.Append(Block(block, openLink, openInItslearning));
            content.SetHexpand(true);
            box.Append(HBox(8, marker, content));
        }
        box.SetMarginStart(6);
        return box;
    }

    private static Widget Quote(RichQuote quote, Action<Uri> openLink, Action? openInItslearning)
    {
        var content = VBox(6);
        foreach (var block in quote.Blocks) content.Append(Block(block, openLink, openInItslearning));
        content.AddCssClass("dim-label");
        content.SetHexpand(true);
        return HBox(12, Separator.New(Orientation.Vertical), content);
    }

    private static Widget Code(RichCode code)
    {
        var label = Label(code.Text, "monospace");
        label.SetSelectable(true);
        label.SetWrap(true);
        label.SetWrapMode(Pango.WrapMode.WordChar);
        return label;
    }

    private static Widget Table(RichTable table, Action<Uri> openLink)
    {
        var grid = Grid.New();
        grid.SetColumnSpacing(24);
        grid.SetRowSpacing(8);
        for (var r = 0; r < table.Rows.Count; r++)
            for (var c = 0; c < table.Rows[r].Count; c++)
            {
                var cell = Text(table.Rows[r][c], openLink);
                cell.SetValign(Align.Start);
                grid.Attach(cell, c, r, 1, 1);
            }
        var scrolled = ScrolledWindow.New();
        scrolled.SetPolicy(PolicyType.Automatic, PolicyType.Never);
        scrolled.SetPropagateNaturalHeight(true);
        scrolled.SetChild(grid);
        return scrolled;
    }

    /// <summary>A row standing in for content the app doesn't show, with a button to view it where it lives.</summary>
    private static Widget Placeholder(AppIcon icon, string title, string? subtitle, string action, Action? open)
    {
        var row = Row(title, subtitle);
        row.AddPrefix(Image.NewFromIconName(Icons.Name(icon)));
        if (open is not null)
        {
            var button = Button.NewWithLabel(action);
            button.SetValign(Align.Center);
            button.OnClicked += (_, _) => open();
            row.AddSuffix(button);
        }
        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.Append(row);
        return list;
    }

    /// <summary>Pango markup for styled spans; all text and link targets are escaped.</summary>
    internal static string Markup(IReadOnlyList<RichSpan> spans)
    {
        var sb = new StringBuilder();
        foreach (var span in spans)
        {
            var text = Escape(span.Text);
            if (span.Style.HasFlag(RichStyle.Code)) text = $"<tt>{text}</tt>";
            if (span.Style.HasFlag(RichStyle.Subscript)) text = $"<sub>{text}</sub>";
            if (span.Style.HasFlag(RichStyle.Superscript)) text = $"<sup>{text}</sup>";
            if (span.Style.HasFlag(RichStyle.Strikethrough)) text = $"<s>{text}</s>";
            if (span.Style.HasFlag(RichStyle.Underline)) text = $"<u>{text}</u>";
            if (span.Style.HasFlag(RichStyle.Italic)) text = $"<i>{text}</i>";
            if (span.Style.HasFlag(RichStyle.Bold)) text = $"<b>{text}</b>";
            if (span.Link is { } link) text = $"<a href=\"{Escape(link.AbsoluteUri)}\">{text}</a>";
            sb.Append(text);
        }
        return sb.ToString();
    }
}
