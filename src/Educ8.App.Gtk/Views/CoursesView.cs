using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>Start page: course cards in a responsive grid. Header bar: semester filter, search, refresh.</summary>
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

        var header = Adw.HeaderBar.New();
        header.PackStart(filter);
        header.SetTitleWidget(search);
        header.PackEnd(refresh);

        // Content
        var count = Label("", "dim-label", "caption");
        Bind.Text(count, vm, nameof(CoursesViewModel.CountText), () => vm.CountText);

        var grid = FlowBox.New();
        grid.SetSelectionMode(SelectionMode.None);
        grid.SetHomogeneous(true);
        grid.SetMinChildrenPerLine(1);
        grid.SetMaxChildrenPerLine(4);
        grid.SetColumnSpacing(12);
        grid.SetRowSpacing(12);
        grid.SetActivateOnSingleClick(true);
        grid.OnChildActivated += (_, e) => vm.OpenCourseCommand.Execute(vm.Courses[e.Child.GetIndex()]);
        Bind.Collection(vm.Courses, () =>
        {
            grid.RemoveAll();
            foreach (var card in vm.Courses) grid.Append(Card(card));
        });

        var list = VBox(12, count, grid);
        list.SetMarginTop(18);
        list.SetMarginBottom(24);
        list.SetMarginStart(18);
        list.SetMarginEnd(18);

        var empty = Adw.StatusPage.New();
        empty.SetIconName(Icons.Name(AppIcon.Search));
        Bind.Property(vm, () => empty.SetTitle(vm.EmptyText ?? ""), nameof(CoursesViewModel.EmptyText));

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(empty, "empty");
        states.AddNamed(Scrolled(Clamp(list, 1400)), "list");
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
        Navigation = navigation;
        Root = navigation;
    }

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
}
