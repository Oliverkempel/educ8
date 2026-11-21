using Educ8.App.Core.Elements;
using Educ8.App.Core.Presentation;
using Educ8.App.Core.ViewModels;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Educ8.App.Core;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the shared app view models. The platform app must also call <c>AddItslearning(...)</c> and register its
    /// <see cref="Services.IUiDispatcher"/>, <see cref="Services.IOpenUrl"/> and <see cref="Services.IOpenFile"/>. Then it resolves <see cref="ShellViewModel"/>.
    /// </summary>
    public static IServiceCollection AddEduc8AppCore(this IServiceCollection services)
    {
        // ItslearningClient is a short-lived typed HttpClient: long-lived objects take a factory, never an instance.
        services.AddSingleton<Func<ItslearningClient>>(sp => () => sp.GetRequiredService<ItslearningClient>());
        services.AddSingleton<CourseCatalog>();
        services.AddSingleton<Services.AppPreferences>();
        services.AddSingleton<Services.MaterialFiles>();
        // Course elements: one module per kind, first match wins; elements no module handles open in the browser.
        services.AddElementModule<Elements.FolderModule>();
        services.AddElementModule<Elements.FileModule>();
        services.AddElementModule<Elements.PageModule>();
        services.AddElementModule<Elements.LinkModule>();
        services.AddElementModule<Elements.AssignmentModule>();
        services.AddSingleton<Elements.AssignmentHandIn>();
        services.AddSingleton<Elements.ElementNavigator>();
        services.AddSingleton<CoursePageFactory>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<CoursesViewModel>();
        services.AddSingleton<EventDetailsViewModel>();
        services.AddSingleton<CalendarViewModel>();
        services.AddSingleton<NotificationsViewModel>();
        services.AddSingleton<Services.MessageAttachments>();
        services.AddSingleton<MessagesViewModel>();
        services.AddSingleton<McpViewModel>();
        services.AddSingleton<ShellViewModel>();
        return services;
    }
}
