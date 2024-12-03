using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Domain.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<Movie?> GetByUniqueIdAsync(Guid uniqueId);
    Task<List<Movie>> SearchByTitleAsync(string title, bool exactMatch = false);
    Task<Movie> AddAsync(Movie movie);
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteAsync(Guid uniqueId);
}
