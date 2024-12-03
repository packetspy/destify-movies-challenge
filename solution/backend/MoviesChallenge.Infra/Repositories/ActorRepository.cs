using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Infra.Data;

namespace MoviesChallenge.Infra.Repositories;

public class ActorRepository : IActorRepository
{

    private readonly MovieDbContext _context;

    public ActorRepository(MovieDbContext context)
    {
        _context = context;   
    }

    public async Task<List<Actor>> GetAllAsync()
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        return await _context.Actors
                .Include(a => a.Movies)
                .AsNoTracking().ToListAsync();
    }

    public async Task<Actor?> GetByUniqueIdAsync(Guid uniqueId)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var actor = await _context.Actors
                .Include(a => a.Movies)
                .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);

        return actor;
    }

    public async Task<List<Actor>> SearchByNameAsync(string name, bool exactMatch = false)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var pattern = exactMatch ? name : $"%{name}%";
        return await _context.Actors
            .Where(m => EF.Functions.Like(m.Name, pattern))
            .AsNoTracking()
            .Include(a => a.Movies)
            .ToListAsync();
    }

    public async Task<Actor> AddAsync(Actor actor)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var model = await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();
        return model.Entity;
    }

    public async Task<bool> UpdateAsync(Actor actor)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var originalActor = await _context.Actors.FirstOrDefaultAsync(x => x.UniqueId == actor.UniqueId);
        if (originalActor == null)
            return false;

        actor.Id = originalActor.Id;
        var model = _context.Actors.Update(actor);
        bool updated = model.State == EntityState.Modified;
        await _context.SaveChangesAsync();

        return updated;
    }

    public async Task<bool> DeleteAsync(Guid uniqueId)
    {

        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var actor = await _context.Actors.FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

        if (actor == null)
            return false;

        var model = _context.Actors.Remove(actor);
        bool deleted = model.State == EntityState.Deleted;
        await _context.SaveChangesAsync();

        return deleted;
    }
}
