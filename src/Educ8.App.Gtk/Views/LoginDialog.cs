using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;

namespace Educ8.App.Gtk.Views;

/// <summary>
/// Sign-in sheet with an embedded WebKitGTK view. Every navigation goes through <see cref="LoginViewModel.OnNavigating"/>;
/// the <c>itsl-itslearning://</c> redirect is cancelled here and the view model completes the login.
/// </summary>
/// <remarks>
/// itslearning delivers the redirect from page script (<c>top.location.replace</c>). It is caught three ways, whichever comes
/// first: the navigation policy, a URI scheme handler registered for the redirect scheme, and a failed load of that scheme.
/// <c>EDUC8_DEBUG=1</c> logs each navigation's scheme and host (never paths or queries: they can hold codes) to stderr.
/// </remarks>
internal sealed class LoginDialog
{
    private static readonly bool Debug = Environment.GetEnvironmentVariable("EDUC8_DEBUG") is "1" or "true";
    private static bool _schemeRegistered;

    private readonly LoginViewModel _login;
    private readonly Widget _parent;
    private Adw.Dialog? _dialog;
    private bool _redirectHandled;

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
        _redirectHandled = false;
        RegisterRedirectScheme();

        var web = WebKit.WebView.New();
        web.SetVexpand(true);
        web.SetHexpand(true);
        web.OnDecidePolicy += (_, args) =>
        {
            if (args.Decision is WebKit.NavigationPolicyDecision navigation
                && navigation.GetNavigationAction().GetRequest().GetUri() is { } uri)
            {
                Log($"policy {args.DecisionType}: {Describe(uri)}");
                if (TryHandleRedirect(uri))
                {
                    args.Decision.Ignore();
                    return true;
                }
            }
            else
            {
                Log($"policy {args.DecisionType} ({args.Decision.GetType().Name})");
            }
            return false; // default handling
        };
        web.OnLoadChanged += (_, args) => Log($"load {args.LoadEvent}: {Describe(web.GetUri())}");
        web.OnLoadFailed += (_, args) =>
        {
            Log($"load failed: {Describe(args.FailingUri)}: {args.Error.Message}");
            return TryHandleRedirect(args.FailingUri); // true = no error page
        };
        // Links that open a new window (target=_blank) would otherwise go nowhere: open them in this view.
        web.OnCreate += (_, args) =>
        {
            var uri = args.NavigationAction.GetRequest().GetUri();
            Log($"new window: {Describe(uri)}");
            if (uri is not null && !TryHandleRedirect(uri)) web.LoadUri(uri);
            return null!;
        };
        web.OnWebProcessTerminated += (_, args) => Log($"web process terminated: {args.Reason}");

        var busy = Widgets.Spinner(48);
        Bind.Visible(busy, _login, nameof(LoginViewModel.IsBusy), () => _login.IsBusy);
        var overlay = Overlay.New();
        overlay.SetChild(web);
        overlay.AddOverlay(busy);

        var header = Adw.HeaderBar.New();
        header.SetTitleWidget(Adw.WindowTitle.New(Strings.LoginTitle, ""));
        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(header);
        toolbar.SetContent(overlay);

        var dialog = _dialog = Adw.Dialog.New();
        dialog.SetContentWidth(960);
        dialog.SetContentHeight(760);
        dialog.SetChild(toolbar);
        dialog.OnClosed += (_, _) =>
        {
            Log("dialog closed");
            if (_dialog == dialog && _login.IsActive) _login.CancelCommand.Execute(null);
            _dialog = null;
        };
        dialog.Present(_parent);
        web.LoadUri(_login.AuthorizeUrl!.ToString());
    }

    /// <summary>Completes the login once per dialog if <paramref name="uri"/> is the itslearning redirect.</summary>
    private bool TryHandleRedirect(string? uri)
    {
        if (uri is null || !Uri.TryCreate(uri, UriKind.Absolute, out var url) || !_login.IsRedirect(url)) return false;
        if (_redirectHandled) return true;
        _redirectHandled = true;
        Log("redirect caught");
        _login.OnNavigating(url);
        return true;
    }

    private void RegisterRedirectScheme()
    {
        if (_schemeRegistered || _login.AuthorizeUrl is null) return;
        _schemeRegistered = true;
        WebKit.WebContext.GetDefault().RegisterUriScheme(_login.RedirectScheme, request =>
        {
            Log("scheme handler");
            TryHandleRedirect(request.GetUri());
            request.Finish(Gio.MemoryInputStream.New(), 0, "text/html");
        });
    }

    private static string Describe(string? uri) =>
        uri is not null && Uri.TryCreate(uri, UriKind.Absolute, out var u) ? $"{u.Scheme}://{u.Host}" : "(none)";

    private static void Log(string message)
    {
        if (Debug) Console.Error.WriteLine($"[login] {message}");
    }
}
