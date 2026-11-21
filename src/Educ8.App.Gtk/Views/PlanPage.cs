using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>A lesson plan on its own page (from a course card): when, topic, description and material. Header: open the course.</summary>
internal static class PlanPage
{
    public static Adw.NavigationPage Create(PlanPageViewModel vm, Action<Adw.NavigationPage> push)
    {
        var plan = vm.Plan;
        vm.FolderOpened += (_, folder) => push(CoursePage.Folder(folder, push));

        var title = WrapLabel(plan.Name, "title-1");
        var when = Label(plan.WhenText, "dim-label");
        when.SetVisible(plan.WhenText.Length > 0);
        var topic = Label(plan.TopicText, "dim-label");
        topic.SetVisible(plan.TopicText.Length > 0);
        var heading = VBox(6, title, when, topic);

        var about = Adw.PreferencesGroup.New();
        about.Add(heading);
        if (plan.DescriptionText.Length > 0)
        {
            var description = WrapLabel(plan.DescriptionText);
            description.SetSelectable(true);
            description.SetMarginTop(12);
            about.Add(description);
        }

        var material = Adw.PreferencesGroup.New();
        material.SetTitle(Escape(Strings.EventMaterial));
        if (plan.EmptyText is not null) material.Add(Label(plan.EmptyText, "dim-label"));
        else
        {
            var list = ListBox.New();
            list.AddCssClass("boxed-list");
            list.SetSelectionMode(SelectionMode.None);
            foreach (var m in plan.Material) list.Append(MaterialRows.Row(m, () => vm.OpenItemCommand.Execute(m)));
            material.Add(list);
        }

        var content = Adw.PreferencesPage.New();
        content.Add(about);
        content.Add(material);

        var openCourse = Button.NewWithLabel(Strings.PlanOpenCourse);
        openCourse.SetTooltipText(vm.CourseTitle);
        Bind.Command(openCourse, vm.OpenCourseCommand);
        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(Adw.WindowTitle.New(plan.Name, vm.CourseTitle));
        header.PackEnd(openCourse);

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(content);
        return Adw.NavigationPage.New(toolbar, plan.Name);
    }
}
