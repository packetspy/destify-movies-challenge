using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Application.Services;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Infra.Data;
using MoviesChallenge.Infra.Repositories;

namespace MoviesChallenge.Api.Helpers;

public static class ServicesExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<MovieDbContext>(options => options.UseInMemoryDatabase("MoviesDB"));
        services.AddTransient<IDataSeedService, DataSeedService>();

        var _serviceProvider = services.BuildServiceProvider();
        var _dataSeedService = _serviceProvider.GetRequiredService<IDataSeedService>();
        _dataSeedService.RunSeedAsync().Wait();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IDirectorService, DirectorService>();
        services.AddScoped<IMovieService, MovieService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IDirectorRepository, DirectorRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IRatingRepository, RatingRepository>();

        return services;
    }

    public static IServiceCollection AddCustomCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CORSPolicy", builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.AllowAnyOrigin();
                builder.WithHeaders("content-type");
                builder.WithHeaders("authorization");
            });
        });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies Challenge", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization Header",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
