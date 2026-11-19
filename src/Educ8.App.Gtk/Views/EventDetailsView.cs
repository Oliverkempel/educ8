using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>Side pane for the selected calendar event: when/where/course, lesson plan with material, linked material.</summary>
internal sealed class EventDetailsView
{
    public Widget Root { get; }

    public EventDetailsView(EventDetailsViewModel vm)
    {
        var hint = Adw.StatusPage.New();
        hint.SetIconName(Icons.Name(AppIcon.Event));
        hint.SetDescription(Escape(vm.HintText));
        hint.AddCssClass("compact");

        var details = VBox(18);
        details.SetMarginTop(18);
        details.SetMarginBottom(24);
        details.SetMarginStart(18);
        details.SetMarginEnd(18);

        var states = Stack.New();
        states.AddNamed(hint, "hint");
        states.AddNamed(Spinner(32), "loading");
        states.AddNamed(Scrolled(details), "details");

        void Render()
        {
            while (details.GetFirstChild() is { } child) details.Remove(child);
            if (vm.ErrorMessage is not null) details.Append(WrapLabel(vm.ErrorMessage, "error"));
            if (vm.HasEvent) Fill(details, vm);
            states.SetVisibleChildName(vm.HasEvent || vm.ErrorMessage is not null ? "details" : vm.IsLoading ? "loading" : "hint");
        }
        Bind.Property(vm, Render); // any property change
        Bind.Collection(vm.Plans, Render);
        Bind.Collection(vm.LinkedMaterial, Render);

        var toolbar = Adw.ToolbarView.New();
        toolbar.SetContent(states);
        toolbar.AddCssClass("view");
        Root = toolbar;
    }

    private static void Fill(Box box, EventDetailsViewModel vm)
    {
        box.Append(WrapLabel(vm.Title, "title-2"));

        var facts = Adw.PreferencesGroup.New();
        foreach (var (name, value) in new[] { (Strings.EventWhen, vm.WhenText), (Strings.EventWhere, vm.PlaceText), (Strings.EventCourse, vm.CourseTitle) })
        {
            if (value.Length == 0) continue;
            var row = Row(name, value);
            row.AddCssClass("property");
            row.SetSubtitleSelectable(true);
            facts.Add(row);
        }
        box.Append(facts);

        if (vm.DescriptionText.Length > 0) box.Append(WrapLabel(vm.DescriptionText));

        foreach (var plan in vm.Plans)
        {
            // Heading as wrapping labels: a PreferencesGroup title is one line and cuts long plan names.
            var section = VBox(6, WrapLabel($"{Strings.EventLessonPlan}: {plan.Name}", "heading"));
            var description = string.Join("\n\n", new[] { plan.TopicText, plan.DescriptionText }.Where(s => s.Length > 0));
            if (description.Length > 0) section.Append(WrapLabel(description, "dim-label"));
            var group = Adw.PreferencesGroup.New();
            if (plan.EmptyText is not null) group.Add(Row(plan.EmptyText));
            foreach (var m in plan.Material) group.Add(MaterialRows.Row(m));
            section.Append(group);
            box.Append(section);
        }

        if (vm.LinkedMaterial.Count > 0)
        {
            var linked = Adw.PreferencesGroup.New();
            linked.SetTitle(Escape(Strings.EventLinkedMaterial));
            foreach (var m in vm.LinkedMaterial) linked.Add(MaterialRows.Row(m));
            box.Append(linked);
        }

        var open = Button.NewWithLabel(Strings.OpenInItslearning);
        open.AddCssClass("pill");
        open.SetHalign(Align.Center);
        Bind.Command(open, vm.OpenInItslearningCommand);
        box.Append(open);
    }
}
