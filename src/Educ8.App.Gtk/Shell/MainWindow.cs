using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Educ8.App.Gtk.Views;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Shell;

/// <summary>
/// The window: a stack of "starting" (spinner), "signed out" (welcome page) and "signed in" (sidebar + pages),
/// following <see cref="ShellViewModel.State"/>.
/// </summary>
internal sealed class MainWindow
{
    public Adw.ApplicationWindow Window { get; }
    public CalendarView Calendar { get; }
    public CoursesView Courses { get; }
    public MessagesView Messages { get; }

    public MainWindow(Adw.Application app, ShellViewModel shell)
    {
        Window = Adw.ApplicationWindow.New(app);
        Window.SetTitle(Strings.AppName);
        Window.SetDefaultSize(1280, 820);
        Window.SetSizeRequest(720, 520);

        var courses = Courses = new CoursesView(shell.Courses);
        Calendar = new CalendarView(shell.Calendar);

        var states = Stack.New();
        states.SetTransitionType(StackTransitionType.Crossfade);
        states.AddNamed(Spinner(48), "starting");
        states.AddNamed(new WelcomeView(shell).Root, "signedout");
        states.AddNamed(SignedIn(shell, courses.Root, Calendar.Root, new NotificationsView(shell.Notifications).Root,
            (Messages = new MessagesView(shell.Messages)).Root, new McpView(shell.Mcp).Root), "signedin");
        Bind.Property(shell, () => states.SetVisibleChildName(shell.State switch
        {
            ShellState.SignedIn => "signedin",
            ShellState.SignedOut => "signedout",
            _ => "starting",
        }), nameof(ShellViewModel.State));

        Window.SetContent(states);
        _ = new LoginDialog(shell.Login, Window);
    }

    private static Widget SignedIn(ShellViewModel shell, Widget coursesPage, Widget calendarPage, Widget notificationsPage, Widget messagesPage, Widget mcpPage)
    {
        // Sidebar: navigation list + account area.
        var nav = ListBox.New();
        nav.AddCssClass("navigation-sidebar");
        var pages = shell.NavigationItems.ToList();
        foreach (var item in pages)
        {
            var icon = Image.NewFromIconName(Icons.Name(item.Icon));
            var title = Label(item.Title);
            title.SetHexpand(true);
            var line = HBox(12, icon, title);
            // Unread counts from itslearning, hidden when there's nothing unread.
            if (item.Page == AppPage.Notifications)
                line.Append(Badge(shell.Notifications, () => shell.Notifications.UnreadText, nameof(NotificationsViewModel.UnreadCount)));
            if (item.Page == AppPage.Messages)
                line.Append(Badge(shell.Messages, () => shell.Messages.UnreadText, nameof(MessagesViewModel.UnreadCount)));
            var row = ListBoxRow.New();
            row.SetChild(line);
            nav.Append(row);
        }
        nav.OnRowSelected += (_, e) =>
        {
            if (e.Row is { } row) shell.CurrentPage = pages[row.GetIndex()].Page;
        };

        var account = Label("", "caption", "dim-label");
        Bind.Text(account, shell, nameof(ShellViewModel.AccountText), () => shell.AccountText);
        var signOut = Button.New();
        signOut.SetChild(HBox(8, Image.NewFromIconName(Icons.Name(Core.Presentation.AppIcon.SignOut)), Label(Strings.SignOut)));
        signOut.AddCssClass("flat");
        Bind.Command(signOut, shell.SignOutCommand);
        var accountBox = VBox(6, account, signOut);
        accountBox.SetMarginStart(12);
        accountBox.SetMarginEnd(12);
        accountBox.SetMarginBottom(12);

        var sidebarToolbar = Adw.ToolbarView.New();
        sidebarToolbar.AddTopBar(Adw.HeaderBar.New());
        sidebarToolbar.SetContent(Scrolled(nav));
        sidebarToolbar.AddBottomBar(accountBox);
        var sidebar = Adw.NavigationPage.New(sidebarToolbar, Strings.AppName);

        // Content: one page per navigation item, each with its own header bar.
        var content = Stack.New();
        content.SetTransitionType(StackTransitionType.Crossfade);
        content.AddNamed(coursesPage, nameof(AppPage.Courses));
        content.AddNamed(calendarPage, nameof(AppPage.Calendar));
        content.AddNamed(notificationsPage, nameof(AppPage.Notifications));
        content.AddNamed(messagesPage, nameof(AppPage.Messages));
        content.AddNamed(mcpPage, nameof(AppPage.Mcp));
        var contentPage = Adw.NavigationPage.New(content, "");
        Bind.Property(shell, () =>
        {
            content.SetVisibleChildName(shell.CurrentPage.ToString());
            var index = pages.FindIndex(p => p.Page == shell.CurrentPage);
            if (nav.GetSelectedRow()?.GetIndex() != index) nav.SelectRow(nav.GetRowAtIndex(index));
            contentPage.SetTitle(pages[index].Title);
        }, nameof(ShellViewModel.CurrentPage));

        var split = Adw.NavigationSplitView.New();
        split.SetSidebar(sidebar);
        split.SetContent(contentPage);
        split.SetMinSidebarWidth(200);
        split.SetMaxSidebarWidth(260);
        return split;
    }

    private static Label Badge(System.ComponentModel.INotifyPropertyChanged source, Func<string> text, string property)
    {
        var badge = Label("", "caption-heading", "accent", "numeric");
        Bind.Property(source, () =>
        {
            badge.SetText(text());
            badge.SetVisible(text().Length > 0);
        }, property);
        return badge;
    }
}
