using System.Windows.Input;
using Gtk;

namespace Educ8.App.Gtk.Ui;

/// <summary>
/// A stock context menu (<see cref="PopoverMenu"/>) for a widget, opened by right-click or a long press, with one item per
/// command. Items are greyed out while their command can't run. The popover exists only while it's shown.
/// </summary>
internal static class ContextMenu
{
    public static void Attach(Widget widget, params (string Label, ICommand Command)[] items)
    {
        var group = Gio.SimpleActionGroup.New();
        var menu = Gio.Menu.New();
        for (var i = 0; i < items.Length; i++)
        {
            var (label, command) = items[i];
            var action = Gio.SimpleAction.New($"item{i}", null);
            action.SetEnabled(command.CanExecute(null));
            command.CanExecuteChanged += (_, _) => action.SetEnabled(command.CanExecute(null));
            action.OnActivate += (_, _) => { if (command.CanExecute(null)) command.Execute(null); };
            group.AddAction(action);
            menu.Append(label, $"context.item{i}");
        }
        widget.InsertActionGroup("context", group);

        void Show(double x, double y)
        {
            var popover = PopoverMenu.NewFromModel(menu);
            popover.SetHasArrow(false);
            popover.SetHalign(Align.Start);
            popover.SetParent(widget);
            var at = new Gdk.Rectangle { X = (int)x, Y = (int)y, Width = 1, Height = 1 };
            popover.SetPointingTo(at);
            popover.OnClosed += (_, _) => GLib.Functions.IdleAdd(GLib.Constants.PRIORITY_DEFAULT, () =>
            {
                popover.Unparent(); // after the item's action ran
                return false;
            });
            popover.Popup();
        }

        var click = GestureClick.New();
        click.SetButton(3); // secondary button
        click.OnPressed += (_, args) => Show(args.X, args.Y);
        widget.AddController(click);

        var press = GestureLongPress.New();
        press.SetTouchOnly(true);
        press.OnPressed += (_, args) => Show(args.X, args.Y);
        widget.AddController(press);
    }
}
