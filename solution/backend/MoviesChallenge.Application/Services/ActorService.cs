using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Application.Services;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IMovieRepository _movieRepository;

    public ActorService(IActorRepository actorRepository, IMovieRepository movieRepository)
    {
        _actorRepository = actorRepository;
        _movieRepository = movieRepository;
    }

    public async Task<IEnumerable<ActorDto>> GetAllAsync()
    {
        var actors = await _actorRepository.GetAllAsync();
        return actors.Select(a => new ActorDto
        {
            UniqueId = a.UniqueId,
            Name = a.Name,
            Movies = a?.Movies?.Select(m => new MovieDto {
                UniqueId = m.UniqueId,
                Title = m.Title,
                Country = m.Country,
                Genre = m.Genre,
                Language = m.Language,
                Plot = m.Plot,
                Poster = m.Poster,
                Rated = m.Rated,
                Year = m.Year,
            }).ToList()
        });
    }
    public async Task<IEnumerable<ActorDto>> SearchActorsAsync(string name)
    {
        var actors = await _actorRepository.SearchByNameAsync(name);
        return actors.Select(a => new ActorDto
        {
            UniqueId = a.UniqueId,
            Name = a.Name,
            Movies = a?.Movies?.Select(m => new MovieDto
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
            }).ToList()
        });
    }

    public async Task<ActorDto?> GetByUniqueIdAsync(Guid uniqueId)
    {
        var movie = await _actorRepository.GetByUniqueIdAsync(uniqueId);
        return movie == null ? null : new ActorDto
        {
            UniqueId = movie.UniqueId,
            Name = movie.Name,
            Movies = movie?.Movies?.Select(m => new MovieDto
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
            }).ToList()
        };
    }

    public async Task<ActorDto> AddAsync(ActorDto actorDto)
    {
        var actor = new Actor
        {
            UniqueId = actorDto.UniqueId,
            Name = actorDto.Name ?? string.Empty,
            Movies = await GetMovies(actorDto.Movies),
        };

        actor = await _actorRepository.AddAsync(actor);
        return new ActorDto { UniqueId = actor.UniqueId, Name = actor.Name };
    }
    public async Task<bool> UpdateAsync(Guid uniqueId, ActorDto actorDto)
    {
        var actor = await _actorRepository.GetByUniqueIdAsync(uniqueId);
        if (actor == null) return false;

        actor.UniqueId = actorDto.UniqueId;
        actor.Name = actorDto.Name ?? string.Empty;
        actor.Movies = await GetMovies(actorDto.Movies);

        return await _actorRepository.UpdateAsync(actor);
    }

    public async Task<bool> DeleteAsync(Guid uniqueId)
    {
        return await _actorRepository.DeleteAsync(uniqueId);
    }


    private async Task<List<Movie>> GetMovies(List<MovieDto> movies)
    {
        HashSet<Movie> listMovies = [];
        if (movies == null) return new List<Movie>();

        foreach (var movie in movies)
        {
            var result = (await _movieRepository.SearchByTitleAsync(movie.Title, new PaginationParameters { Page = 1, PageSize = 100 }, true)).Data?.FirstOrDefault();
            if (result == null)
                listMovies.Add(new Movie { Title = movie.Title });
            else
                listMovies.Add(new Movie { Id = result.Id, UniqueId = result.UniqueId, Title = result.Title });
        }

        return listMovies.ToList();
    }
}
