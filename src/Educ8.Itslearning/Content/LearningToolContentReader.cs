using System.Net;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Educ8.Itslearning.Content;

/// <summary>Creates the HTTP handler for the itslearning web session. Replaceable in tests.</summary>
public interface IWebHandlerFactory
{
    HttpMessageHandler Create(CookieContainer cookies);
}

internal sealed class DefaultWebHandlerFactory : IWebHandlerFactory
{
    public HttpMessageHandler Create(CookieContainer cookies) => new SocketsHttpHandler
    {
        CookieContainer = cookies,
        UseCookies = true,
        AllowAutoRedirect = true,
        MaxAutomaticRedirections = 10,
        AutomaticDecompression = DecompressionMethods.All,
        PooledConnectionLifetime = TimeSpan.FromMinutes(5),
    };
}

/// <summary>
/// Reads content the REST API doesn't serve (Pages, Links; later Files) through the itslearning web UI:
/// <c>sso/url/v1</c> → web session cookies → <c>ViewLearningToolElement.aspx</c> → signed tool iframe → tool page.
/// Keeps one web session (cookie jar) per process and re-establishes it when it expires.
/// URLs on this path carry personal data and signatures: never log them.
/// </summary>
public sealed class LearningToolContentReader : ICourseFileSource, IDisposable
{
    private readonly ItslearningClient _api;
    private readonly ItslearningOptions _options;
    private readonly ILogger<LearningToolContentReader> _logger;
    private readonly HttpClient _web;
    private readonly SemaphoreSlim _ssoGate = new(1, 1);
    private volatile bool _hasSession;

