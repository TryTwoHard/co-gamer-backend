using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Infrastructure.Common.Repository;
using Microsoft.AspNetCore.Mvc.Versioning;
using Tournament.API.Persistence;
using Tournament.API.Profiles;
using Tournament.API.Repositories.Implementations;
using Tournament.API.Repositories.Interfaces;
using Tournament.API.Services.Implementations.Tournament;
using Tournament.API.Services.Interfaces.Tournament;

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
        services.AddSwaggerGen();
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
            .AddScoped<ITournamentService, TournamentService>();
    }

    private static void ConfigureTournamentDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TournamentContext>();
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