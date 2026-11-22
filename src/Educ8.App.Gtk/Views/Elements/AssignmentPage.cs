using Educ8.App.Core.Elements;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views.Elements;

/// <summary>
/// An assignment: deadline and hand-in button (or why it's off), facts, description, files, the student's answer, assessment and
/// feedback, grading scale. Rebuilt whenever the assignment (re)loads.
/// </summary>
internal static class AssignmentPage
{
    public static Adw.NavigationPage Create(AssignmentViewModel vm)
    {
        var holder = VBox(0);
        holder.SetVexpand(true);
        void Rebuild()
        {
            while (holder.GetFirstChild() is { } child) holder.Remove(child);
            if (vm.Info is not null) holder.Append(Content(vm));
        }
        Bind.Property(vm, Rebuild, nameof(AssignmentViewModel.Info));

        var toasts = Adw.ToastOverlay.New();
        toasts.SetChild(holder);
        vm.HandedIn += (_, _) => toasts.AddToast(Adw.Toast.New(Strings.HandInDone));
        vm.HandInStarted += (_, form) => HandInDialog.Present(form, holder);
        return ElementPages.Scaffold(vm, toasts);
    }

    private static Widget Content(AssignmentViewModel vm)
    {
        var page = Adw.PreferencesPage.New();

        // Deadline and hand-in.
        var top = Adw.PreferencesGroup.New();
        top.SetTitle(Escape(vm.DeadlineText));
        top.SetDescription(Escape(vm.CourseTitle));
        if (vm.CanHandIn)
        {
            var handIn = Button.NewWithLabel(Strings.HandIn);
            handIn.AddCssClass("suggested-action");
            handIn.AddCssClass("pill");
            handIn.SetHalign(Align.Start);
            Bind.Command(handIn, vm.StartHandInCommand);
            top.Add(handIn);
        }
        else if (vm.HandInDisabledText is { } off)
        {
            top.Add(WrapLabel(off, "dim-label"));
        }
        page.Add(top);

        if (vm.Facts.Count > 0)
        {
            var facts = Group(null);
            foreach (var f in vm.Facts) facts.Add(Property(f.Label, f.Value));
            page.Add(facts);
        }

        if (!vm.Description.IsEmpty)
        {
            var description = Group(Strings.AssignmentDescription);
            description.Add(Rich(vm, vm.Description));
            page.Add(description);
        }

        if (vm.TeacherFiles.Count > 0) page.Add(Files(Strings.AssignmentFiles, vm.TeacherFiles));

        var answer = Group(Strings.AssignmentYourAnswer);
        answer.SetDescription(Escape(vm.SubmissionText));
        if (!vm.AnswerText.IsEmpty) answer.Add(Rich(vm, vm.AnswerText));
        if (vm.AnswerFiles.Count > 0) answer.Add(FileList(vm.AnswerFiles));
        page.Add(answer);

        var assessment = Group(Strings.AssignmentAssessment);
        if (vm.ViewUnavailableText is { } unavailable) assessment.SetDescription(Escape(unavailable));
        else if (vm.NotAssessedText is { } notAssessed) assessment.SetDescription(Escape(notAssessed));
        var rows = new List<Widget>();
        if (vm.StatusText is { } status) rows.Add(Property(Strings.AssignmentStatus, status));
        if (vm.ScoreText is { } score) rows.Add(Property(Strings.AssignmentScore, score));
        if (rows.Count > 0 || vm.AssessedByText is not null)
        {
            var list = BoxedList();
            foreach (var r in rows) list.Append(r);
            if (vm.AssessedByText is { } by) list.Append(Row(by));
            assessment.Add(list);
        }
        if (!vm.Feedback.IsEmpty) assessment.Add(Rich(vm, vm.Feedback));
        if (vm.FeedbackFiles.Count > 0) assessment.Add(FileList(vm.FeedbackFiles));
        page.Add(assessment);

        if (vm.GradingScale.Count > 0)
        {
            var scale = Group(Strings.AssignmentGradingScale);
            foreach (var grade in vm.GradingScale) scale.Add(Row(grade));
            page.Add(scale);
        }
        return page;
    }

    private static Adw.PreferencesGroup Group(string? title)
    {
        var group = Adw.PreferencesGroup.New();
        if (title is not null) group.SetTitle(Escape(title));
        return group;
    }

    private static Adw.ActionRow Property(string label, string value)
    {
        var row = Row(label, value);
        row.AddCssClass("property");
        return row;
    }

    private static Widget Rich(AssignmentViewModel vm, Documents.RichDocument document)
    {
        var text = RichTextView.Create(document, url => vm.OpenLinkCommand.Execute(url), () => vm.OpenInItslearningCommand.Execute(null));
        text.SetMarginBottom(6);
        return text;
    }

    private static Adw.PreferencesGroup Files(string title, IReadOnlyList<AssignmentFileItemViewModel> files)
    {
        var group = Group(title);
        group.Add(FileList(files));
        return group;
    }

    private static ListBox BoxedList()
    {
        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        return list;
    }

    /// <summary>Files that open in the default app, with a spinner and cancel while downloading (like course files).</summary>
    private static ListBox FileList(IReadOnlyList<AssignmentFileItemViewModel> files)
    {
        var list = BoxedList();
        foreach (var f in files)
        {
            var row = Row(f.Name, f.DetailText);
            row.AddPrefix(Image.NewFromIconName(Icons.Name(AppIcon.File)));
            var open = Image.NewFromIconName(Icons.Name(AppIcon.OpenExternal));
            var progress = RowProgress.Attach(row);
            var cancel = Button.NewFromIconName(Icons.Name(AppIcon.CancelDownload));
            cancel.AddCssClass("flat");
            cancel.AddCssClass("circular");
            cancel.SetValign(Align.Center);
            cancel.SetTooltipText(Strings.CancelDownload);
            cancel.OnClicked += (_, _) =>
            {
                if (f.OpenCommand.IsRunning) f.OpenCancelCommand.Execute(null);
                if (f.SaveCopyCommand.IsRunning) f.SaveCopyCancelCommand.Execute(null);
            };
            row.AddSuffix(open);
            ContextMenu.Attach(row, (Strings.OpenFile, f.OpenCommand), (Strings.SaveCopy, f.SaveCopyCommand));
            row.AddSuffix(cancel);
            row.SetActivatable(true);
            row.SetTooltipText(Strings.OpenFile);
            row.OnActivated += (_, _) => { if (!f.OpenCommand.IsRunning) f.OpenCommand.Execute(null); };
            void ShowState()
            {
                var running = f.OpenCommand.IsRunning || f.SaveCopyCommand.IsRunning;
                progress.Show(running, f.DownloadFraction);
                open.SetVisible(!running);
                cancel.SetVisible(running);
                row.SetSubtitle(running ? Strings.Downloading : f.ErrorMessage ?? f.NoticeText ?? f.DetailText);
            }
            Bind.Property(f.OpenCommand, ShowState, nameof(f.OpenCommand.IsRunning));
            Bind.Property(f.SaveCopyCommand, ShowState, nameof(f.SaveCopyCommand.IsRunning));
            Bind.Property(f, ShowState, nameof(AssignmentFileItemViewModel.ErrorMessage), nameof(AssignmentFileItemViewModel.NoticeText),
                nameof(AssignmentFileItemViewModel.DownloadFraction));
            list.Append(row);
        }
        return list;
    }
}
