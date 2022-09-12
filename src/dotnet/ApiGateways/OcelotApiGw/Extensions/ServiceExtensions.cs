using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;

namespace OcelotApiGw.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOcelot(configuration)
            .AddConsul()
            .AddConfigStoredInConsul();

        services.AddSwaggerForOcelot(configuration);
    }
}