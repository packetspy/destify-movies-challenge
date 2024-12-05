using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Domain.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync();
    Task<PagedResult<Movie>> GetPaginatedAsync(PaginationParameters paginationParams);
    Task<PagedResult<Movie>> SearchByTitleAsync(string param, PaginationParameters paginationParams, bool exactMatch = false);
    Task<Movie?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Movie> AddAsync(Movie movie);
    Task<bool> UpdateAsync(Movie movie);
    Task<bool> DeleteAsync(Guid uniqueId);
}
