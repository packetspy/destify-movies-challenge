using MoviesChallenge.Application.Dtos;

namespace MoviesChallenge.Application.Interfaces;

public interface IActorService
{
    Task<IEnumerable<ActorDto>> GetAllAsync();

    Task<IEnumerable<ActorDto>> SearchActorsAsync(string name);

    Task<ActorDto?> GetByUniqueIdAsync(Guid uniqueId);

    Task<ActorDto> AddAsync(ActorDto actorDto);

    Task<bool> UpdateAsync(Guid uniqueId, ActorDto actorDto);

    Task<bool> DeleteAsync(Guid uniqueId);
}
