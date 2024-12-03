using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Application.Interfaces;

public interface IMovieService
{
    Task<List<Movie>> GetAllAsync();

    Task<Movie?> GetByUniqueIdAsync(Guid uniqueId);

    Task<Movie> AddAsync(Movie movie);

    Task<bool> UpdateAsync(Guid uniqueId, Movie movie);

    Task<bool> DeleteAsync(Guid uniqueId);
}
