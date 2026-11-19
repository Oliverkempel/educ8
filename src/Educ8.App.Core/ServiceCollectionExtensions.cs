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
        services.AddSingleton<Services.MaterialFiles>();
        services.AddSingleton<CoursePageFactory>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<CoursesViewModel>();
        services.AddSingleton<EventDetailsViewModel>();
        services.AddSingleton<CalendarViewModel>();
        services.AddSingleton<McpViewModel>();
        services.AddSingleton<ShellViewModel>();
        return services;
    }
}
