using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// The Notifications page: itslearning's bell notifications as a boxed list, unread ones marked with an accent dot. Clicking one
/// marks it read and opens it: a course page pushed here (back returns to the list) or the browser. Header: mark all as read, refresh.
/// </summary>
internal sealed class NotificationsView
{
    public Widget Root { get; }

    public NotificationsView(NotificationsViewModel vm)
    {
        var markAll = Button.NewWithLabel(Strings.NotificationsMarkAllRead);
        Bind.Command(markAll, vm.MarkAllReadCommand);
        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);

        var header = Adw.HeaderBar.New();
        header.PackStart(markAll);
        header.PackEnd(refresh);

        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetValign(Align.Start);
        Bind.Collection(vm.Items, () =>
        {
            list.RemoveAll();
            foreach (var item in vm.Items) list.Append(Row(item, vm));
        });

        var page = VBox(12, list);
        page.SetMarginTop(18);
        page.SetMarginBottom(24);
        page.SetMarginStart(18);
        page.SetMarginEnd(18);

        var empty = Adw.StatusPage.New();
        empty.SetIconName(Icons.Name(AppIcon.Notification));
        empty.SetTitle(Strings.NotificationsNone);

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(empty, "empty");
        states.AddNamed(Scrolled(Clamp(page, 900)), "list");
        Bind.Property(vm, () => states.SetVisibleChildName(
                vm.IsLoading && vm.Items.Count == 0 ? "loading" : vm.EmptyText is not null ? "empty" : "list"),
            nameof(NotificationsViewModel.IsLoading), nameof(NotificationsViewModel.EmptyText));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(VBox(0, Bind.ErrorBanner(vm, nameof(NotificationsViewModel.ErrorMessage), () => vm.ErrorMessage, vm.RefreshCommand), states));

        // Course pages (and their folders) open on top of the list, with the standard back button.
        var navigation = Adw.NavigationView.New();
        navigation.Add(Adw.NavigationPage.New(toolbar, Strings.NavNotifications));
        vm.CourseOpened += (_, course) => navigation.Push(CoursePage.Create(course, navigation.Push));
        Root = navigation;
    }

    private static Adw.ActionRow Row(NotificationItemViewModel n, NotificationsViewModel vm)
    {
        var row = Widgets.Row(n.Text, n.MetaText);
        row.SetTitleLines(3);
        row.AddPrefix(Image.NewFromIconName(Icons.Name(n.Icon)));

        var unread = Image.NewFromIconName("media-record-symbolic");
        unread.AddCssClass("accent");
        unread.SetTooltipText(Strings.NotificationUnread);
        row.AddSuffix(unread);
        if (n.OpensInApp) row.AddSuffix(Image.NewFromIconName("go-next-symbolic"));
        else if (n.Url is not null) row.AddSuffix(Image.NewFromIconName(Icons.Name(AppIcon.OpenExternal)));
        Bind.Property(n, () => unread.SetVisible(!n.IsRead), nameof(NotificationItemViewModel.IsRead));

        row.SetActivatable(true);
        row.SetTooltipText(n.OpensInApp ? Strings.NotificationOpenCourseHint : Strings.NotificationOpenHint);
        row.OnActivated += (_, _) => vm.OpenCommand.Execute(n);
        return row;
    }
}
