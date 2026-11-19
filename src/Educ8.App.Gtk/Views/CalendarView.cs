using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// Week agenda. Header bar: previous/today/next, week title, date picker, course filter, refresh.
/// Content: days with their events; the selected event's details in a side pane.
/// </summary>
internal sealed class CalendarView
{
    private readonly CalendarViewModel _vm;
    private bool _syncingFilter;
    private string[] _filterTitles = [];

    public Widget Root { get; }

    public CalendarView(CalendarViewModel vm)
    {
        _vm = vm;

        // Header bar: navigation
        var previous = IconButton(Icons.Name(AppIcon.PreviousWeek), Strings.PreviousWeek);
        Bind.Command(previous, vm.PreviousWeekCommand);
        var today = Button.NewWithLabel(Strings.Today);
        Bind.Command(today, vm.TodayCommand);
        var next = IconButton(Icons.Name(AppIcon.NextWeek), Strings.NextWeek);
        Bind.Command(next, vm.NextWeekCommand);
        var navigation = HBox(0, previous, today, next);
        navigation.AddCssClass("linked");

        var title = Adw.WindowTitle.New("", "");
        Bind.Property(vm, () =>
        {
            title.SetTitle(vm.WeekTitle);
            title.SetSubtitle(vm.WeekRangeText);
        }, nameof(CalendarViewModel.WeekStart));

        // Header bar: date picker, course filter, refresh
        var calendar = global::Gtk.Calendar.New();
        var popover = Popover.New();
        popover.SetChild(calendar);
        var pick = MenuButton.New();
        pick.SetIconName(Icons.Name(AppIcon.Calendar));
        pick.SetTooltipText(Strings.CalendarTitle);
        pick.SetPopover(popover);
        calendar.OnDaySelected += (_, _) =>
        {
            var d = calendar.GetDate();
            popover.Popdown();
            _ = vm.GoToDateCommand.ExecuteAsync(new DateOnly(d.GetYear(), d.GetMonth(), d.GetDayOfMonth()));
        };

        var filter = DropDown.NewFromStrings([]);
        filter.SetTooltipText(Strings.AllCourses);
        Bind.Collection(vm.CourseFilters, () => SyncFilter(filter));
        Bind.Property(vm, () => SyncFilter(filter), nameof(CalendarViewModel.SelectedCourseFilter));
        filter.OnNotify += (_, e) =>
        {
            if (_syncingFilter || e.Pspec.GetName() != "selected") return;
            var index = (int)filter.GetSelected();
            if (index >= 0 && index < vm.CourseFilters.Count) vm.SelectedCourseFilter = vm.CourseFilters[index];
        };

        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);

        var header = Adw.HeaderBar.New();
        header.PackStart(navigation);
        header.SetTitleWidget(title);
        header.PackEnd(refresh);
        header.PackEnd(pick);
        header.PackEnd(filter);

        // Agenda
        var days = VBox(18);
        days.SetMarginTop(18);
        days.SetMarginBottom(24);
        days.SetMarginStart(18);
        days.SetMarginEnd(18);
        Bind.Collection(vm.Days, () =>
        {
            while (days.GetFirstChild() is { } child) days.Remove(child);
            foreach (var day in vm.Days) days.Append(Day(day));
        });

        var empty = Adw.StatusPage.New();
        empty.SetIconName(Icons.Name(AppIcon.Calendar));
        Bind.Property(vm, () => empty.SetTitle(vm.EmptyText ?? ""), nameof(CalendarViewModel.EmptyText));

        var states = Stack.New();
        states.AddNamed(Spinner(48), "loading");
        states.AddNamed(empty, "empty");
        states.AddNamed(Scrolled(Clamp(days, 900)), "agenda");
        Bind.Property(vm, () => states.SetVisibleChildName(
                vm.IsLoading && vm.Days.Count == 0 ? "loading" : vm.EmptyText is not null ? "empty" : "agenda"),
            nameof(CalendarViewModel.IsLoading), nameof(CalendarViewModel.EmptyText));

        var agenda = VBox(0, Bind.ErrorBanner(vm, nameof(CalendarViewModel.ErrorMessage), () => vm.ErrorMessage, vm.RefreshCommand), states);

        // Details side pane
        var split = Adw.OverlaySplitView.New();
        split.SetContent(agenda);
        split.SetSidebar(new EventDetailsView(vm.Details).Root);
        split.SetSidebarPosition(PackType.End);
        split.SetSidebarWidthFraction(0.4);
        split.SetMinSidebarWidth(320);
        split.SetMaxSidebarWidth(520);
        split.SetShowSidebar(true);
        // Keep the date picker on the shown week.
        Bind.Property(vm, () => calendar.SelectDay(GLib.DateTime.NewLocal(vm.WeekStart.Year, vm.WeekStart.Month, vm.WeekStart.Day, 0, 0, 0)!),
            nameof(CalendarViewModel.WeekStart));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(split);
        Root = toolbar;
    }

    private Widget Day(CalendarDayViewModel day)
    {
        var heading = Label($"{day.DayName} · {day.DateText}", "heading");
        if (day.IsToday) heading.AddCssClass("accent");
        if (day.IsPast) heading.AddCssClass("dim-label");
        var box = VBox(8, heading);

        if (!day.HasEvents)
        {
            box.Append(Label(day.NoEventsText, "dim-label"));
            return box;
        }

        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        foreach (var e in day.Events)
        {
            var row = Row(e.Title, string.Join(" · ", new[] { e.Subtitle, e.CourseTitle }.Where(s => s.Length > 0)));
            var time = Label(e.TimeText, "numeric");
            time.SetSizeRequest(100, -1);
            row.AddPrefix(time);
            row.SetActivatable(true);
            row.OnActivated += (_, _) => _vm.SelectedEvent = e;
            list.Append(row);
        }
        box.Append(list);
        return box;
    }

    /// <summary>
    /// Mirrors the view model into the drop-down. Touch the model and selection only when they really differ: this runs while the
    /// drop-down is still handling the user's pick (selected → view model → here), and replacing its model mid-click crashes GTK.
    /// </summary>
    private void SyncFilter(DropDown filter)
    {
        _syncingFilter = true;
        try
        {
            var titles = _vm.CourseFilters.Select(f => f.Title).ToArray();
            if (!titles.SequenceEqual(_filterTitles))
            {
                _filterTitles = titles;
                filter.SetModel(StringList.New(titles));
            }
            var index = (uint)Math.Max(_vm.CourseFilters.IndexOf(_vm.SelectedCourseFilter), 0);
            if (filter.GetSelected() != index) filter.SetSelected(index);
        }
        finally
        {
            _syncingFilter = false;
        }
    }
}
