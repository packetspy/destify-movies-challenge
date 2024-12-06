using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Application.Interfaces;

public interface IDirectorService
{
    Task<IEnumerable<DirectorDto>> GetAllAsync();
    Task<PagedResult<DirectorDto>> GetPaginatedAsync(string? param, PaginationParameters paginationParams);
    Task<Result<DirectorDto>?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Result<DirectorDto>> AddAsync(DirectorDto directorDto);
    Task<bool> UpdateAsync(Guid uniqueId, DirectorDto directorDto);
    Task<bool> DeleteAsync(Guid uniqueId);
}
