using System.Diagnostics;
using System.Runtime.Versioning;
using Educ8.Itslearning;
using Microsoft.Win32;

namespace Educ8.Mcp.Server;

/// <summary>
/// Registers this executable as the OS handler for itsl-itslearning:// so the browser can hand the login redirect to us.
/// Linux: ~/.local/share/applications/*.desktop + xdg-mime. Windows: HKCU\Software\Classes. macOS needs an app bundle
/// (not supported here; use paste/`login_complete`). The desktop app will capture the redirect in its WebView instead.
/// </summary>
internal static class UrlSchemeHandler
{
    private const string Scheme = "itsl-itslearning";
    private const string DesktopFileName = "educ8-itslearning-login.desktop";

    public static int Register()
    {
        var command = HandlerCommand();
        if (OperatingSystem.IsLinux()) return RegisterLinux(command);
        if (OperatingSystem.IsWindows()) return RegisterWindows(command);
        Console.Error.WriteLine("URL handler registration isn't supported on this OS. Paste the final login link instead (`educ8-mcp login` or the `login_complete` tool).");
        return 1;
    }

    public static int Unregister()
    {
        if (OperatingSystem.IsLinux())
        {
            var file = Path.Combine(ApplicationsDir(), DesktopFileName);
            if (File.Exists(file)) File.Delete(file);
            Run("update-desktop-database", ApplicationsDir());
            Console.WriteLine($"Removed {file}.");
            return 0;
        }
        if (OperatingSystem.IsWindows())
        {
            Registry.CurrentUser.DeleteSubKeyTree($@"Software\Classes\{Scheme}", throwOnMissingSubKey: false);
            Console.WriteLine($@"Removed HKCU\Software\Classes\{Scheme}.");
            return 0;
        }
        return 1;
    }

    /// <summary>The command line the OS should run, with the data directory baked in so the handler finds the same inbox.</summary>
    private static (string Executable, string[] PrefixArgs, string DataDir) HandlerCommand()
    {
        var process = Environment.ProcessPath ?? throw new InvalidOperationException("Cannot determine the executable path.");
        var dataDir = Educ8Paths.DefaultDataDirectory;
        // When started as `dotnet educ8-mcp.dll`, the process is the dotnet host: pass the dll explicitly.
        return Path.GetFileNameWithoutExtension(process).Equals("dotnet", StringComparison.OrdinalIgnoreCase)
            ? (process, [typeof(UrlSchemeHandler).Assembly.Location], dataDir)
            : (process, [], dataDir);
    }

    private static int RegisterLinux((string Executable, string[] PrefixArgs, string DataDir) cmd)
    {
        var dir = Directory.CreateDirectory(ApplicationsDir()).FullName;
        var file = Path.Combine(dir, DesktopFileName);
        var exec = string.Join(' ', new[] { cmd.Executable }.Concat(cmd.PrefixArgs).Concat(["handle-url", "--data-dir", cmd.DataDir]).Select(QuoteDesktop)) + " %u";
        File.WriteAllText(file, $"""
            [Desktop Entry]
            Type=Application
            Name=Educ8 itslearning login
            Comment=Receives itslearning login redirects for educ8-mcp
            Exec={exec}
            NoDisplay=true
            Terminal=false
            MimeType=x-scheme-handler/{Scheme};

            """);
        Run("update-desktop-database", dir);
        var ok = Run("xdg-mime", "default", DesktopFileName, $"x-scheme-handler/{Scheme}");
        Console.WriteLine($"Wrote {file}.");
        Console.WriteLine(ok
            ? $"Registered as the handler for {Scheme}:// links."
            : $"Could not run xdg-mime. Register manually: xdg-mime default {DesktopFileName} x-scheme-handler/{Scheme}");
        return ok ? 0 : 1;
    }

    [SupportedOSPlatform("windows")]
    private static int RegisterWindows((string Executable, string[] PrefixArgs, string DataDir) cmd)
    {
        using var key = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{Scheme}");
        key.SetValue("", "URL:itslearning login (Educ8)");
        key.SetValue("URL Protocol", "");
        using var command = key.CreateSubKey(@"shell\open\command");
        var parts = new[] { cmd.Executable }.Concat(cmd.PrefixArgs).Concat(["handle-url", "--data-dir", cmd.DataDir]).Select(p => $"\"{p}\"");
        command.SetValue("", string.Join(' ', parts) + " \"%1\"");
        Console.WriteLine($@"Registered HKCU\Software\Classes\{Scheme} as the handler for {Scheme}:// links.");
        return 0;
    }

    private static string ApplicationsDir() =>
        Path.Combine(Environment.GetEnvironmentVariable("XDG_DATA_HOME") is { Length: > 0 } xdg
            ? xdg
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "share"), "applications");

    /// <summary>Quotes an argument for a .desktop Exec line.</summary>
    private static string QuoteDesktop(string arg) =>
        arg.Any(c => char.IsWhiteSpace(c) || c is '"' or '\'' or '\\' or '$' or '`')
            ? "\"" + arg.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("$", "\\$").Replace("`", "\\`") + "\""
            : arg;

    private static bool Run(string file, params string[] arguments)
    {
        try
        {
            var psi = new ProcessStartInfo(file) { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
            foreach (var a in arguments) psi.ArgumentList.Add(a);
            using var p = Process.Start(psi)!;
            p.WaitForExit(10_000);
            return p.ExitCode == 0;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
