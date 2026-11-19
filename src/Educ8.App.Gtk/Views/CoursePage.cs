using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// A course, pushed onto the courses navigation view. Header: back (automatic), Overview/Resources switcher, open in itslearning,
/// refresh. Folders open as further pages.
/// </summary>
internal static class CoursePage
{
    public static Adw.NavigationPage Create(CourseViewModel vm, Action<Adw.NavigationPage> push)
    {
        var stack = Adw.ViewStack.New();
        // Material rows on this page go through the course: folders open as pages, the rest opens as usual.
        vm.FolderOpened += (_, folder) => push(Folder(folder, push));
        Func<MaterialItemViewModel, Widget> material = m => MaterialRows.Row(m, () => vm.OpenItemCommand.Execute(m));
        stack.AddTitledWithIcon(Overview(vm, material), "overview", vm.OverviewTitle, "view-list-symbolic");
        stack.AddTitledWithIcon(AllPlans(vm.AllPlans, material), "plans", vm.AllPlans.Title, Icons.Name(AppIcon.LearningPath));
        stack.AddTitledWithIcon(FolderContent(vm.Resources, push), "resources", vm.Resources.Title, Icons.Name(AppIcon.Folder));
        // Tabs other than the overview load the first time they're shown.
        stack.OnNotify += (sender, e) =>
        {
            if (e.Pspec.GetName() != "visible-child-name") return;
            switch (stack.GetVisibleChildName())
            {
                case "plans": _ = vm.AllPlans.EnsureLoadedAsync(); break;
                case "resources": _ = vm.Resources.EnsureLoadedAsync(); break;
            }
        };

        var switcher = Adw.ViewSwitcher.New();
        switcher.SetStack(stack);
        switcher.SetPolicy(Adw.ViewSwitcherPolicy.Wide);
        var narrowTitle = Adw.WindowTitle.New(vm.Title, "");
        narrowTitle.SetVisible(false);

        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);
        var web = IconButton(Icons.Name(AppIcon.OpenExternal), Strings.OpenInItslearning);
        Bind.Command(web, vm.OpenInItslearningCommand);

        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(HBox(0, switcher, narrowTitle));
        header.PackEnd(refresh);
        header.PackEnd(web);

        var bottom = Adw.ViewSwitcherBar.New();
        bottom.SetStack(stack);

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(stack);
        toolbar.AddBottomBar(bottom);

        // Narrow: the tabs move to a bar at the bottom and the header shows the course title (the stock libadwaita pattern).
        var narrow = Adw.Breakpoint.New(Adw.BreakpointCondition.Parse("max-width: 820px")!);
        narrow.OnApply += (_, _) => { switcher.SetVisible(false); narrowTitle.SetVisible(true); bottom.SetReveal(true); };
        narrow.OnUnapply += (_, _) => { switcher.SetVisible(true); narrowTitle.SetVisible(false); bottom.SetReveal(false); };
        var bin = Adw.BreakpointBin.New();
        bin.SetSizeRequest(360, 300);
        bin.SetChild(toolbar);
        bin.AddBreakpoint(narrow);

