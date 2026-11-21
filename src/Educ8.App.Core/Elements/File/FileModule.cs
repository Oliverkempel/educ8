using Educ8.App.Core.Services;
using Educ8.Itslearning.Models;

namespace Educ8.App.Core.Elements;

/// <summary>Course files are downloaded into the cache and opened in the user's default application for their type.</summary>
public sealed class FileModule(MaterialFiles files) : IElementModule
{
    public bool Handles(ElementRef element) => element.Kind == ResourceKind.File && element.ElementId > 0;

    public OpenStyle Style => OpenStyle.LocalFile;

    public async Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, CancellationToken ct)
    {
        await files.OpenAsync(element.ElementId, ct);
        return null;
    }
}
