using Educ8.App.Core.Elements;
using Educ8.App.Core.Resources;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views.Elements;

/// <summary>An itslearning Page: its blocks as titled sections of native rich text.</summary>
internal static class ContentPage
{
    public static Adw.NavigationPage Create(ContentPageViewModel vm)
    {
        var sections = VBox(30);
        Bind.Collection(vm.Sections, () =>
        {
            while (sections.GetFirstChild() is { } child) sections.Remove(child);
            foreach (var section in vm.Sections) sections.Append(Section(section, vm));
        });

        var column = VBox(0, sections);
        column.SetMarginTop(24);
        column.SetMarginBottom(24);
        column.SetMarginStart(18);
        column.SetMarginEnd(18);
        return ElementPages.Scaffold(vm, Scrolled(Clamp(column, 800)));
    }

    private static Widget Section(PageSectionViewModel section, ContentPageViewModel vm)
    {
        var box = VBox(12);
        if (section.Title is { } title) box.Append(WrapLabel(title, "title-3"));
        if (section.UnsupportedText is { } unsupported)
        {
            var open = Button.NewWithLabel(Strings.OpenInItslearning);
            open.SetHalign(Align.Start);
            Bind.Command(open, vm.OpenInItslearningCommand);
            box.Append(WrapLabel(unsupported, "dim-label"));
            box.Append(open);
        }
        else
        {
            box.Append(RichTextView.Create(section.Content, url => vm.OpenLinkCommand.Execute(url),
                vm.HasUrl ? () => vm.OpenInItslearningCommand.Execute(null) : null));
        }
        return box;
    }
}
