namespace OcelotApiGw.Extensions;

public static class HostExtensions
{
    public static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var env = context.HostingEnvironment;
            configBuilder.SetBasePath(context.HostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, reloadOnChange: true)
                .AddJsonFile($"Ocelot.{env.EnvironmentName}.json", false, reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }
}