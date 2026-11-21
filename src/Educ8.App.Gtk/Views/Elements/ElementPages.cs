using Educ8.App.Core.Elements;
using Educ8.App.Core.Resources;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views.Elements;

/// <summary>
/// Builds the page for an element page view model (one arm per module's page type). A view model without a builder here gets a
/// generic page with "Open in itslearning", so a module that's ahead of this platform still works.
/// </summary>
internal static class ElementPages
{
    public static Adw.NavigationPage Create(ElementPageViewModel vm) => vm switch
    {
        FolderViewModel folder => FolderPage.Create(folder),
        ContentPageViewModel page => ContentPage.Create(page),
        LinkViewModel link => LinkPage.Create(link),
        AssignmentViewModel assignment => AssignmentPage.Create(assignment),
        _ => Scaffold(vm, Label("")),
    };

    /// <summary>
    /// The frame every element page shares: header (title and kind, "Open in itslearning", refresh) and the loading / message /
    /// content states. Starts loading the view model.
    /// </summary>
    public static Adw.NavigationPage Scaffold(ElementPageViewModel vm, Widget content, params Widget[] headerExtras)
    {
        var web = IconButton(Icons.Name(Core.Presentation.AppIcon.OpenExternal), Strings.OpenInItslearning);
        Bind.Command(web, vm.OpenInItslearningCommand);
        var refresh = IconButton(Icons.Name(Core.Presentation.AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);

        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(Adw.WindowTitle.New(vm.Title, vm.KindText));
        header.PackEnd(refresh);
        header.PackEnd(web);
        foreach (var extra in headerExtras) header.PackEnd(extra);

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(States(vm, content));

        var page = Adw.NavigationPage.New(toolbar, vm.Title);
        _ = vm.EnsureLoadedAsync();
        return page;
    }

    /// <summary>The loading / message / content states of an element page, without a header (e.g. the course page's Resources tab).</summary>
    public static Widget States(ElementPageViewModel vm, Widget content)
    {
        var openWeb = Button.NewWithLabel(Strings.OpenInItslearning);
        openWeb.AddCssClass("pill");
        openWeb.SetHalign(Align.Center);
        Bind.Command(openWeb, vm.OpenInItslearningCommand);

        var message = Adw.StatusPage.New();
        message.SetIconName(Icons.Name(vm.Icon));
        message.SetChild(openWeb);

        var states = Stack.New();
        states.AddNamed(Spinner(48), nameof(ElementPageState.Loading));
        states.AddNamed(message, nameof(ElementPageState.Message));
        states.AddNamed(content, nameof(ElementPageState.Content));
        Bind.Property(vm, () =>
        {
            message.SetTitle(vm.MessageText ?? "");
            openWeb.SetVisible(vm.HasUrl);
            states.SetVisibleChildName(vm.State.ToString());
        }, nameof(ElementPageViewModel.State), nameof(ElementPageViewModel.MessageText));
        return states;
    }
}
