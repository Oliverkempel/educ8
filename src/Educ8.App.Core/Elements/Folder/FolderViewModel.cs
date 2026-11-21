using System.Collections.ObjectModel;
using Educ8.App.Core.Resources;

namespace Educ8.App.Core.Elements;

/// <summary>
/// One course folder (or the course's top level): its visible items in itslearning's order. Items open through their modules
/// (<see cref="ElementNavigator"/>): subfolders as further pages, files in the default app, and so on.
/// </summary>
public sealed class FolderViewModel : ElementPageViewModel
{
    internal FolderViewModel(ElementNavigator navigator, ElementRef element, int courseId, int? folderId)
        : base(element, navigator)
    {
        CourseId = courseId;
        FolderId = folderId;
    }

    /// <summary>The top level of a course's resources (the course page's Resources tab).</summary>
    internal static FolderViewModel CourseRoot(ElementNavigator navigator, int courseId, Uri? courseUrl) =>
        new(navigator, new ElementRef(0, Strings.CourseResources, "Folder", 0, courseId, courseUrl), courseId, null);

    public int CourseId { get; }

    /// <summary>Null for the course's top level.</summary>
    public int? FolderId { get; }

    public ObservableCollection<MaterialItemViewModel> Items { get; } = [];

    /// <summary>"This folder is empty." when loaded with no items; otherwise null.</summary>
    public override string? EmptyText => Loaded && !IsLoading && ErrorMessage is null && Items.Count == 0 ? Strings.FolderEmpty : null;

    protected override bool HasContent => Items.Count > 0;

    private bool Loaded { get; set; }

    protected override async Task LoadAsync(CancellationToken ct)
    {
        var items = await Navigator.Client().GetFolderResourcesAsync(CourseId, FolderId, ct);
        ct.ThrowIfCancellationRequested();
        Items.Clear();
        foreach (var r in items.Where(r => r.Active)) Items.Add(Navigator.Item(ElementRef.From(r)));
        Loaded = true;
    }
}
