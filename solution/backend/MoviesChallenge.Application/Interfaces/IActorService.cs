using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Application.Interfaces;

public interface IActorService
{
    Task<PagedResult<ActorDto>> GetAllAsync(PaginationParameters paginationParams);

    Task<PagedResult<ActorDto>> SearchActorsAsync(string param, PaginationParameters paginationParams);

    Task<Result<ActorDto>?> GetByUniqueIdAsync(Guid uniqueId);

    Task<Result<ActorDto>> AddAsync(ActorDto actorDto);

    Task<bool> UpdateAsync(Guid uniqueId, ActorDto actorDto);

    Task<bool> DeleteAsync(Guid uniqueId);
}
