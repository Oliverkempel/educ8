using Educ8.App.Core.Elements;
using Educ8.App.Core.Resources;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views.Elements;

/// <summary>A link with its description: the description, then where it goes and "Open link".</summary>
internal static class LinkPage
{
    public static Adw.NavigationPage Create(LinkViewModel vm)
    {
        var open = Button.NewWithLabel(Strings.LinkOpen);
        open.AddCssClass("suggested-action");
        open.AddCssClass("pill");
        open.SetHalign(Align.Start);
        Bind.Command(open, vm.OpenTargetCommand);

        var column = VBox(18,
            RichTextView.Create(vm.Description, url => vm.OpenLinkCommand.Execute(url), () => vm.OpenInItslearningCommand.Execute(null)),
            Label(vm.TargetText, "dim-label"),
            open);
        column.SetMarginTop(24);
        column.SetMarginBottom(24);
        column.SetMarginStart(18);
        column.SetMarginEnd(18);
        return ElementPages.Scaffold(vm, Scrolled(Clamp(column, 800)));
    }
}
