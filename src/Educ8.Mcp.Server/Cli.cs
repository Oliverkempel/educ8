using System.Diagnostics;
using Educ8.Itslearning;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Educ8.Mcp.Server;

/// <summary>Interactive commands. These run in a terminal, so writing to stdout is fine here (not in serve mode).</summary>
internal static class Cli
{
    public static async Task<int> Login(ServiceProvider sp, bool noBrowser)
    {
        var login = sp.GetRequiredService<LoginService>();
        var pending = login.Begin();
        Console.WriteLine("Sign in to itslearning (with your school's login) at:");
        Console.WriteLine();
        Console.WriteLine(pending.AuthorizeUrl);
        Console.WriteLine();
        if (!noBrowser) TryOpenBrowser(pending.AuthorizeUrl);
        Console.WriteLine("Waiting for the login to finish. If the URL handler is registered this happens by itself.");
        Console.WriteLine("Otherwise paste the final itsl-itslearning://login/?… link here and press Enter:");

        using var cts = new CancellationTokenSource(LoginService.PendingLifetime);
        var inbox = login.WaitForInboxAsync(pending, cts.Token);
        var paste = Task.Run(() => Console.In.ReadLine(), cts.Token);
        try
        {
            while (true)
            {
                var done = await Task.WhenAny(inbox, paste);
                if (done == inbox)
                {
                    if (await inbox) break;
                    Console.Error.WriteLine("The login expired. Run `educ8-mcp login` again.");
                    return 1;
                }
                var line = await paste;
                if (string.IsNullOrWhiteSpace(line)) { paste = Task.Run(() => Console.In.ReadLine(), cts.Token); continue; }
                await login.CompleteAsync(line, cts.Token);
                break;
            }
        }
        catch (LoginException e)
        {
            Console.Error.WriteLine(e.Message);
            return 1;
        }
        finally
        {
            await cts.CancelAsync();
        }
        return await Status(sp);
    }

    public static async Task<int> Status(ServiceProvider sp)
    {
        var options = sp.GetRequiredService<IOptions<ItslearningOptions>>().Value;
        try
        {
            var me = await sp.GetRequiredService<ItslearningClient>().GetCurrentPersonAsync();
            Console.WriteLine($"Logged in to {options.Site.Host} as {me.DisplayName}.");
            return 0;
        }
        catch (NotAuthenticatedException e)
        {
            Console.WriteLine($"{e.Message} Run `educ8-mcp login`.");
            return 1;
        }
    }

    /// <summary>Shows or sets AllowWriteActions in config.json. Takes effect when the MCP server (re)starts.</summary>
    public static int WriteActions(string? value)
    {
        if (value is not null)
        {
            bool? parsed = value.Trim().ToLowerInvariant() switch
            {
                "on" or "true" or "enable" or "enabled" => true,
                "off" or "false" or "disable" or "disabled" => false,
                _ => null,
            };
            if (parsed is not { } allow)
            {
                Console.Error.WriteLine("Use `educ8-mcp write-actions on` or `educ8-mcp write-actions off`.");
                return 1;
            }
            Educ8McpOptions.SaveAllowWriteActions(allow);
            Console.WriteLine(allow
                ? "Write actions ENABLED: the AI can submit assignments and upload files (it always shows a preview and needs confirmation first)."
                : "Write actions disabled: the MCP server can only read.");
            Console.WriteLine("Restart the MCP server (e.g. restart Claude Desktop, or /mcp in Claude Code) for this to take effect.");
        }
        var current = Educ8McpOptions.Load();
        if (value is null) Console.WriteLine($"Write actions are {(current.AllowWriteActions ? "ENABLED" : "disabled")}.");
        if (Educ8McpOptions.IsOverriddenByEnvironment())
            Console.WriteLine($"Note: {Educ8McpOptions.AllowWriteActionsEnvVar} is set and overrides config.json (currently {(current.AllowWriteActions ? "on" : "off")}).");
        return 0;
    }

    public static async Task<int> Logout(ServiceProvider sp)
    {
        await sp.GetRequiredService<TokenManager>().SignOutAsync();
        Console.WriteLine("Logged out (stored itslearning session removed).");
        return 0;
    }

    /// <summary>
    /// Invoked by the OS for itsl-itslearning:// links. Drops the URL in the callback inbox for the process that started
    /// the login. Needs no ClientId, only the data directory.
    /// </summary>
    public static async Task<int> HandleUrl(string[] args)
    {
        var url = args.FirstOrDefault(a => a.StartsWith("itsl-itslearning:", StringComparison.OrdinalIgnoreCase));
        var dirIndex = Array.IndexOf(args, "--data-dir");
        var dataDir = dirIndex >= 0 && dirIndex + 1 < args.Length ? args[dirIndex + 1] : Educ8Paths.DefaultDataDirectory;
        if (url is null)
        {
            Console.Error.WriteLine("handle-url: no itsl-itslearning:// URL given.");
            return 1;
        }
        try
        {
            var redirect = AuthorizationRedirect.Parse(url);
            var inbox = new FileCallbackInbox(Options.Create(new ItslearningOptions { DataDirectory = dataDir }));
            await inbox.DeliverAsync(redirect, url);
            Notify("itslearning login received. You can close the browser tab.");
            return 0;
        }
        catch (LoginException e)
        {
            Notify($"itslearning login failed: {e.Message}");
            return 1;
        }
    }

    private static void Notify(string message)
    {
        Console.Error.WriteLine(message);
        if (OperatingSystem.IsLinux()) TryStart("notify-send", ["Educ8", message]);
    }

    private static void TryOpenBrowser(Uri url)
    {
        if (OperatingSystem.IsWindows()) TryStart("cmd", ["/c", "start", "", url.ToString()]);
        else if (OperatingSystem.IsMacOS()) TryStart("open", [url.ToString()]);
        else TryStart("xdg-open", [url.ToString()]);
    }

    private static void TryStart(string file, string[] arguments)
    {
        try
        {
            var psi = new ProcessStartInfo(file) { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
            foreach (var a in arguments) psi.ArgumentList.Add(a);
            Process.Start(psi);
        }
        catch (Exception)
        {
            // best effort only
        }
    }
}
