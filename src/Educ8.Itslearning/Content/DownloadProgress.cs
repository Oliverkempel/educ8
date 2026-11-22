namespace Educ8.Itslearning.Content;

/// <summary>Copying a download to disk while reporting how far it got.</summary>
public static class DownloadProgress
{
    /// <summary>
    /// Copies <paramref name="from"/> to <paramref name="to"/>, reporting the share done (0–1) when <paramref name="length"/> is
    /// known, at most once per percent. With an unknown length nothing is reported (show an indeterminate indicator).
    /// </summary>
    public static async Task CopyAsync(Stream from, Stream to, long? length, IProgress<double>? progress, CancellationToken ct)
    {
        if (progress is null || length is not > 0)
        {
            await from.CopyToAsync(to, ct).ConfigureAwait(false);
            return;
        }
        var buffer = new byte[81920];
        long done = 0;
        var lastPercent = -1;
        int read;
        progress.Report(0);
        while ((read = await from.ReadAsync(buffer, ct).ConfigureAwait(false)) > 0)
        {
            await to.WriteAsync(buffer.AsMemory(0, read), ct).ConfigureAwait(false);
            done += read;
            var percent = (int)Math.Min(100, done * 100 / length.Value);
            if (percent == lastPercent) continue;
            lastPercent = percent;
            progress.Report(percent / 100.0);
        }
    }
}
