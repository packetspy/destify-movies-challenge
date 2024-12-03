using Microsoft.EntityFrameworkCore;
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
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IActorService, ActorService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();

        return services;
    }
}
