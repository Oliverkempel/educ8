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
        var busy = Spinner(16);
        // While it opens (a file downloading), the open icon becomes a stop button that cancels it.
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
        row.SetActivatable(m.CanOpen);
        row.SetTooltipText(m.OpenTooltip);
        row.OnActivated += (_, _) =>
        {
            Launch.Debug($"row activated: {m.Kind}, style={m.OpenStyle}, url={Launch.Describe(m.Url)}");
            if (!m.OpenCommand.IsRunning) m.OpenCommand.Execute(null);
        };
        Bind.Property(m.OpenCommand, () =>
        {
            var running = m.OpenCommand.IsRunning;
            busy.SetVisible(running);
            open.SetVisible(!running);
            cancel.SetVisible(running);
            row.SetSubtitle(running ? m.BusyText : m.ErrorMessage ?? m.SubtitleText);
        }, nameof(m.OpenCommand.IsRunning));
        Bind.Property(m, () => row.SetSubtitle(m.ErrorMessage ?? m.SubtitleText), nameof(MaterialItemViewModel.ErrorMessage));
        return row;
    }
}
