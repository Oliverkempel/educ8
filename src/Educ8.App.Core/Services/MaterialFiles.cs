using System.Collections.Concurrent;
using Educ8.Itslearning.Content;

namespace Educ8.App.Core.Services;

/// <summary>
/// Opens course files in the user's own applications: downloads a File element into the cache folder, then hands it to the
/// platform. Each file is downloaded once per app session; later clicks reuse the local copy.
/// </summary>
public sealed class MaterialFiles(ICourseFileSource source, IOpenFile openFile)
{
    private readonly ConcurrentDictionary<int, string> _downloaded = new();

    /// <summary>Where downloaded files go (one subfolder per element). Tests point it elsewhere.</summary>
    public string CacheDirectory { get; init; } = DefaultCacheDirectory();

    public async Task OpenAsync(int elementId, CancellationToken ct = default)
    {
        if (!_downloaded.TryGetValue(elementId, out var path) || !File.Exists(path))
        {
            path = await source.SaveFileAsync(elementId, Path.Combine(CacheDirectory, elementId.ToString(System.Globalization.CultureInfo.InvariantCulture)), ct);
            _downloaded[elementId] = path;
        }
        openFile.Open(path);
    }

    /// <summary>The platform's per-user cache folder: XDG_CACHE_HOME (~/.cache) on Linux, ~/Library/Caches on macOS, %LOCALAPPDATA% on Windows.</summary>
    public static string DefaultCacheDirectory()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (OperatingSystem.IsWindows())
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Educ8", "Cache", "Files");
        if (OperatingSystem.IsMacOS()) return Path.Combine(home, "Library", "Caches", "Educ8", "Files");
        var xdg = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");
        return Path.Combine(string.IsNullOrEmpty(xdg) ? Path.Combine(home, ".cache") : xdg, "Educ8", "files");
    }
}
