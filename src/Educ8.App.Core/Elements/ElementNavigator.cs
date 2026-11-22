using System.Globalization;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.Resources;
using Educ8.App.Core.Services;
using Educ8.Itslearning.Http;

namespace Educ8.App.Core.Elements;

/// <summary>
/// The one place that decides how a course element opens: the first registered <see cref="IElementModule"/> that handles it, else
/// the browser. In-app pages are announced with <see cref="PageOpened"/>; the platform pushes them onto the navigation stack of
/// the section that's showing (an open always follows a click there).
/// </summary>
public sealed class ElementNavigator(IEnumerable<IElementModule> modules, IOpenUrl openUrl, Func<ItslearningClient> client)
{
    private readonly IReadOnlyList<IElementModule> _modules = modules.ToList();

    /// <summary>Raised with a page to show inside the app (the platform pushes it and calls <see cref="ElementPageViewModel.EnsureLoadedAsync"/>).</summary>
    public event EventHandler<ElementPageViewModel>? PageOpened;

    internal Func<ItslearningClient> Client => client;
    internal IOpenUrl OpenUrl => openUrl;

    public IElementModule? ModuleFor(ElementRef element) => _modules.FirstOrDefault(m => m.Handles(element));

    public OpenStyle StyleOf(ElementRef element) => ModuleFor(element)?.Style ?? OpenStyle.Browser;

    /// <summary>A row for the element (title, kind, how it opens, the open command).</summary>
    public MaterialItemViewModel Item(ElementRef element, string? detail = null) => new(element, this, detail);

    /// <summary>Shows a page that was created outside a module (e.g. a course's root folder).</summary>
    public void Show(ElementPageViewModel page) => PageOpened?.Invoke(this, page);

    /// <summary>
    /// Opens the element with its module, or in the browser. Returns a message to show when the module failed (the element was
    /// then opened in the browser instead); null otherwise. Cancelling stops quietly.
    /// </summary>
    internal async Task<string?> OpenAsync(ElementRef element, CancellationToken ct) => await OpenAsync(element, null, ct);

    /// <inheritdoc cref="OpenAsync(ElementRef, CancellationToken)"/>
    internal async Task<string?> OpenAsync(ElementRef element, IProgress<double>? progress, CancellationToken ct)
    {
        if (ModuleFor(element) is not { } module)
        {
            if (element.Url is { } url) openUrl.Open(url);
            return null;
        }
        try
        {
            var page = await module.OpenAsync(element, this, progress, ct);
            if (page is not null && !ct.IsCancellationRequested) Show(page);
            return null;
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return null;
        }
        catch (Exception e)
        {
            if (element.Url is { } url) openUrl.Open(url);
            var format = module.Style == OpenStyle.LocalFile ? Strings.OpenFileFailed : Strings.OpenHereFailed;
            return string.Format(CultureInfo.CurrentCulture, format, Texts.Error(e));
        }
    }

    /// <summary>
    /// Opens a link from page content: links to an element on the itslearning site open like that element (in the app where a
    /// module handles it); anything else goes to the browser. Only http(s) and mailto links are opened.
    /// </summary>
    public async Task OpenLinkAsync(Uri url, CancellationToken ct = default)
    {
        if (!url.IsAbsoluteUri || url.Scheme is not ("http" or "https" or "mailto")) return;
        if (Itslearning.Content.WebPageParsers.ElementIdFromUrl(url, client().BaseAddress) is { } elementId)
            await OpenElementAsync(elementId, url, ct);
        else
            openUrl.Open(url);
    }

    /// <summary>Opens an element known only by id (e.g. a link inside a page): looks it up, then opens it like a row would.</summary>
    public async Task<string?> OpenElementAsync(int elementId, Uri? fallback, CancellationToken ct = default)
    {
        ElementRef element;
        try
        {
            element = ElementRef.From(await client().GetResourceAsync(elementId, ct));
        }
        catch (Exception e) when (e is ItslearningApiException or HttpRequestException or Itslearning.Auth.NotAuthenticatedException)
        {
            if (fallback is not null) openUrl.Open(fallback);
            return null;
        }
        return await OpenAsync(element.Url is null ? element with { Url = fallback } : element, ct);
    }
}
