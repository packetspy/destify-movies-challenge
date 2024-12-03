using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;

namespace MoviesChallenge.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IActorRepository _actorRepository;

    public MovieService(IMovieRepository movieRepository, IActorRepository actorRepository)
    {
        _movieRepository = movieRepository;
        _actorRepository = actorRepository;
    }
    
    public async Task<IEnumerable<MovieDto>> GetAllAsync()
    {
        var movies = await _movieRepository.GetAllAsync();
        return movies.Select(m => new MovieDto
        {
            UniqueId = m.UniqueId,
            Title = m.Title,
            Country = m.Country,
            Genre = m.Genre,
            Language = m.Language,
            Plot = m.Plot,
            Poster = m.Poster,
            Rated = m.Rated,
            Year = m.Year,
            Actors = m.Actors.Select(a => a.Name).ToList(),
            Directors = m.Directors.Select(d => d.Name).ToList(),
            Ratings = m?.Ratings?.Select(r => new MovieRatingDto { Source = r.Source, Value = r.Value }).ToList()
        });
    }

    public async Task<IEnumerable<MovieDto>> SearchMoviesAsync(string title)
    {
        var movies = await _movieRepository.SearchByTitleAsync(title);
        return movies.Select(m => new MovieDto
        {
            UniqueId = m.UniqueId,
            Title = m.Title,
            Country = m.Country,
            Genre = m.Genre,
            Language = m.Language,
            Plot = m.Plot,
            Poster = m.Poster,
            Rated = m.Rated,
            Year = m.Year,
            Actors = m.Actors.Select(a => a.Name).ToList(),
            Directors = m.Directors.Select(d => d.Name).ToList(),
            Ratings = m?.Ratings?.Select(r => new MovieRatingDto { Source = r.Source, Value = r.Value }).ToList()
        });
    }
    
    public async Task<MovieDto?> GetByUniqueIdAsync(Guid uniqueId)
    {
        var movie = await _movieRepository.GetByUniqueIdAsync(uniqueId);
        return movie == null ? null : new MovieDto
        {
            UniqueId = movie.UniqueId,
            Title = movie.Title,
            Country = movie.Country,
            Genre = movie.Genre,
            Language = movie.Language,
            Plot = movie.Plot,
            Poster = movie.Poster,
            Rated = movie.Rated,
            Year = movie.Year,
            Actors = movie.Actors.Select(a => a.Name).ToList(),
            Directors = movie.Directors.Select(d => d.Name).ToList(),
            Ratings = movie?.Ratings?.Select(r => new MovieRatingDto { Source = r.Source, Value = r.Value }).ToList()
        };
    }

    public async Task<MovieDto> AddAsync(MovieDto movieDto)
    {
        var movie = new Movie
        {
            UniqueId = movieDto.UniqueId,
            Title = movieDto.Title,
            Country = movieDto.Country,
            Genre = movieDto.Genre,
            Language = movieDto.Language,
            Plot = movieDto.Plot,
            Poster = movieDto.Poster,
            Rated = movieDto.Rated,
            Year = movieDto.Year,
            Actors = await GetActors(movieDto.Actors),
            Directors = await GetDirectors(movieDto.Directors),
            Ratings = movieDto.Ratings.Select(r => new MovieRating { Source = r.Source, Value = r.Value }).ToList()
        };

        var updatedMovie = await _movieRepository.AddAsync(movie);
        return new MovieDto
        {
            UniqueId = updatedMovie.UniqueId,
            Title = updatedMovie.Title,
            Country = updatedMovie.Country,
            Genre = updatedMovie.Genre,
            Language = updatedMovie.Language,
            Plot = updatedMovie.Plot,
            Poster = updatedMovie.Poster,
            Rated = updatedMovie.Rated,
            Year = updatedMovie.Year,
            Actors = updatedMovie.Actors.Select(a => a.Name).ToList(),
            Directors = updatedMovie.Directors.Select(d => d.Name).ToList(),
            Ratings = updatedMovie?.Ratings?.Select(r => new MovieRatingDto { Source = r.Source, Value = r.Value }).ToList()
        };
    }
    
    public async Task<bool> UpdateAsync(Guid uniqueId, MovieDto movieDto)
    {
        var movie = await _movieRepository.GetByUniqueIdAsync(uniqueId);
        if (movie == null) return false;

        movie.UniqueId = movieDto.UniqueId;
        movie.Title = movieDto.Title;
        movie.Country = movieDto.Country;
        movie.Genre = movieDto.Genre;
        movie.Language = movieDto.Language;
        movie.Plot = movieDto.Plot;
        movie.Poster = movieDto.Poster;
        movie.Rated = movieDto.Rated;
        movie.Year = movieDto.Year;
        movie.Actors = await GetActors(movieDto.Actors);
        movie.Directors = await GetDirectors(movieDto.Directors);
        movie.Ratings = movieDto.Ratings.Select(r => new MovieRating { Source = r.Source, Value = r.Value }).ToList();

        return await _movieRepository.UpdateAsync(movie);
    }

    public async Task<bool> DeleteAsync(Guid uniqueId)
    {
        return await _movieRepository.DeleteAsync(uniqueId);
    }

    private async Task<List<Actor>> GetActors(List<string> actors)
    {
        HashSet<Actor> listActors = [];
        if (actors == null) return new List<Actor>();

        foreach (var actor in actors)
        {
            var result = (await _actorRepository.SearchByNameAsync(actor, true)).FirstOrDefault();
            if (result == null)
                listActors.Add(new Actor { Name = actor });
            else
                listActors.Add(new Actor { Id = result.Id, UniqueId = result.UniqueId, Name = result.Name });
        }

        return listActors.ToList();
    }

    private async Task<List<Director>> GetDirectors(List<string> directors)
    {
        HashSet<Director> listDirectors = [];
        if (directors == null) return new List<Director>();

        foreach (var director in directors)
        {
            var result = (await _actorRepository.SearchByNameAsync(director, true)).FirstOrDefault();
            if (result == null)
                listDirectors.Add(new Director { Name = director });
            else
                listDirectors.Add(new Director { Id = result.Id, UniqueId = result.UniqueId, Name = result.Name });
        }

        return listDirectors.ToList();
    }
}
