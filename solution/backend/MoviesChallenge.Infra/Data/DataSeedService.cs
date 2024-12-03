using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Infra.Dtos;
using System.Reflection;
using System.Text.Json;

namespace MoviesChallenge.Infra.Data;

public class DataSeedService : IDataSeedService
{
    private readonly MovieDbContext _context;
    private static List<RawDataDto> moviesList = new List<RawDataDto>();
    private static HashSet<Actor> actorsList = new HashSet<Actor>();
    private static HashSet<Director> directorsList = new HashSet<Director>();
    private static HashSet<Movie> movieList = new HashSet<Movie>();

    public DataSeedService(MovieDbContext context)
    {
        _context = context;
    }

    public async Task RunSeedAsync()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/movies.json");

        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            moviesList = JsonSerializer.Deserialize<List<RawDataDto>>(json);
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

                LoadActors(line?.Actors);
                LoadDirectors(line?.Director);

                movie.Actors = actorsList.ToList();
                movie.Directors = directorsList.ToList();

                movieList.Add(movie);
                actorsList.Clear();
                directorsList.Clear();
            }
        }

        if (!await _context.Movies.AnyAsync())
            _context?.Movies?.AddRangeAsync(movieList);

        await _context?.SaveChangesAsync();
    }

    private void LoadActors(string actors)
    {
        actors.Split([","], StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => {
            var existentActor = _context?.Actors?.Where(n => n.Name == x.Trim()).FirstOrDefault();
            if (existentActor != null)
                actorsList.Add(existentActor);
            else
            {
                var newActor = new Actor { Name = x.Trim() };
                _context?.Actors?.Add(newActor);
                actorsList.Add(newActor);
            }
        });
    }

    private void LoadDirectors(string directors)
    {
        directors.Split([","], StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(x => {
            var existentDirector = _context?.Directors?.Where(n => n.Name == x.Trim()).FirstOrDefault();
            if (existentDirector != null)
                directorsList.Add(existentDirector);
            else
            {
                var newDirector = new Director { Name = x.Trim() };
                _context?.Directors?.Add(newDirector);
                directorsList.Add(newDirector);
            }
        });
    }
}
