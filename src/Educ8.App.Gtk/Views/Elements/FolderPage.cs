using Educ8.App.Core.Elements;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views.Elements;

/// <summary>A folder's items as a boxed list. Items open through their modules (subfolders push further pages).</summary>
internal static class FolderPage
{
    public static Adw.NavigationPage Create(FolderViewModel vm) => ElementPages.Scaffold(vm, List(vm));

    /// <summary>The folder's list with its states, for embedding (the course page's Resources tab).</summary>
    public static Widget Content(FolderViewModel vm) => ElementPages.States(vm, List(vm));

    private static Widget List(FolderViewModel vm)
    {
        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetValign(Align.Start);
        Bind.Collection(vm.Items, () =>
        {
            list.RemoveAll();
            foreach (var item in vm.Items) list.Append(MaterialRows.Row(item));
        });

        var page = VBox(0, list);
        page.SetMarginTop(24);
        page.SetMarginBottom(24);
        page.SetMarginStart(18);
        page.SetMarginEnd(18);
        return Scrolled(Clamp(page, 900));
    }
}
