using Microsoft.Extensions.DependencyInjection;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Infra.Dtos;
using System.Reflection;
using System.Text.Json;

namespace MoviesChallenge.Infra.Data;

public class MovieSeeder
{
    private static List<MovieDto> moviesList = new List<MovieDto>();
    private static HashSet<Actor> actorsList = new HashSet<Actor>();
    private static HashSet<Director> directorsList = new HashSet<Director>();

    public static void RunSeed(IServiceScope scope)
    {
        var context = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/movies.json");

        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            moviesList = JsonSerializer.Deserialize<List<MovieDto>>(json);
        }

        if (moviesList != null) 
        { 
            foreach (var line in moviesList)
            {
                var movie = new Movie
                {
                    Title = line.Title,
                    Year = Convert.ToInt32(line.Year),
                    Rated = line.Rated,
                    Genre = line.Genre,
                    Plot = line.Plot,
                    Language = line.Language,
                    Country = line.Country,
                    Poster = line.Poster,
                    Ratings = line.Ratings,
                };

                LoadActors(scope, line?.Actors);
                LoadDirectors(scope, line?.Director);

                movie.Actors = actorsList.ToList();
                movie.Directors = directorsList.ToList();
                context?.Movies?.Add(movie);

                actorsList.Clear();
                directorsList.Clear();
            }
        }

        context?.SaveChanges();

        var ratings = context?.MovieRatings?.ToList();
        var directors = context?.MovieDirectors?.ToList();
        var mActors = context?.MovieActors?.ToList();
        var actors = context?.Actors?.ToList();
        var movies = context?.Movies?.ToList();
    }

    private static void LoadActors(IServiceScope scope, string actors)
    {
        var context = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
        actors.Split([","], StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => {
            var existentActor = context?.Actors?.Where(n => n.Name == x.Trim()).FirstOrDefault();
            if (existentActor != null)
                actorsList.Add(existentActor);
            else
            {
                var newActor = new Actor { Name = x.Trim() };
                context?.Actors?.Add(newActor);
                actorsList.Add(newActor);
            }
        });
    }

    private static void LoadDirectors(IServiceScope scope, string directors)
    {
        var context = scope.ServiceProvider.GetRequiredService<MovieDbContext>();
        directors.Split([","], StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => {
            var existentDirector = context?.Directors?.Where(n => n.Name == x.Trim()).FirstOrDefault();
            if (existentDirector != null)
                directorsList.Add(existentDirector);
            else
            {
                var newDirector = new Director { Name = x.Trim() };
                context?.Directors?.Add(newDirector);
                directorsList.Add(newDirector);
            }
        });
    }
}
