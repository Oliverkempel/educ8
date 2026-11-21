namespace Educ8.App.Core.Elements;

/// <summary>Course folders open as an in-app page listing their items.</summary>
public sealed class FolderModule : IElementModule
{
    public bool Handles(ElementRef element) => string.Equals(element.ElementType, "Folder", StringComparison.OrdinalIgnoreCase);

    public OpenStyle Style => OpenStyle.InApp;

    public async Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, CancellationToken ct)
    {
        // Event links don't say which course the folder is in: look it up.
        var courseId = element.CourseId ?? (await navigator.Client().GetResourceAsync(element.ElementId, ct)).CourseId;
        return new FolderViewModel(navigator, element with { CourseId = courseId }, courseId, element.ElementId);
    }
}
