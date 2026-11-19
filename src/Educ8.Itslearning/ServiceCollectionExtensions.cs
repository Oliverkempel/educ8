using System.Net.Http.Headers;
using Educ8.Itslearning.Auth;
using Educ8.Itslearning.Content;
using Educ8.Itslearning.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Educ8.Itslearning;

public static class ServiceCollectionExtensions
{
    private const string UserAgent = "Educ8/0.1";

    /// <summary>
    /// Registers the itslearning core: options, OAuth, token store/manager, login service, callback inbox and the REST client.
    /// Hosts may register their own <see cref="ITokenStore"/> or <see cref="ICallbackInbox"/> first to replace the file-based ones.
    /// </summary>
    public static IServiceCollection AddItslearning(this IServiceCollection services, ItslearningOptions options)
    {
        options.Validate();
        services.AddSingleton<IOptions<ItslearningOptions>>(Options.Create(options));
        services.TryAddSingleton(TimeProvider.System);
        services.TryAddSingleton<ITokenStore, FileTokenStore>();
        services.TryAddSingleton<ICallbackInbox, FileCallbackInbox>();
        services.TryAddSingleton<TokenManager>();
        services.TryAddSingleton<IAccessTokenProvider>(sp => sp.GetRequiredService<TokenManager>());
        services.TryAddSingleton<LoginService>();
        services.AddTransient<BearerTokenHandler>();
        services.TryAddSingleton<IWebHandlerFactory, DefaultWebHandlerFactory>();
        services.TryAddSingleton<LearningToolContentReader>();
        services.TryAddSingleton<ICourseFileSource>(sp => sp.GetRequiredService<LearningToolContentReader>());

        services.AddHttpClient<OAuthClient>(http => Configure(http, options));
        services.AddHttpClient<ItslearningClient>(http =>
            {
                Configure(http, options);
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<BearerTokenHandler>();
        return services;
    }

    private static void Configure(HttpClient http, ItslearningOptions options)
    {
        var baseUrl = options.BaseUrl.ToString();
        http.BaseAddress = new Uri(baseUrl.EndsWith('/') ? baseUrl : baseUrl + "/");
        http.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
        http.Timeout = TimeSpan.FromSeconds(60);
    }
}
