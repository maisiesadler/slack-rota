using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rota.Domain;
using Rota.Infrastructure;

namespace Rota.Console;

internal static class Services
{
    public static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var config = BuildConfiguration();

        services
            .AddDomain()
            .AddAdapters()
            .BindOptions(config);

        services.AddTransient<UpdateRotaInteractor>();

        return services.BuildServiceProvider();
    }

    private static IServiceCollection BindOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var rotaOptions = configuration.GetSection("RotaOptions");
        var builder = services.AddOptions<RotaOptions>()
            .Bind(configuration);

        return services;
    }

    private static IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration;
    }
}
