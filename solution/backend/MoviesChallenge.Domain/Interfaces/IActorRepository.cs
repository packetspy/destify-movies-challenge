using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Domain.Interfaces;

public interface IActorRepository
{
    Task<List<Actor>> GetAllAsync();
    Task<Actor?> GetByUniqueIdAsync(Guid uniqueId);
    Task<List<Actor>> SearchByNameAsync(string name, bool exactMatch = false);
    Task<Actor> AddAsync(Actor actor);
    Task<bool> UpdateAsync(Actor actor);
    Task<bool> DeleteAsync(Guid uniqueId);
}