    public LearningToolContentReader(ItslearningClient api, IWebHandlerFactory handlers, IOptions<ItslearningOptions> options, ILogger<LearningToolContentReader> logger)
    {
        _api = api;
        _options = options.Value;
        _logger = logger;
        _web = new HttpClient(handlers.Create(new CookieContainer()), disposeHandler: true) { Timeout = TimeSpan.FromSeconds(60) };
        _web.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; Educ8/0.1)");
    }

    public async Task<PageContent> ReadPageAsync(int elementId, CancellationToken ct = default)
    {
        var tool = await OpenToolAsync(elementId, ct).ConfigureAwait(false);
        var page = WebPageParsers.Page(tool.Html);
        if (page.Blocks.Count == 0 && !tool.Html.Contains("il-widgetzone", StringComparison.Ordinal))
            throw new ContentUnavailableException("The page opened, but its layout wasn't recognised (itslearning may have changed it).");
        return page;
    }

    public async Task<LinkContent> ReadLinkAsync(int elementId, CancellationToken ct = default)
    {
        var tool = await OpenToolAsync(elementId, ct).ConfigureAwait(false);
        return WebPageParsers.Link(tool.Html);
    }

    /// <summary>The student's view of an assignment (status, score, feedback), which REST doesn't provide.</summary>
    public async Task<AssignmentView> ReadAssignmentViewAsync(int elementId, CancellationToken ct = default)
    {
        var tool = await OpenToolAsync(elementId, ct).ConfigureAwait(false);
        return WebPageParsers.Assignment(tool.Html);
    }

    /// <summary>The File element's page info (name, description) without downloading it.</summary>
    public async Task<FileLink> GetFileLinkAsync(int elementId, CancellationToken ct = default)
    {
        var tool = await OpenToolAsync(elementId, ct).ConfigureAwait(false);
        return WebPageParsers.File(tool.Html, tool.FinalUri);
    }

    /// <summary>Largest file we download (bytes). Files are held in memory for extraction.</summary>
    public const long MaxFileBytes = 50L * 1024 * 1024;

    private readonly FileCache _files = new(maxEntries: 12, maxTotalBytes: 200L * 1024 * 1024, ttl: TimeSpan.FromMinutes(30));

    /// <summary>
    /// Downloads a File element (learning tool 5009) via its tool page's download link. Recently downloaded files are
    /// cached in memory so paging through a large document doesn't download it again.
    /// </summary>
    public async Task<DownloadedFile> DownloadFileAsync(int elementId, CancellationToken ct = default)
    {
        if (_files.TryGet(elementId, out var cached)) return cached;

        var tool = await OpenToolAsync(elementId, ct).ConfigureAwait(false);
        var link = WebPageParsers.File(tool.Html, tool.FinalUri);
        if (link.DownloadUrl is null)
            throw new ContentUnavailableException("The file page has no download link (the file may have been removed, or itslearning changed its layout).");

        var file = await DownloadAsync(link.DownloadUrl, link.FileName, ct).ConfigureAwait(false);
        _files.Add(elementId, file);
        return file;
    }

    /// <summary>
    /// Downloads a File element (learning tool 5009) into <paramref name="directory"/>, streamed to disk with no size limit,
    /// and returns the saved file's path. Used to open course files in the user's own applications.
    /// </summary>
    public async Task<string> SaveFileAsync(int elementId, string directory, CancellationToken ct = default)
    {
        var tool = await OpenToolAsync(elementId, ct).ConfigureAwait(false);
        var link = WebPageParsers.File(tool.Html, tool.FinalUri);
        if (link.DownloadUrl is null)
            throw new ContentUnavailableException("The file page has no download link (the file may have been removed, or itslearning changed its layout).");

        using var response = await _web.GetAsync(link.DownloadUrl, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            throw new ContentUnavailableException($"Downloading the file failed ({(int)response.StatusCode}).");

        var disposition = response.Content.Headers.ContentDisposition;
        var name = SafeFileName(disposition?.FileNameStar ?? disposition?.FileName?.Trim('"') ?? link.FileName, elementId);
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, name);
        var partial = path + ".part";
        try
        {
            await using (var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false))
            await using (var file = File.Create(partial))
                await stream.CopyToAsync(file, ct).ConfigureAwait(false);
            File.Move(partial, path, overwrite: true);
        }
        catch
        {
            File.Delete(partial);
            throw;
        }
        return path;
    }

    /// <summary>A file name safe to create locally: no directories, no invalid characters.</summary>
    internal static string SafeFileName(string? name, int elementId)
    {
        var bare = Path.GetFileName((name ?? "").Replace('\\', '/'));
        var invalid = Path.GetInvalidFileNameChars();
        var clean = new string(bare.Select(ch => invalid.Contains(ch) || char.IsControl(ch) ? '_' : ch).ToArray()).Trim().TrimStart('.');
        return clean.Length > 0 ? clean : $"file-{elementId}";
    }

    private async Task<DownloadedFile> DownloadAsync(Uri url, string? fallbackName, CancellationToken ct)
    {
        using var response = await _web.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            throw new ContentUnavailableException($"Downloading the file failed ({(int)response.StatusCode}).");
        if (response.Content.Headers.ContentLength > MaxFileBytes)
            throw new ContentUnavailableException($"The file is too large to read here ({response.Content.Headers.ContentLength / (1024 * 1024)} MB; limit {MaxFileBytes / (1024 * 1024)} MB).");

        var disposition = response.Content.Headers.ContentDisposition;
        var name = disposition?.FileNameStar ?? disposition?.FileName?.Trim('"') ?? fallbackName ?? "file";

        await using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var buffer = new MemoryStream();
        var chunk = new byte[81920];
        int read;
        while ((read = await stream.ReadAsync(chunk, ct).ConfigureAwait(false)) > 0)
        {
            if (buffer.Length + read > MaxFileBytes)
                throw new ContentUnavailableException($"The file is too large to read here (limit {MaxFileBytes / (1024 * 1024)} MB).");
            buffer.Write(chunk, 0, read);
        }
        return new DownloadedFile(name, response.Content.Headers.ContentType?.MediaType, buffer.ToArray());
    }

    /// <summary>Opens the element's tool page (the content inside itslearning's iframe).</summary>
    internal async Task<(Uri FinalUri, string Html)> OpenToolAsync(int elementId, CancellationToken ct)
    {
        var viewUrl = new Uri(_options.BaseUrl, $"LearningToolElement/ViewLearningToolElement.aspx?LearningToolElementId={elementId}");

        (Uri FinalUri, string Html)? view = null;
        if (_hasSession)
        {
            var existing = await GetAsync(viewUrl, ct).ConfigureAwait(false);
            if (WebPageParsers.IsElementView(existing.FinalUri)) view = existing;
            else _hasSession = false; // web session expired
        }
        if (view is null)
        {
            await _ssoGate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                var sso = await _api.GetSsoUrlAsync(viewUrl, ct).ConfigureAwait(false);
                view = await GetAsync(sso, ct).ConfigureAwait(false);
                _hasSession = WebPageParsers.IsElementView(view.Value.FinalUri);
                _logger.LogDebug("itslearning web session established: {Ok}", _hasSession);
            }
            finally
            {
                _ssoGate.Release();
            }
        }
        var (finalUri, html) = view.Value;
        if (!WebPageParsers.IsElementView(finalUri))
            throw new ContentUnavailableException("itslearning didn't open the item in its web view (no access, or the item was removed).");

        var iframe = WebPageParsers.ExtensionIframeUrl(html, finalUri)
                     ?? throw new ContentUnavailableException("This item has no readable content frame in the itslearning web view.");
        return await GetAsync(iframe, ct).ConfigureAwait(false);
    }

    private async Task<(Uri FinalUri, string Html)> GetAsync(Uri url, CancellationToken ct)
    {
        using var response = await _web.GetAsync(url, ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
            throw new ContentUnavailableException($"The itslearning web view returned {(int)response.StatusCode}.");
        var html = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
        return (response.RequestMessage?.RequestUri ?? url, html);
    }

    public void Dispose()
    {
        _web.Dispose();
        _ssoGate.Dispose();
    }
}
