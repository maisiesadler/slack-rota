using Microsoft.Extensions.DependencyInjection;

namespace Rota.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<UpdateRotaInteractor>();
        return services;
    }
}
