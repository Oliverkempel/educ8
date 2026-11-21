using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>Start page: courses as a grid of cards or a list of collapsible rows. Header bar: semester filter, search, view, refresh.</summary>
internal sealed class CoursesView
{
    public Widget Root { get; }

    /// <summary>The navigation stack (grid → course → folders); exposed for the screenshot aid.</summary>
    public Adw.NavigationView Navigation { get; }

    public CoursesView(CoursesViewModel vm)
    {
        // Header bar
        var current = ToggleButton.NewWithLabel(Strings.FilterCurrentSemester);
        var all = ToggleButton.NewWithLabel(Strings.FilterAllCourses);
        all.SetGroup(current);
        current.SetActive(!vm.ShowAll);
        all.OnToggled += (_, _) => vm.ShowAll = all.GetActive();
        var filter = HBox(0, current, all);
        filter.AddCssClass("linked");

        var search = SearchEntry.New();
        search.SetPlaceholderText(Strings.SearchCourses);
        search.SetSizeRequest(320, -1);
        search.OnSearchChanged += (_, _) => vm.SearchText = search.GetText();

        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);

        // Grid of cards or list of collapsible rows (stock linked icon toggles, like Files' view switch).
        var gridView = ToggleButton.New();
        gridView.SetIconName("view-grid-symbolic");
        gridView.SetTooltipText(Strings.CoursesGridView);
        var listView = ToggleButton.New();
        listView.SetIconName("view-list-symbolic");
        listView.SetTooltipText(Strings.CoursesListView);
        listView.SetGroup(gridView);
        var views = HBox(0, gridView, listView);
        views.AddCssClass("linked");

        var header = Adw.HeaderBar.New();
        header.PackStart(filter);
        header.SetTitleWidget(search);
        header.PackEnd(refresh);
        header.PackEnd(views);

        // Content
        var count = Label("", "dim-label", "caption");
        Bind.Text(count, vm, nameof(CoursesViewModel.CountText), () => vm.CountText);

        // Grid: cards with the course's details; clicking one opens the course.
        var grid = FlowBox.New();
        grid.SetSelectionMode(SelectionMode.None);
        grid.SetHomogeneous(true);
        grid.SetMinChildrenPerLine(1);
        grid.SetMaxChildrenPerLine(4);
        grid.SetColumnSpacing(12);
        grid.SetRowSpacing(12);
        grid.SetActivateOnSingleClick(true);
        grid.OnChildActivated += (_, e) => vm.OpenCourseCommand.Execute(vm.Courses[e.Child.GetIndex()]);

        // List: one collapsible row per course. One row per view model, reused while searching and filtering (the view
        // models are kept too, so their bindings aren't made twice); new view models after a reload get new rows.
        var rows = new System.Runtime.CompilerServices.ConditionalWeakTable<CourseCardViewModel, ListBoxRow>();
        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetValign(Align.Start);

        // Only the shown layout is built; switching builds the other from the same view models.
        void Rebuild()
        {
            grid.RemoveAll();
            list.RemoveAll();
            if (vm.Layout == CoursesLayout.Grid)
                foreach (var card in vm.Courses) grid.Append(Card(card));
            else
                foreach (var course in vm.Courses) list.Append(rows.GetValue(course, c => ListRow(c, vm)));
        }
        Bind.Collection(vm.Courses, Rebuild);

        var layouts = Stack.New();
        layouts.SetVhomogeneous(false);
        layouts.AddNamed(grid, "grid");
        layouts.AddNamed(list, "list"); // same width as the grid, aligned with the course count
        void ShowLayout()
        {
            layouts.SetVisibleChildName(vm.Layout == CoursesLayout.Grid ? "grid" : "list");
            gridView.SetActive(vm.Layout == CoursesLayout.Grid);
            listView.SetActive(vm.Layout == CoursesLayout.List);
            Rebuild();
        }
        Bind.Property(vm, ShowLayout, nameof(CoursesViewModel.Layout));
        gridView.OnToggled += (_, _) => { if (gridView.GetActive()) vm.Layout = CoursesLayout.Grid; };
        listView.OnToggled += (_, _) => { if (listView.GetActive()) vm.Layout = CoursesLayout.List; };

        var page = VBox(12, count, layouts);
        page.SetMarginTop(18);
        page.SetMarginBottom(24);
        page.SetMarginStart(18);
        page.SetMarginEnd(18);

        var empty = Adw.StatusPage.New();
        empty.SetIconName(Icons.Name(AppIcon.Search));
        Bind.Property(vm, () => empty.SetTitle(vm.EmptyText ?? ""), nameof(CoursesViewModel.EmptyText));

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(empty, "empty");
        states.AddNamed(Scrolled(Clamp(page, 1400)), "list");
        Bind.Property(vm, () => states.SetVisibleChildName(
                vm.IsLoading && vm.Courses.Count == 0 ? "loading" : vm.IsEmpty ? "empty" : "list"),
            nameof(CoursesViewModel.IsLoading), nameof(CoursesViewModel.EmptyText));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(VBox(0, Bind.ErrorBanner(vm, nameof(CoursesViewModel.ErrorMessage), () => vm.ErrorMessage, vm.RefreshCommand), states));

