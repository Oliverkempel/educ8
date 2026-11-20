namespace Educ8.Itslearning;

/// <summary>
/// Well-known locations in the per-user data directory. The MCP server and the desktop app use the
/// same directory, so a login in one is visible to the other.
/// </summary>
public static class Educ8Paths
{
    public const string DataDirectoryEnvVar = "EDUC8_DATA_DIR";
    public const string ConfigFileName = "config.json";
    public const string TokenFileName = "tokens.json";
    public const string TokenLockFileName = "tokens.lock";
    public const string CallbackDirectoryName = "callbacks";

    public static string DefaultDataDirectory => DataDirectoryFor(Environment.GetEnvironmentVariable);

    /// <summary>
    /// <see cref="DataDirectoryEnvVar"/> if set. Inside a Flatpak or a strict snap, the real <c>~/.local/share/Educ8</c>:
    /// there <c>XDG_DATA_HOME</c> is the app's private <c>~/.var/app/…</c>, and the sandboxed app must share its login and
    /// MCP settings with an <c>educ8-mcp</c> started outside it (the manifest grants <c>--filesystem=xdg-data/Educ8</c>).
    /// Otherwise the platform's per-user data folder.
    /// </summary>
    public static string DataDirectoryFor(Func<string, string?> getEnv)
    {
        if (getEnv(DataDirectoryEnvVar) is { Length: > 0 } overridden) return overridden;
        if (getEnv("FLATPAK_ID") is { Length: > 0 } && getEnv("HOME") is { Length: > 0 } home)
            return Path.Combine(home, ".local", "share", "Educ8");
        // A strict snap points HOME into the snap's own folder; SNAP_REAL_HOME is the user's home (the snap's
        // personal-files plug grants ~/.local/share/Educ8).
        if (getEnv("SNAP") is { Length: > 0 } && getEnv("SNAP_REAL_HOME") is { Length: > 0 } realHome)
            return Path.Combine(realHome, ".local", "share", "Educ8");
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), "Educ8");
    }

    /// <summary>Creates the directory if needed, restricted to the current user on Unix.</summary>
    public static string EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            if (OperatingSystem.IsWindows())
                Directory.CreateDirectory(path);
            else
                Directory.CreateDirectory(path, UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
        }
        return path;
    }

    /// <summary>Restricts a file to the current user (no-op on Windows, where the profile directory is already private).</summary>
    public static void RestrictToUser(string file)
    {
        if (!OperatingSystem.IsWindows())
            File.SetUnixFileMode(file, UnixFileMode.UserRead | UnixFileMode.UserWrite);
    }
}
