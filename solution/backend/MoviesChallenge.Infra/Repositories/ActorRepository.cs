using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Domain.Models;
using MoviesChallenge.Infra.Data;
using System.IO;
using System.Xml.Linq;

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
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .ToListAsync();  
    }

    public async Task<PagedResult<Actor>> GetPaginatedAsync(string? param, PaginationParameters paginationParams, bool exactMatch = false)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");
        var pattern = exactMatch ? param : $"%{param}%";
        var query = _context.Actors
                .Where(m => EF.Functions.Like(m.Name, pattern))
                .Include(a => a.Movies)
                .OrderBy(a => a.Name)
                .AsNoTracking();

        var totalCount = await query.CountAsync();
        var items = await query
               .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
               .Take(paginationParams.PageSize)
               .ToListAsync();

        return new PagedResult<Actor>
        {
            Data = items,
            Meta = new PagedMetadata
            {
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize),
                Page = paginationParams.Page,
                PageSize = paginationParams.PageSize
            }
        };
    }

    public async Task<Actor?> GetByUniqueIdAsync(Guid uniqueId)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var actor = await _context.Actors
                .Include(a => a.Movies)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);

        return actor;
    }

    public async Task<Actor> AddAsync(Actor actor)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        if (actor.Movies.Count > 0)
        {
            HashSet<Movie> movieSet = new HashSet<Movie>();
            foreach (var movie in actor.Movies)
            {
                var existntMovie = await _context?.Movies?.FirstOrDefaultAsync(x => x.UniqueId == movie.UniqueId);
                if (existntMovie != null)
                    movieSet.Add(existntMovie);
            }

            actor.Movies.Clear();
            actor.Movies = movieSet.ToList();
        }

        var model = await _context.Actors.AddAsync(actor);
        await _context.SaveChangesAsync();
        return model.Entity;
    }

    public async Task<bool> UpdateAsync(Actor actor)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var existingActor = await _context.Actors
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(x => x.UniqueId == actor.UniqueId);

        if (existingActor == null)
            return false;

        existingActor.Name = actor.Name;

        existingActor.Movies.Clear();
        foreach (var movie in actor.Movies)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(d => d.UniqueId == movie.UniqueId) ?? new Movie { Title = movie.Title };
            existingActor.Movies.Add(existingMovie);
        }

        var model = _context.Actors.Update(existingActor);
        bool updated = model?.State == EntityState.Modified;
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
