using System.Reflection;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Common.Repository;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using MySql.EntityFrameworkCore.Infrastructure;
using Shared.Configurations;
using Tournament.API.Controllers.ErrorsController;
using Tournament.API.Helpers;
using Tournament.API.Persistence;
using Tournament.API.Profiles;
using Tournament.API.Repositories.Implementations;
using Tournament.API.Repositories.Interfaces;
using Tournament.API.Services.Implementations;
using Tournament.API.Services.Interfaces;

namespace Tournament.API.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {   
        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(gen =>
        {
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

            gen.IncludeXmlComments(xmlCommentsFullPath);
        });
        services.ConfigureTournamentDbContext(configuration);
        services.ConfigureApiVersioning();
        services.AddAutoMapper(config =>
        {
            config.AddProfile<TournamentProfile>();
        });
        services.AddInfrastructureServices();

        return services;
    }

    private static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
            .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
            .AddScoped<ITournamentRepository, TournamentRepository>()
            .AddScoped<ITournamentService, TournamentService>()
            .AddTransient<ProblemDetailsFactory, TournamentProblemDetailsFactory>()
            .AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    private static IServiceCollection ConfigureTournamentDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
            throw new ArgumentNullException("Connection string is not configured.");
        
        var builder = new MySqlConnectionStringBuilder(databaseSettings.ConnectionString);
        services.AddDbContext<TournamentContext>(m => m.UseMySql(builder.ConnectionString, 
            ServerVersion.AutoDetect(builder.ConnectionString)));
            // , e =>
            // {
            //     e.MigrationsAssembly("Product.API");
            //     e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            // }));

        return services;
    }

    private static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });
    }
}