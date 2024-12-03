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
    
    public async Task<List<Movie>> GetAllAsync()
    {
        return await _movieRepository.GetAllAsync();
    }
    
    public async Task<Movie?> GetByUniqueIdAsync(Guid uniqueId)
    {
        return await _movieRepository.GetByUniqueIdAsync(uniqueId);
    }
    
    public async Task<Movie> AddAsync(Movie movie)
    {
        return await _movieRepository.AddAsync(movie);
    }
    
    public async Task<bool> UpdateAsync(Guid uniqueId, Movie movie)
    {
        return await _movieRepository.UpdateAsync(movie);
    }

    public async Task<bool> DeleteAsync(Guid uniqueId)
    {
        return await _movieRepository.DeleteAsync(uniqueId);
    }
}
