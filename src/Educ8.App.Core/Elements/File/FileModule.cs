using Educ8.App.Core.Services;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>Course files are downloaded into the cache and opened in the user's default application for their type.</summary>
public sealed class FileModule(MaterialFiles files) : IElementModule, ISavesCopies
{
    public bool Handles(ElementRef element) => element.Kind == ResourceKind.File && element.ElementId > 0;

    public OpenStyle Style => OpenStyle.LocalFile;

    public async Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, IProgress<double>? progress, CancellationToken ct)
    {
        await files.OpenAsync(element.ElementId, progress, ct);
        return null;
    }

    public async Task<string?> SaveCopyAsync(ElementRef element, IProgress<double>? progress, CancellationToken ct) =>
        await files.SaveCopyAsync(await files.DownloadAsync(element.ElementId, progress, ct), ct);
}
