using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// The MCP page: status of running servers, the write-actions switch, a switch per tool and recent tool calls.
/// Stock preferences widgets; everything is in <see cref="McpViewModel"/>.
/// </summary>
internal sealed class McpView
{
    public Widget Root { get; }

    public McpView(McpViewModel vm)
    {
        var refresh = IconButton(Icons.Name(AppIcon.Refresh), Strings.Refresh);
        Bind.Command(refresh, vm.RefreshCommand);
        var header = Adw.HeaderBar.New();
        header.PackEnd(refresh);

        var page = Adw.PreferencesPage.New();
        page.SetDescription(Escape(vm.IntroText));
        page.Add(Status(vm));
        page.Add(Permissions(vm));
        page.Add(Tools(vm, write: false));
        page.Add(Tools(vm, write: true));
        page.Add(Activity(vm));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(page);
        Root = toolbar;
    }

    private static Adw.PreferencesGroup Status(McpViewModel vm)
    {
        var group = Adw.PreferencesGroup.New();
        group.SetTitle(Escape(Strings.McpStatusTitle));
        var empty = WithIcon(Row(vm.StatusText), AppIcon.Mcp);
        group.Add(List(vm.Servers, s => WithIcon(Row(s.ClientText, s.DetailText), AppIcon.Mcp), empty));
        Bind.Property(vm, () =>
        {
            empty.SetTitle(vm.StatusText);
            group.SetDescription(vm.Servers.Count > 0 ? Escape(vm.StatusText) : null);
        }, nameof(McpViewModel.StatusText));
        return group;
    }

    private static Adw.PreferencesGroup Permissions(McpViewModel vm)
    {
        var group = Adw.PreferencesGroup.New();
        group.SetTitle(Escape(Strings.McpPermissionsTitle));
        var row = Switch(Strings.McpWriteActions, vm.WriteActionsHint, () => vm.AllowWriteActions, v => vm.AllowWriteActions = v,
            vm, nameof(McpViewModel.AllowWriteActions));
        row.SetSensitive(!vm.IsWriteActionsOverridden);
        group.Add(row);
        return group;
    }

    private static Adw.PreferencesGroup Tools(McpViewModel vm, bool write)
    {
        var group = Adw.PreferencesGroup.New();
        group.SetTitle(Escape(write ? Strings.McpWriteToolsTitle : Strings.McpToolsTitle));
        if (!write) group.SetDescription(Escape(Strings.McpToolsDescription));
        foreach (var tool in vm.Tools.Where(t => t.IsWrite == write))
        {
            var row = Switch(tool.Title, tool.Summary, () => tool.IsEnabled, v => tool.IsEnabled = v, tool, nameof(McpToolItemViewModel.IsEnabled));
            row.SetTooltipText(tool.Name);
            Bind.Property(tool, () => row.SetSensitive(tool.CanSwitch), nameof(McpToolItemViewModel.CanSwitch));
            group.Add(row);
        }
        return group;
    }

    private static Adw.PreferencesGroup Activity(McpViewModel vm)
    {
        var group = Adw.PreferencesGroup.New();
        group.SetTitle(Escape(Strings.McpActivityTitle));
        var empty = Row(vm.ActivityEmptyText ?? "");
        Bind.Property(vm, () => empty.SetTitle(vm.ActivityEmptyText ?? ""), nameof(McpViewModel.ActivityEmptyText));
        group.Add(List(vm.Activity, a =>
        {
            var subtitle = string.Join("\n", new[] { a.DetailText, a.ErrorText, a.ArgumentsText }.Where(s => !string.IsNullOrEmpty(s)));
            var row = WithIcon(Row(a.Title, subtitle), a.Outcome switch
            {
                McpCallOutcome.Ok => AppIcon.CallOk,
                McpCallOutcome.Failed => AppIcon.CallFailed,
                _ => AppIcon.CallBlocked,
            });
            row.SetSubtitleLines(3);
            return row;
        }, empty));
        return group;
    }

    /// <summary>A boxed list that follows a live collection row by row (keeps the scroll position); the placeholder shows when it's empty.</summary>
    private static ListBox List<T>(System.Collections.ObjectModel.ObservableCollection<T> items, Func<T, Widget> row, Widget placeholder)
    {
        var list = ListBox.New();
        list.AddCssClass("boxed-list");
        list.SetSelectionMode(SelectionMode.None);
        list.SetPlaceholder(placeholder);
        Bind.Rows(list, items, row);
        return list;
    }

    /// <summary>A switch row bound both ways to a view-model property.</summary>
    private static Adw.SwitchRow Switch(string title, string subtitle, Func<bool> get, Action<bool> set,
        System.ComponentModel.INotifyPropertyChanged source, string property)
    {
        var row = Adw.SwitchRow.New();
        row.SetUseMarkup(false);
        row.SetTitle(title);
        row.SetSubtitle(subtitle);
        var syncing = false;
        Bind.Property(source, () =>
        {
            syncing = true;
            row.SetActive(get());
            syncing = false;
        }, property);
        row.OnNotify += (_, e) =>
        {
            if (!syncing && e.Pspec.GetName() == "active" && row.GetActive() != get()) set(row.GetActive());
        };
        return row;
    }

    private static Adw.ActionRow WithIcon(Adw.ActionRow row, AppIcon icon)
    {
        row.AddPrefix(Image.NewFromIconName(Icons.Name(icon)));
        return row;
    }
}
