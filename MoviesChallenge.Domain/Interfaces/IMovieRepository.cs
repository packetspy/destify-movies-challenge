using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Domain.Interfaces;

public interface IMovieRepository
{
    Task<Movie?> GetByUniqueIdAsync(Guid uniqueId);
    Task<List<Movie>> SearchByTitleAsync(string title);
    Task<List<Movie>> GetAllAsync();
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Guid uniqueId);
}
