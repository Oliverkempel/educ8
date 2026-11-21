using Educ8.Itslearning.Http;

namespace Educ8.App.Core.Services;

/// <summary>
/// Opens message attachments: a file on the itslearning site is downloaded with the user's login into the cache (one folder per
/// message) and opened in the default app; anything else (another host, or a web page) opens in the browser.
/// The attachment link format wasn't seen live yet (2026-11-21: no attachments in the test account), hence the fallback.
/// </summary>
public sealed class MessageAttachments(IOpenFile openFile, IOpenUrl openUrl)
{
    /// <summary>Where attachments go. Tests point it elsewhere.</summary>
    public string CacheDirectory { get; init; } = Path.Combine(Path.GetDirectoryName(MaterialFiles.DefaultCacheDirectory())!, "messages");

    public async Task OpenAsync(ItslearningClient client, string link, string? fileName, long messageId, CancellationToken ct = default)
    {
        if (!Uri.TryCreate(link, UriKind.Absolute, out var url) && (client.BaseAddress is null || !Uri.TryCreate(client.BaseAddress, link, out url)))
            return;
        var folder = Path.Combine(CacheDirectory, messageId.ToString(System.Globalization.CultureInfo.InvariantCulture));
        if (await client.TryDownloadSiteFileAsync(url, folder, fileName, ct) is { } path) openFile.Open(path);
        else openUrl.Open(url);
    }
}
