using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Application.Interfaces;

public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllAsync();
    Task<PagedResult<ActorDto>> GetPaginatedAsync(string? param, PaginationParameters paginationParams);
    Task<Result<ActorDto>?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Result<ActorDto>> AddAsync(ActorDto actorDto);
    Task<bool> UpdateAsync(Guid uniqueId, ActorDto actorDto);
    Task<bool> DeleteAsync(Guid uniqueId);
}
