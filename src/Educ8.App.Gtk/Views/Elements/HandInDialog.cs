using Educ8.App.Core.Elements;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views.Elements;

/// <summary>
/// The hand-in form as a dialog: answer text, files, Review. Review shows a stock alert with exactly what will be sent; only its
/// "Hand in" answer submits. The dialog can't be closed while a hand-in is running.
/// </summary>
internal static class HandInDialog
{
    public static void Present(HandInViewModel form, Widget parent)
    {
        // Answer text
        var text = TextView.New();
        text.SetWrapMode(WrapMode.WordChar);
        text.SetTopMargin(10);
        text.SetBottomMargin(10);
        text.SetLeftMargin(12);
        text.SetRightMargin(12);
        text.GetBuffer().OnChanged += (_, _) =>
        {
            var buffer = text.GetBuffer();
            buffer.GetBounds(out var start, out var end);
            form.Draft = buffer.GetText(start, end, false);
        };
        var textScroll = ScrolledWindow.New();
        textScroll.SetChild(text);
        textScroll.SetMinContentHeight(160);
        textScroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        var textFrame = Frame.New(null);
        textFrame.SetChild(textScroll);
        var answer = Adw.PreferencesGroup.New();
        answer.SetTitle(Escape(Strings.HandInAnswer));
        answer.Add(textFrame);

        // Files
        var files = ListBox.New();
        files.AddCssClass("boxed-list");
        files.SetSelectionMode(SelectionMode.None);
        Bind.Collection(form.Files, () =>
        {
            files.RemoveAll();
            foreach (var f in form.Files)
            {
                var row = Row(f.Name, f.SizeText);
                row.AddPrefix(Image.NewFromIconName(Icons.Name(AppIcon.File)));
                var remove = IconButton("list-remove-symbolic", Strings.HandInRemoveFile);
                remove.AddCssClass("flat");
                remove.SetValign(Align.Center);
                Bind.Command(remove, f.RemoveCommand);
                row.AddSuffix(remove);
                files.Append(row);
            }
            files.SetVisible(form.Files.Count > 0);
        });
        var add = Button.NewWithLabel(Strings.HandInAddFiles);
        add.SetHalign(Align.Start);
        Bind.Command(add, form.AddFilesCommand);
        var filesGroup = Adw.PreferencesGroup.New();
        filesGroup.SetTitle(Escape(Strings.HandInFiles));
        filesGroup.Add(VBox(12, files, add));

        // Warning, problems and progress
        var warning = WrapLabel(form.WarningText ?? "", "warning");
        warning.SetVisible(form.WarningText is not null);
        var error = WrapLabel("", "error");
        Bind.Property(form, () => { error.SetText(form.ErrorMessage ?? ""); error.SetVisible(form.ErrorMessage is not null); },
            nameof(HandInViewModel.ErrorMessage));
        var progressText = Label("");
        var cancelUpload = Button.NewWithLabel(Strings.HandInCancel);
        Bind.Command(cancelUpload, form.SubmitCancelCommand);
        var progress = HBox(12, Spinner(16), progressText, cancelUpload);
        Bind.Property(form, () =>
        {
            progressText.SetText(form.StatusText ?? "");
            progress.SetVisible(form.IsSubmitting);
            text.SetEditable(!form.IsSubmitting);
        }, nameof(HandInViewModel.StatusText), nameof(HandInViewModel.IsSubmitting));
        var notes = Adw.PreferencesGroup.New();
        notes.Add(VBox(12, warning, error, progress));

        var content = Adw.PreferencesPage.New();
        content.Add(answer);
        content.Add(filesGroup);
        content.Add(notes);

        var review = Button.NewWithLabel(Strings.HandInReview);
        review.AddCssClass("suggested-action");
        Bind.Command(review, form.ReviewCommand);
        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(Adw.WindowTitle.New(form.Title, ""));
        header.PackEnd(review);

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(content);

        var dialog = Adw.Dialog.New();
        dialog.SetContentWidth(560);
        dialog.SetContentHeight(620);
        dialog.SetChild(toolbar);
        Bind.Property(form, () => dialog.SetCanClose(!form.IsSubmitting), nameof(HandInViewModel.IsSubmitting));

        form.ConfirmationRequested += (_, confirmation) =>
        {
            var alert = Adw.AlertDialog.New(confirmation.Title, confirmation.Body);
            alert.AddResponse("cancel", confirmation.CancelText);
            alert.AddResponse("hand-in", confirmation.ConfirmText);
            alert.SetResponseAppearance("hand-in", Adw.ResponseAppearance.Suggested);
            alert.SetDefaultResponse("cancel");
            alert.SetCloseResponse("cancel");
            alert.OnResponse += (_, args) =>
            {
                if (args.Response == "hand-in") form.SubmitCommand.Execute(confirmation);
            };
            alert.Present(dialog);
        };
        form.Submitted += (_, _) =>
        {
            dialog.SetCanClose(true);
            dialog.ForceClose();
        };
        dialog.Present(parent);
    }
}
