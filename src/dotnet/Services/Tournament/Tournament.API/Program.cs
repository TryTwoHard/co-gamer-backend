using Serilog;
using Tournament.API.Extensions;
using Tournament.API.Persistence;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    builder.Host.AddAppConfigurations();
    builder.Services.AddInfrastructure(builder.Configuration);
    var app = builder.Build();
    app.UseInfrastructure();
    app.MigrateDatabase<TournamentContext>((context, _) =>
        {
            TournamentContextSeed.SeedTournamentAsync(context, Log.Logger).Wait();
        })
        .Run();
}
catch (Exception ex)
{
    var error = ex.GetType().Name;
    if (error.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    
    Log.Fatal($"Unhandled exception: {ex}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}

