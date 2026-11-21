using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Educ8.App.Gtk.Services;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>The one row used for course material everywhere (event details, course overview, folders).</summary>
internal static class MaterialRows
{
    /// <param name="activate">What a click does; default: the item's own open command (files open in the default app).</param>
    public static Adw.ActionRow Row(MaterialItemViewModel m, Action? activate = null)
    {
        var folder = m.Kind == Itslearning.Models.ResourceKind.Folder;
        var row = Widgets.Row(m.Title, m.SubtitleText);
        row.AddPrefix(Image.NewFromIconName(Icons.Name(m.Icon)));
        var open = Image.NewFromIconName(folder ? "go-next-symbolic" : Icons.Name(AppIcon.OpenExternal));
        var busy = Spinner(16);
        // While a file downloads, the open icon becomes a stop button that cancels it.
        var cancel = Button.NewFromIconName(Icons.Name(AppIcon.CancelDownload));
        cancel.AddCssClass("flat");
        cancel.AddCssClass("circular");
        cancel.SetValign(Align.Center);
        cancel.SetTooltipText(Strings.CancelDownload);
        cancel.SetVisible(false);
        Bind.Command(cancel, m.OpenCancelCommand);
        row.AddSuffix(busy);
        row.AddSuffix(open);
        row.AddSuffix(cancel);
        row.SetActivatable(folder || m.OpensLocally || m.Url is not null);
        if (!folder) row.SetTooltipText(m.OpensLocally ? Strings.OpenFile : Strings.OpenInItslearning);
        row.OnActivated += (_, _) =>
        {
            Launch.Debug($"row activated: {m.Kind}, local={m.OpensLocally}, url={Launch.Describe(m.Url)}");
            if (activate is not null) activate();
            else if (!m.OpenCommand.IsRunning) m.OpenCommand.Execute(null);
        };
        Bind.Property(m.OpenCommand, () =>
        {
            var running = m.OpenCommand.IsRunning;
            busy.SetVisible(running);
            open.SetVisible(!running);
            cancel.SetVisible(running);
            row.SetSubtitle(running ? Strings.Downloading : m.ErrorMessage ?? m.SubtitleText);
        }, nameof(m.OpenCommand.IsRunning));
        Bind.Property(m, () => row.SetSubtitle(m.ErrorMessage ?? m.SubtitleText), nameof(MaterialItemViewModel.ErrorMessage));
        return row;
    }
}
