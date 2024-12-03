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
        if (!_context.Movies.Any())
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data/movies.json");
            using (StreamReader stream = new StreamReader(path))
            {
                string json = stream.ReadToEnd();
                moviesList = JsonSerializer.Deserialize<List<RawDataDto>>(json);
            }

            if (moviesList != null)
            {
                foreach (var movieData in moviesList)
                {
                    var existingMovie = await _context.Movies
                        .Include(m => m.Actors)
                        .Include(m => m.Directors)
                        .Include(m => m.Ratings)
                        .FirstOrDefaultAsync(m => m.Title == movieData.Title && m.Year == Convert.ToInt32(movieData.Year));

                    if (existingMovie == null)
                    {
                        var movie = new Movie
                        {
                            Title = movieData.Title,
                            Year = Convert.ToInt32(movieData.Year),
                            Rated = movieData.Rated,
                            Genre = movieData.Genre,
                            Plot = movieData.Plot,
                            Language = movieData.Language,
                            Country = movieData.Country,
                            Poster = movieData.Poster
                        };

                        foreach (var actorName in movieData.Actors.Split(", "))
                        {
                            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorName);
                            if (actor == null)
                            {
                                actor = new Actor { Name = actorName };
                                _context.Actors.Add(actor);
                            }
                            movie.Actors.Add(actor);
                        }

                        foreach (var directorName in movieData.Director.Split(", "))
                        {
                            var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == directorName);
                            if (director == null)
                            {
                                director = new Director { Name = directorName };
                                _context.Directors.Add(director);
                            }
                            movie.Directors.Add(director);
                        }

                        foreach (var ratingData in movieData.Ratings)
                        {
                            movie.Ratings.Add(new MovieRating
                            {
                                Source = ratingData.Source,
                                Value = ratingData.Value
                            });
                        }

                        _context.Movies.Add(movie);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }
    }

}
