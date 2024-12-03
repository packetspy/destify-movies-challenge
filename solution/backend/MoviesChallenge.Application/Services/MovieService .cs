using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;

namespace MoviesChallenge.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public Task<Movie> AddAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid uniqueId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Movie>> GetAllAsync()
    {
        return await _movieRepository.GetAllAsync();
    }

    public Task<Movie?> GetByUniqueIdAsync(Guid uniqueId)
    {
        throw new NotImplementedException();
    }

    public async Task<Movie?> GetMovieByUniqueIdAsync(Guid uniqueId)
    {
        return await _movieRepository.GetByUniqueIdAsync(uniqueId);
    }

    public Task<bool> UpdateAsync(Guid uniqueId, Movie movie)
    {
        throw new NotImplementedException();
    }
}
