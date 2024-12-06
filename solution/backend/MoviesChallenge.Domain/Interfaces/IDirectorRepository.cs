using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Domain.Interfaces;

public interface IDirectorRepository
{
    Task<List<Director>> GetAllAsync();
    Task<PagedResult<Director>> GetPaginatedAsync(PaginationParameters paginationParams);
    Task<PagedResult<Director>> SearchByNameAsync(string name, PaginationParameters paginationParams, bool exactMatch = false);
    Task<Director?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Director> AddAsync(Director actor);
    Task<bool> UpdateAsync(Director actor);
    Task<bool> DeleteAsync(Guid uniqueId);
}