        var page = Adw.NavigationPage.New(bin, vm.Title);
        _ = vm.EnsureLoadedAsync();
        return page;
    }

    /// <summary>A subfolder: its own page with the folder's name as title.</summary>
    public static Adw.NavigationPage Folder(FolderViewModel vm, Action<Adw.NavigationPage> push)
    {
        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);
        var header = Adw.HeaderBar.New();
        header.PackEnd(refresh);

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(FolderContent(vm, push));

        var page = Adw.NavigationPage.New(toolbar, vm.Title);
        _ = vm.EnsureLoadedAsync();
        return page;
    }

    private static Widget Overview(CourseViewModel vm, Func<MaterialItemViewModel, Widget> material)
    {
        var title = WrapLabel(vm.Title, "title-1");
        var code = Label(vm.Subtitle, "dim-label");
        code.SetVisible(vm.Subtitle.Length > 0);

        // Announcements can be long: a one-paragraph preview as the row (title lines count per paragraph, so the text itself
        // would be cut in every paragraph), the whole text once when expanded.
        var announcements = Section(vm.Announcements, a =>
        {
            var row = Adw.ExpanderRow.New();
            row.SetUseMarkup(false);
            row.SetTitle(a.PreviewText);
            row.SetTitleLines(2);
            row.SetSubtitle(a.MetaText);
            if (a.IsPinned)
            {
                var pin = Image.NewFromIconName(Icons.Name(AppIcon.Pinned));
                pin.SetTooltipText(Strings.Pinned);
                row.AddPrefix(pin);
            }
            var text = WrapLabel(a.Text);
            text.SetSelectable(true);
            text.SetMarginTop(12);
            text.SetMarginBottom(12);
            text.SetMarginStart(12);
            text.SetMarginEnd(12);
            row.AddRow(text);
            return row;
        });
        var plans = Section(vm.Plans, p => PlanRow(p, material));
        var tasks = Section(vm.Tasks, material);
        var changes = Section(vm.Changes, material);

        var content = VBox(24, VBox(4, title, code), announcements, plans, tasks, changes);
        content.SetMarginTop(24);
        content.SetMarginBottom(24);
        content.SetMarginStart(18);
        content.SetMarginEnd(18);
        return Scrolled(Clamp(content, 900));
    }

    /// <summary>The Plans tab: every plan in course order, each an expander with its material.</summary>
    private static Widget AllPlans(CoursePlansViewModel vm, Func<MaterialItemViewModel, Widget> material)
    {
        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetValign(Align.Start);
        Bind.Collection(vm.Plans, () =>
        {
            list.RemoveAll();
            foreach (var plan in vm.Plans) list.Append(PlanRow(plan, material));
        });
        var groups = VBox(0, list);
        groups.SetMarginTop(24);
        groups.SetMarginBottom(24);
        groups.SetMarginStart(18);
        groups.SetMarginEnd(18);

        var message = Adw.StatusPage.New();
        message.SetIconName(Icons.Name(AppIcon.LearningPath));

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(message, "message");
        states.AddNamed(Scrolled(Clamp(groups, 900)), "list");
        Bind.Property(vm, () =>
        {
            message.SetTitle(vm.ErrorMessage ?? vm.EmptyText ?? "");
            states.SetVisibleChildName(vm.IsLoading && vm.Plans.Count == 0 ? "loading"
                : vm.ErrorMessage is not null || vm.EmptyText is not null ? "message" : "list");
        }, nameof(CoursePlansViewModel.IsLoading), nameof(CoursePlansViewModel.ErrorMessage), nameof(CoursePlansViewModel.EmptyText));
        return states;
    }

    private static Adw.ExpanderRow PlanRow(PlanItemViewModel p, Func<MaterialItemViewModel, Widget> material)
    {
        var row = Adw.ExpanderRow.New();
        row.SetUseMarkup(false);
        row.SetTitle(p.Name);
        row.SetSubtitle(p.WhenText);
        var description = string.Join("\n\n", new[] { p.TopicText, p.DescriptionText }.Where(s => s.Length > 0));
        if (description.Length > 0)
        {
            var text = WrapLabel(description, "dim-label");
            text.SetMarginTop(12);
            text.SetMarginBottom(12);
            text.SetMarginStart(12);
            text.SetMarginEnd(12);
            row.AddRow(text);
        }
        if (p.EmptyText is not null) row.AddRow(Row(p.EmptyText));
        foreach (var m in p.Material) row.AddRow(material(m));
        return row;
    }

    /// <summary>A titled group whose rows follow the section: items, or a dim line with the empty/error text, or a spinner.</summary>
    private static Adw.PreferencesGroup Section<T>(CourseSection<T> section, Func<T, Widget> row)
    {
        var group = Adw.PreferencesGroup.New();
        group.SetTitle(Escape(section.Title));
        var rows = new List<Widget>();
        void Rebuild()
        {
            foreach (var r in rows) group.Remove(r);
            rows.Clear();
            if (section.IsLoading && section.Items.Count == 0) rows.Add(Spinner(24));
            else if (section.EmptyText is { } empty) rows.Add(Label(empty, "dim-label"));
            else rows.AddRange(section.Items.Select(row));
            foreach (var r in rows) group.Add(r);
        }
        Bind.Property(section, Rebuild, nameof(CourseSection<T>.IsLoading), nameof(CourseSection<T>.EmptyText));
        Bind.Collection(section.Items, Rebuild);
        return group;
    }

    /// <summary>A folder's items as a boxed list, with loading, error and empty states. Folders push a new page.</summary>
    private static Widget FolderContent(FolderViewModel vm, Action<Adw.NavigationPage> push)
    {
        vm.FolderOpened += (_, child) => push(Folder(child, push));

        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetValign(Align.Start);
        Bind.Collection(vm.Items, () =>
        {
            list.RemoveAll();
            foreach (var item in vm.Items) list.Append(MaterialRows.Row(item, () => vm.OpenCommand.Execute(item)));
        });

        var listPage = VBox(0, list);
        listPage.SetMarginTop(24);
        listPage.SetMarginBottom(24);
        listPage.SetMarginStart(18);
        listPage.SetMarginEnd(18);

        var message = Adw.StatusPage.New();
        message.SetIconName(Icons.Name(AppIcon.Folder));

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(message, "message");
        states.AddNamed(Scrolled(Clamp(listPage, 900)), "list");
        Bind.Property(vm, () =>
        {
            message.SetTitle(vm.ErrorMessage ?? vm.EmptyText ?? "");
            states.SetVisibleChildName(vm.IsLoading && vm.Items.Count == 0 ? "loading"
                : vm.ErrorMessage is not null || vm.EmptyText is not null ? "message" : "list");
        }, nameof(FolderViewModel.IsLoading), nameof(FolderViewModel.ErrorMessage), nameof(FolderViewModel.EmptyText));
        return states;
    }
}
