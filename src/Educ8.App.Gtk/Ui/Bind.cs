using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Gtk;

namespace Educ8.App.Gtk.Ui;

/// <summary>
/// All widget ↔ view-model wiring goes through here (GTK has no data binding). Rule 5 of the desktop plan: binding bugs get
/// fixed in one place. Views live as long as the window, so subscriptions aren't removed.
/// </summary>
internal static class Bind
{
    /// <summary>Calls <paramref name="apply"/> now and whenever any of <paramref name="properties"/> changes.</summary>
    public static void Property(INotifyPropertyChanged source, Action apply, params string[] properties)
    {
        apply();
        source.PropertyChanged += (_, e) =>
        {
            if (properties.Length == 0 || e.PropertyName is null || properties.Contains(e.PropertyName)) apply();
        };
    }

    /// <summary>Label text from a view-model property.</summary>
    public static void Text(Label label, INotifyPropertyChanged source, string property, Func<string?> get) =>
        Property(source, () => label.SetText(get() ?? ""), property);

    /// <summary>Widget visibility from a view-model property.</summary>
    public static void Visible(Widget widget, INotifyPropertyChanged source, string property, Func<bool> get) =>
        Property(source, () => widget.SetVisible(get()), property);

    /// <summary>Rebuilds when the collection changes (lists here are small: a few dozen items).</summary>
    public static void Collection(INotifyCollectionChanged source, Action rebuild)
    {
        rebuild();
        source.CollectionChanged += (_, _) => rebuild();
    }

    /// <summary>
    /// Keeps a list box's rows in step with a collection, row by row: adds and removes touch only those rows, so the
    /// scroll position stays put when a live list grows (a full rebuild briefly empties the page and jumps to the top).
    /// Other changes (reset, move, replace) rebuild.
    /// </summary>
    public static void Rows<T>(ListBox list, System.Collections.ObjectModel.ObservableCollection<T> items, Func<T, Widget> row)
    {
        void Rebuild()
        {
            list.RemoveAll();
            foreach (var item in items) list.Append(row(item));
        }
        Rebuild();
        items.CollectionChanged += (_, e) =>
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add when e.NewItems is not null:
                    for (var i = 0; i < e.NewItems.Count; i++) list.Insert(row((T)e.NewItems[i]!), e.NewStartingIndex + i);
                    break;
                case NotifyCollectionChangedAction.Remove when e.OldItems is not null:
                    for (var i = 0; i < e.OldItems.Count; i++)
                        if (list.GetRowAtIndex(e.OldStartingIndex) is { } r) list.Remove(r);
                    break;
                default:
                    Rebuild();
                    break;
            }
        };
    }

    /// <summary>Button → command, with sensitivity from CanExecute.</summary>
    public static void Command(Button button, ICommand command, Func<object?>? parameter = null)
    {
        button.OnClicked += (_, _) =>
        {
            var p = parameter?.Invoke();
            if (command.CanExecute(p)) command.Execute(p);
        };
        button.SetSensitive(command.CanExecute(parameter?.Invoke()));
        command.CanExecuteChanged += (_, _) => button.SetSensitive(command.CanExecute(parameter?.Invoke()));
    }

    /// <summary>An error banner with "Try again" bound to a message property and a retry command.</summary>
    public static Adw.Banner ErrorBanner(INotifyPropertyChanged source, string property, Func<string?> get, ICommand? retry)
    {
        var banner = Adw.Banner.New("");
        banner.SetUseMarkup(false);
        if (retry is not null)
        {
            banner.SetButtonLabel(Core.Resources.Strings.Retry);
            banner.OnButtonClicked += (_, _) => retry.Execute(null);
        }
        Property(source, () =>
        {
            var message = get();
            banner.SetTitle(message ?? "");
            banner.SetRevealed(message is not null);
        }, property);
        return banner;
    }
}
