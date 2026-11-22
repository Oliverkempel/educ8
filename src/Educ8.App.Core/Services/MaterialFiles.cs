using System.Collections.Concurrent;
using Educ8.Itslearning.Content;

namespace Educ8.App.Core.Services;

/// <summary>
/// Opens course files in the user's own applications: downloads a File element (or an assignment's file) into the cache folder,
/// then hands it to the platform, or saves a copy where the user chooses. Each file is downloaded once per app session; later
/// clicks reuse the local copy.
/// </summary>
public sealed class MaterialFiles(ICourseFileSource source, IOpenFile openFile, Func<Itslearning.Http.ItslearningClient>? client = null,
    IPickSaveLocation? saveLocation = null)
{
    private readonly ConcurrentDictionary<int, string> _downloaded = new();
    private readonly ConcurrentDictionary<string, string> _restDownloaded = new();

    /// <summary>Where downloaded files go (one subfolder per element). Tests point it elsewhere.</summary>
    public string CacheDirectory { get; init; } = DefaultCacheDirectory();

    public async Task OpenAsync(int elementId, IProgress<double>? progress = null, CancellationToken ct = default) =>
        openFile.Open(await DownloadAsync(elementId, progress, ct));

    /// <summary>The course file in the cache (downloaded the first time in a session). Reports the share downloaded when known.</summary>
    public async Task<string> DownloadAsync(int elementId, IProgress<double>? progress = null, CancellationToken ct = default)
    {
        if (!_downloaded.TryGetValue(elementId, out var path) || !File.Exists(path))
        {
            path = await source.SaveFileAsync(elementId, Path.Combine(CacheDirectory, elementId.ToString(System.Globalization.CultureInfo.InvariantCulture)), progress, ct);
            _downloaded[elementId] = path;
        }
        return path;
    }

    /// <summary>
    /// Asks where to save <paramref name="cachedPath"/> (suggesting its name) and copies it there. Returns the saved path, or null
    /// when the user cancelled. The save dialog already asked about replacing an existing file.
    /// </summary>
    public async Task<string?> SaveCopyAsync(string cachedPath, CancellationToken ct = default)
    {
        if (saveLocation is null) throw new InvalidOperationException("No save dialog on this platform.");
        var target = await saveLocation.PickSaveLocationAsync(Path.GetFileName(cachedPath), ct);
        if (string.IsNullOrEmpty(target)) return null;
        await using (var from = File.OpenRead(cachedPath))
        await using (var to = File.Create(target))
            await from.CopyToAsync(to, ct);
        return target;
    }

    /// <summary>
    /// Opens a file served by a REST path that redirects to it (assignment, answer and feedback files): saved under
    /// <c>&lt;cache&gt;/&lt;cacheKey&gt;/</c> once per session, then opened in the default application.
    /// </summary>
    public async Task OpenRestFileAsync(string relativePath, string cacheKey, string fallbackName, IProgress<double>? progress = null,
        CancellationToken ct = default) =>
        openFile.Open(await DownloadRestFileAsync(relativePath, cacheKey, fallbackName, progress, ct));

    /// <summary>A REST-served file (assignment, answer, feedback) in the cache (downloaded the first time in a session).</summary>
    public async Task<string> DownloadRestFileAsync(string relativePath, string cacheKey, string fallbackName, IProgress<double>? progress = null,
        CancellationToken ct = default)
    {
        if (client is null) throw new InvalidOperationException("No itslearning client for REST downloads.");
        if (!_restDownloaded.TryGetValue(relativePath, out var path) || !File.Exists(path))
        {
            path = await client().SaveAsync(relativePath, Path.Combine(CacheDirectory, cacheKey), fallbackName, progress, ct);
            _restDownloaded[relativePath] = path;
        }
        return path;
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
