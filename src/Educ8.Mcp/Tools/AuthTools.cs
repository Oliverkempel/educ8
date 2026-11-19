using System.ComponentModel;
using Educ8.Itslearning;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;

namespace Educ8.Mcp.Tools;

[McpServerToolType]
public static class AuthTools
{
    [McpServerTool(Name = "auth_status", Title = "itslearning login status", ReadOnly = true, Idempotent = true, OpenWorld = true)]
    [Description("Shows whether the user is logged in to itslearning, and as whom. Use it if other tools report a login problem.")]
    public static Task<string> AuthStatus(
        LoginService login, TokenManager tokens, ItslearningClient client, IOptions<ItslearningOptions> options, Educ8McpOptions mcpOptions, CancellationToken ct) =>
        ToolGuard.RunAsync(login, async () =>
        {
            var site = options.Value.Site.Host;
            if (await tokens.HasSessionAsync(ct))
            {
                try
                {
                    var me = await client.GetCurrentPersonAsync(ct);
                    return $"Logged in to {site} as {me.DisplayName}. " +
                           (mcpOptions.AllowWriteActions
                               ? "Write actions (e.g. submitting assignments) are ENABLED."
                               : "Write actions are disabled: this server can only read.");
                }
                catch (NotAuthenticatedException)
                {
                    // fall through: the stored session has expired
                }
            }
            return login.Current is { } pending
                ? $"Not logged in to {site} yet. A login was started at {pending.StartedAt:HH:mm} UTC and is waiting for the user to finish it. " +
                  "If they've signed in, ask them for the final itsl-itslearning://login/?… link and pass it to `login_complete`."
                : $"Not logged in to {site}. {ToolGuard.LoginHint}";
        }, ct);

    [McpServerTool(Name = "login", Title = "Log in to itslearning", Destructive = false, OpenWorld = true)]
    [Description("Starts the itslearning login. Returns a link for the user to open and sign in with (their school's login). " +
                 "Show the link and instructions to the user exactly; never ask for their password.")]
    public static Task<string> Login(
        LoginService login, TokenManager tokens, ItslearningClient client,
        [Description("Start a new login even if already logged in.")] bool force = false,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            if (!force && await tokens.HasSessionAsync(ct))
            {
                try
                {
                    var me = await client.GetCurrentPersonAsync(ct);
                    return $"Already logged in as {me.DisplayName}. Pass force=true to log in again.";
                }
                catch (NotAuthenticatedException)
                {
                    // expired; start a new login
                }
            }

            var pending = login.Begin();
            return $"""
                Ask the user to open this link and sign in to itslearning (with their school's login):

                {pending.AuthorizeUrl}

                After signing in, itslearning hands the login to the `itsl-itslearning://` link:
                - If the Educ8 login handler is installed (`educ8-mcp register-url-handler`), the browser may ask to open it. Accept, and the login completes by itself. Then call `auth_status` to confirm.
                - Otherwise the page stays blank. The user must copy the final `itsl-itslearning://login/?state=…&code=…` link and you pass it to `login_complete`.

                The link is valid for {LoginService.PendingLifetime.TotalMinutes:0} minutes, and the code in it works only once.
                """;
        }, ct);

    [McpServerTool(Name = "login_complete", Title = "Finish itslearning login", Destructive = false, OpenWorld = true)]
    [Description("Finishes an itslearning login started with `login`, using the itsl-itslearning://login/?state=…&code=… link the user copied after signing in.")]
    public static Task<string> LoginComplete(
        LoginService login, ItslearningClient client,
        [Description("The full itsl-itslearning://login/?state=…&code=… link (or text containing it).")] string redirectUrl,
        CancellationToken ct = default) =>
        ToolGuard.RunAsync(login, async () =>
        {
            await login.CompleteAsync(redirectUrl, ct);
            var me = await client.GetCurrentPersonAsync(ct);
            return $"Logged in to itslearning as {me.DisplayName}.";
        }, ct);
}
