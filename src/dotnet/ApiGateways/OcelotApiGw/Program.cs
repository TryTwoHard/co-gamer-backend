using Ocelot.Middleware;
using OcelotApiGw.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    builder.Host.AddAppConfigurations();
    builder.Services.ConfigureOcelot(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        
    }
    
    
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGet("/", (context) =>
        {
            context.Response.Redirect("/swagger/index.html");
            return Task.CompletedTask;
        });
    });
    app.UseSwagger();
    app.UseSwaggerUI();

    // app.UseSwaggerForOcelotUI(options =>
    // {
    //     options.PathToSwaggerGenerator = "/swagger/docs";
    // });
    await app.UseOcelot();

    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled Exception: {ex}");
}
finally
{
    Log.Information($"Shut down {builder.Environment.ApplicationName} completes");
    Log.CloseAndFlush();
}