using Microsoft.Extensions.DependencyInjection;
using Rota.Domain.Commands;
using Rota.Infrastructure.Commands;
using Rota.Infrastructure.Dependencies.Slack;

namespace Rota.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdapters(this IServiceCollection services)
    {
        services.AddHttpClient<IUpdateSlackTopicCommand, UpdateSlackTopicCommand>(client =>
        {
            client.BaseAddress = new Uri("https://slack.com");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SlackReporterConfiguration.Token);
        });
        return services;
    }
}