        // Course pages (and their folders) are pushed on top of the grid, with the standard back button.
        var navigation = Adw.NavigationView.New();
        navigation.Add(Adw.NavigationPage.New(toolbar, Strings.CoursesTitle));
        vm.CourseOpened += (_, course) => navigation.Push(CoursePage.Create(course, navigation.Push));
        vm.PlanOpened += (_, plan) => navigation.Push(PlanPage.Create(plan, navigation.Push));
        Navigation = navigation;
        Root = navigation;
    }

    /// <summary>A grid card: name, code and semester, teachers, last activity and badges. Clicking it opens the course.</summary>
    private static Widget Card(CourseCardViewModel c)
    {
        var title = WrapLabel(c.Title, "heading");
        title.SetLines(2);
        title.SetEllipsize(Pango.EllipsizeMode.End);
        // Wrapping labels report their whole text as natural width; cap it so the homogeneous grid fits several cards per row.
        title.SetMaxWidthChars(28);

        var meta = Label(string.Join(" · ", new[] { c.Code, c.SemesterText }.Where(s => s.Length > 0)), "caption", "dim-label");
        var teachers = WrapLabel(c.TeachersText, "caption");
        teachers.SetVisible(c.TeachersText.Length > 0);
        teachers.SetLines(2);
        teachers.SetEllipsize(Pango.EllipsizeMode.End);
        teachers.SetMaxWidthChars(28);
        meta.SetMaxWidthChars(28);
        meta.SetEllipsize(Pango.EllipsizeMode.End);

        var activity = Label(c.LastActivityText, "caption", "dim-label");
        activity.SetHexpand(true);
        var badge = Label(c.BadgeText, "caption", "accent");
        badge.SetVisible(c.HasBadges);
        badge.SetXalign(1);

        var spacer = Box.New(Orientation.Vertical, 0);
        spacer.SetVexpand(true);
        var content = VBox(6, title, meta, teachers, spacer, HBox(8, activity, badge));
        content.SetMarginTop(14);
        content.SetMarginBottom(14);
        content.SetMarginStart(14);
        content.SetMarginEnd(14);

        // The FlowBoxChild itself is the card, so the focus ring and hover/press states follow the card's rounded corners.
        var card = FlowBoxChild.New();
        card.SetChild(content);
        card.AddCssClass("card");
        card.AddCssClass("activatable");
        card.SetSizeRequest(240, 150);
        card.SetTooltipText(Strings.CourseOpenHint);
        return card;
    }

    /// <summary>
    /// A course in the list. Clicking the row opens the course; the fold-out button shows the current plans (click one to open
    /// it) and the teachers. <c>Adw.ExpanderRow</c> can't do this (its whole header toggles), so the row is built from the same
    /// parts and stock style classes it uses: a header row, and a revealer with a nested list.
    /// </summary>
    private static ListBoxRow ListRow(CourseCardViewModel c, CoursesViewModel vm)
    {
        var header = Row(c.Title);
        header.SetTitleLines(2);
        header.AddCssClass("header");
        header.SetActivatable(true);
        header.SetTooltipText(Strings.CourseOpenHint);
        header.OnActivated += (_, _) => vm.OpenCourseCommand.Execute(c);

        var toggle = Button.New();
        toggle.AddCssClass("flat");
        toggle.AddCssClass("circular");
        toggle.SetValign(Align.Center);
        toggle.SetTooltipText(Strings.CourseShowDetails);
        header.AddSuffix(toggle);

        var headerList = ListBox.New();
        headerList.SetSelectionMode(SelectionMode.None);
        headerList.Append(header);

        var nested = ListBox.New();
        nested.AddCssClass("nested");
        nested.SetSelectionMode(SelectionMode.None);
        var revealer = Revealer.New();
        revealer.SetTransitionType(RevealerTransitionType.SlideDown);
        revealer.SetChild(nested);

        void ShowExpanded()
        {
            toggle.SetIconName(c.IsExpanded ? "pan-up-symbolic" : "pan-down-symbolic");
            revealer.SetRevealChild(c.IsExpanded);
        }
        Bind.Property(c, ShowExpanded, nameof(CourseCardViewModel.IsExpanded));
        toggle.OnClicked += (_, _) => c.IsExpanded = !c.IsExpanded;

        // Current plans (loading/empty text as the heading's subtitle), then one row per plan, then the teachers.
        var plansHeading = Row(c.ActivePlans.Title, "");
        plansHeading.AddCssClass("property");
        Adw.ActionRow? teachers = c.HasTeachers ? Row(Strings.CourseTeachers, c.TeacherNames) : null;
        teachers?.AddCssClass("property");
        void RebuildDetails()
        {
            nested.RemoveAll();
            plansHeading.SetSubtitle(c.ActivePlans.IsLoading && c.ActivePlans.Items.Count == 0 ? Strings.Loading : c.ActivePlans.EmptyText ?? "");
            nested.Append(plansHeading);
            foreach (var plan in c.ActivePlans.Items)
            {
                var row = Row(plan.Name, plan.WhenText);
                row.AddPrefix(Image.NewFromIconName(Icons.Name(AppIcon.LearningPath)));
                row.AddSuffix(Image.NewFromIconName("go-next-symbolic"));
                row.SetActivatable(true);
                row.OnActivated += (_, _) => c.OpenPlanCommand.Execute(plan);
                nested.Append(row);
            }
            if (teachers is not null) nested.Append(teachers);
        }
        Bind.Collection(c.ActivePlans.Items, RebuildDetails);
        Bind.Property(c.ActivePlans, RebuildDetails, nameof(CourseSection<PlanItemViewModel>.IsLoading), nameof(CourseSection<PlanItemViewModel>.EmptyText));

        var course = ListBoxRow.New();
        course.AddCssClass("expander");
        course.SetActivatable(false);
        course.SetSelectable(false);
        course.SetChild(VBox(0, headerList, revealer));
        return course;
    }
}
