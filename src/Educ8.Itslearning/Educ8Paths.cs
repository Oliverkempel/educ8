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

    public static string DefaultDataDirectory =>
        Environment.GetEnvironmentVariable(DataDirectoryEnvVar) is { Length: > 0 } overridden
            ? overridden
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create), "Educ8");

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
