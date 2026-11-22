using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Educ8.App.Core.Elements;

/// <summary>How an element opens; platforms show it on the row (a chevron for in-app pages, the external icon otherwise).</summary>
public enum OpenStyle
{
    /// <summary>As a page inside the app.</summary>
    InApp,

    /// <summary>Downloaded and opened in the user's default application.</summary>
    LocalFile,

    /// <summary>In the browser (itslearning's web UI, or the link's target).</summary>
    Browser,
}

/// <summary>
/// Opens one kind of course element in the app. One class per kind (or learning tool), registered with
/// <see cref="ElementModules.AddElementModule{T}"/>; elements no module handles open in the browser. To support a new
/// itslearning tool: a module whose <see cref="Handles"/> matches it (e.g. <c>e.IsTool(13221)</c>), its page view model, and a
/// page builder per platform.
/// </summary>
public interface IElementModule
{
    /// <summary>Whether this module opens <paramref name="element"/>. Decided from the element's type and tool only (no I/O).</summary>
    bool Handles(ElementRef element);

    OpenStyle Style { get; }

    /// <summary>
    /// Opens the element: returns the page to show, or null when the module already finished (file opened, link launched).
    /// Throwing makes the caller report the problem and open the element in the browser instead. Modules that download report the
    /// share done (0–1) to <paramref name="progress"/> when they know it.
    /// </summary>
    Task<ElementPageViewModel?> OpenAsync(ElementRef element, ElementNavigator navigator, IProgress<double>? progress, CancellationToken ct);
}

/// <summary>A module whose elements are files the user can also save a copy of (in addition to opening them).</summary>
public interface ISavesCopies
{
    /// <summary>Downloads the element if needed and asks where to save a copy. Returns the saved path, or null when cancelled.</summary>
    Task<string?> SaveCopyAsync(ElementRef element, IProgress<double>? progress, CancellationToken ct);
}

public static class ElementModules
{
    /// <summary>Registers an element module. Registration order is priority (the first module that handles an element wins).</summary>
    public static IServiceCollection AddElementModule<T>(this IServiceCollection services) where T : class, IElementModule
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IElementModule, T>());
        return services;
    }
}
