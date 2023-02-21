using Microsoft.Extensions.DependencyInjection;
using Rota.Commands;
using Rota.Queries;
using Rota.Adapters.Commands;
using Rota.Adapters.Dependencies.Slack;
using Rota.Adapters.Queries;

namespace Rota;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<UpdateRotaInteractor>();
        return services;
    }

    public static IServiceCollection AddAdapters(this IServiceCollection services)
    {
        services.AddTransient<IUpdateSlackTopicCommand, UpdateSlackTopicCommand>();
        services.AddTransient<IGetCurrentSlackTopicQuery, GetCurrentSlackTopicQuery>();
        services.AddTransient<IGetSlackUsersQuery, GetSlackUsersQuery>();

        services.AddHttpClient<SlackHttpClient>(client =>
        {
            client.BaseAddress = new Uri("https://slack.com");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SlackReporterConfiguration.Token);
        });
        return services;
    }
}
