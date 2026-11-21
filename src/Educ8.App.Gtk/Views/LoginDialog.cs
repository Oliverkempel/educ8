using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// "Continue in your browser" while a sign-in runs in the default browser. The redirect comes back through the
/// application's <c>open</c> signal (see Program.cs), not through this dialog. Closing it cancels the sign-in.
/// </summary>
internal sealed class LoginDialog
{
    private readonly LoginViewModel _login;
    private readonly Widget _parent;
    private Adw.Dialog? _dialog;

    public LoginDialog(LoginViewModel login, Widget parent)
    {
        _login = login;
        _parent = parent;
        Bind.Property(login, () =>
        {
            if (login.IsActive && _dialog is null) Open();
            else if (!login.IsActive && _dialog is { } d) { _dialog = null; d.ForceClose(); }
        }, nameof(LoginViewModel.IsActive));
    }

    private void Open()
    {
        var again = Button.NewWithLabel(Strings.LoginOpenBrowserAgain);
        again.AddCssClass("pill");
        again.SetHalign(Align.Center);
        Bind.Command(again, _login.OpenBrowserAgainCommand);

        var status = Adw.StatusPage.New();
        status.SetIconName("web-browser-symbolic");
        status.SetTitle(Strings.LoginInBrowserTitle);
        status.SetDescription(Escape(Strings.LoginInBrowserText));
        status.SetChild(VBox(18, Spinner(32), again));

        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(Adw.WindowTitle.New(Strings.LoginTitle, ""));
        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(status);

        var dialog = _dialog = Adw.Dialog.New();
        dialog.SetContentWidth(460);
        dialog.SetChild(toolbar);
        dialog.OnClosed += (_, _) =>
        {
            if (_dialog == dialog && _login.IsActive) _login.CancelCommand.Execute(null);
            _dialog = null;
        };
        dialog.Present(_parent);
    }
}
