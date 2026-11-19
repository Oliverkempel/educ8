using Educ8.App.Core.Resources;
using Educ8.App.Core.ViewModels;
using Educ8.App.Gtk.Ui;
using Gtk;
using static Educ8.App.Gtk.Ui.Widgets;

namespace Educ8.App.Gtk.Views;

/// <summary>Signed-out page: what the app is, and "Sign in with itslearning".</summary>
internal sealed class WelcomeView
{
    public Widget Root { get; }

    public WelcomeView(ShellViewModel shell)
    {
        var signIn = Button.NewWithLabel(Strings.SignIn);
        signIn.AddCssClass("suggested-action");
        signIn.AddCssClass("pill");
        signIn.SetHalign(Align.Center);
        Bind.Command(signIn, shell.Login.StartCommand);

        var error = WrapLabel("", "error");
        error.SetJustify(Justification.Center);
        error.SetXalign(0.5f);
        void ShowError()
        {
            var message = shell.Login.ErrorMessage ?? shell.ErrorMessage;
            error.SetText(message ?? "");
            error.SetVisible(message is not null);
        }
        Bind.Property(shell, ShowError, nameof(ShellViewModel.ErrorMessage));
        Bind.Property(shell.Login, ShowError, nameof(LoginViewModel.ErrorMessage));

        var status = Adw.StatusPage.New();
        status.SetIconName(Icons.Name(Core.Presentation.AppIcon.Courses));
        status.SetTitle(Strings.AppName);
        status.SetDescription(Escape(Strings.SignInIntro));
        status.SetChild(VBox(18, signIn, error));

        var toolbar = Adw.ToolbarView.New();
        toolbar.AddTopBar(Adw.HeaderBar.New());
        toolbar.SetContent(status);
        Root = toolbar;
    }
}
