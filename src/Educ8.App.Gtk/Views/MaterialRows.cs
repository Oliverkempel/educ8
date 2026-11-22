using Educ8.App.Core.Elements;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Gtk.Ui;
using Educ8.App.Gtk.Services;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// The one row used for course material everywhere (event details, course overview, folders, plans). How it opens (and the
/// trailing icon) comes from the view model: in the app, in the default application, or in the browser.
/// </summary>
internal static class MaterialRows
{
    public static Adw.ActionRow Row(MaterialItemViewModel m)
    {
        var row = Widgets.Row(m.Title, m.SubtitleText);
        row.AddPrefix(Image.NewFromIconName(Icons.Name(m.Icon)));
        var open = Image.NewFromIconName(Icons.Name(m.OpenIcon));
        // While it downloads: a progress line along the bottom, and the open icon becomes a stop button that cancels it.
        var progress = RowProgress.Attach(row);
        var cancel = Button.NewFromIconName(Icons.Name(AppIcon.CancelDownload));
        cancel.AddCssClass("flat");
        cancel.AddCssClass("circular");
        cancel.SetValign(Align.Center);
        cancel.SetTooltipText(Strings.CancelDownload);
        cancel.SetVisible(false);
        cancel.OnClicked += (_, _) =>
        {
            if (m.OpenCommand.IsRunning) m.OpenCancelCommand.Execute(null);
            if (m.SaveCopyCommand.IsRunning) m.SaveCopyCancelCommand.Execute(null);
        };
        row.AddSuffix(open);
        row.AddSuffix(cancel);
        row.SetActivatable(m.CanOpen);
        row.SetTooltipText(m.OpenTooltip);
        row.OnActivated += (_, _) =>
        {
            Launch.Debug($"row activated: {m.Kind}, style={m.OpenStyle}, url={Launch.Describe(m.Url)}");
            if (!m.OpenCommand.IsRunning) m.OpenCommand.Execute(null);
        };
        void ShowState()
        {
            var running = m.OpenCommand.IsRunning || m.SaveCopyCommand.IsRunning;
            progress.Show(running, m.DownloadFraction);
            open.SetVisible(!running);
            cancel.SetVisible(running);
            row.SetSubtitle(running ? m.BusyText : m.ErrorMessage ?? m.NoticeText ?? m.SubtitleText);
        }
        Bind.Property(m.OpenCommand, ShowState, nameof(m.OpenCommand.IsRunning));
        Bind.Property(m.SaveCopyCommand, ShowState, nameof(m.SaveCopyCommand.IsRunning));
        Bind.Property(m, ShowState, nameof(MaterialItemViewModel.ErrorMessage), nameof(MaterialItemViewModel.NoticeText),
            nameof(MaterialItemViewModel.DownloadFraction));
        // Files: "Save a copy…" in the right-click menu (a click on the row opens them).
        if (m.CanSaveCopy) ContextMenu.Attach(row, (Strings.OpenFile, m.OpenCommand), (Strings.SaveCopy, m.SaveCopyCommand));
        return row;
    }
}
