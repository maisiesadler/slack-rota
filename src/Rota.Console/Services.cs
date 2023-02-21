using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rota.Domain;
using Rota.Infrastructure;

namespace Rota.Console;

internal class Services
{
    public static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        var config = BuildConfiguration();

        services
            .AddDomain()
            .AddAdapters();

        services.AddTransient<UpdateRotaInteractor>();

        return services.BuildServiceProvider();
    }

    static IConfiguration BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration;
    }
}
