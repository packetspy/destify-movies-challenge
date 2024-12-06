using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Domain.Interfaces;

public interface IDirectorRepository
{
    Task<List<Director>> GetAllAsync();
    Task<PagedResult<Director>> GetPaginatedAsync(string? param, PaginationParameters paginationParams, bool exactMatch = false);
    Task<Director?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Director> AddAsync(Director actor);
    Task<bool> UpdateAsync(Director actor);
    Task<bool> DeleteAsync(Guid uniqueId);
}
