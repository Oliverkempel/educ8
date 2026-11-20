using Gtk;

namespace Educ8.App.Gtk.Ui;

/// <summary>Small factories for common GTK widgets, so views read as layout.</summary>
internal static class Widgets
{
    /// <summary>A start-aligned label with GTK's default behaviour (one line, no wrapping).</summary>
    public static Label Label(string text = "", params string[] cssClasses)
    {
        var label = global::Gtk.Label.New(text);
        label.SetXalign(0);
        foreach (var c in cssClasses) label.AddCssClass(c);
        return label;
    }

    /// <summary>A start-aligned label that wraps at word boundaries, for text that can run over several lines.</summary>
    public static Label WrapLabel(string text = "", params string[] cssClasses)
    {
        var label = Label(text, cssClasses);
        label.SetWrap(true);
        return label;
    }

    public static Box VBox(int spacing = 0, params Widget[] children)
    {
        var box = Box.New(Orientation.Vertical, spacing);
        foreach (var c in children) box.Append(c);
        return box;
    }

    public static Box HBox(int spacing = 0, params Widget[] children)
    {
        var box = Box.New(Orientation.Horizontal, spacing);
        foreach (var c in children) box.Append(c);
        return box;
    }

    public static Button IconButton(string iconName, string tooltip)
    {
        var button = Button.NewFromIconName(iconName);
        button.SetTooltipText(tooltip);
        return button;
    }

    public static Adw.Clamp Clamp(Widget child, int maximumSize = 1000)
    {
        var clamp = Adw.Clamp.New();
        clamp.SetMaximumSize(maximumSize);
        clamp.SetChild(child);
        return clamp;
    }

    public static ScrolledWindow Scrolled(Widget child)
    {
        var scrolled = ScrolledWindow.New();
        scrolled.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        scrolled.SetVexpand(true);
        scrolled.SetChild(child);
        return scrolled;
    }

    /// <summary>A plain-text row (Adwaita rows treat titles as markup by default; course titles contain "&amp;").</summary>
    public static Adw.ActionRow Row(string title, string? subtitle = null)
    {
        var row = Adw.ActionRow.New();
        row.SetUseMarkup(false);
        row.SetTitle(title);
        if (!string.IsNullOrEmpty(subtitle)) row.SetSubtitle(subtitle);
        return row;
    }

    /// <summary>
    /// Escapes text for setters that parse Pango markup: <c>StatusPage</c> description, <c>PreferencesGroup</c> title and description
    /// (and <c>ActionRow</c> unless <see cref="Row"/> turned markup off). <c>StatusPage</c> titles and <c>WindowTitle</c> are plain text:
    /// escaping them shows the entities (checked against libadwaita at runtime).
    /// </summary>
    public static string Escape(string? text) =>
        (text ?? "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");

    /// <summary>
    /// A loading spinner: <c>Adw.Spinner</c> (libadwaita 1.6+), or GTK's own on older libadwaita, e.g. the snap's
    /// GNOME 46 platform (1.5), where the newer widget doesn't exist.
    /// </summary>
    public static Widget Spinner(int size = 32)
    {
        Widget spinner;
        if (Adw.Functions.GetMajorVersion() > 1 || Adw.Functions.GetMinorVersion() >= 6) spinner = Adw.Spinner.New();
        else
        {
            var gtk = global::Gtk.Spinner.New();
            gtk.Start();
            spinner = gtk;
        }
        spinner.SetSizeRequest(size, size);
        spinner.SetHalign(Align.Center);
        spinner.SetValign(Align.Center);
        return spinner;
    }
}
