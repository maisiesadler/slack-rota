using Microsoft.Extensions.DependencyInjection;
using Rota.Domain.Commands;
using Rota.Domain.Queries;
using Rota.Infrastructure.Commands;
using Rota.Infrastructure.Dependencies.Slack;
using Rota.Infrastructure.Queries;

namespace Rota.Infrastructure;

public static class ServiceCollectionExtensions
{
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
