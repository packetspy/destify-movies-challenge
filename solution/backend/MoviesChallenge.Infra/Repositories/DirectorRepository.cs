using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Domain.Models;
using MoviesChallenge.Infra.Data;

namespace MoviesChallenge.Infra.Repositories;

public class DirectorRepository : IDirectorRepository
{

    private readonly MovieDbContext _context;

    public DirectorRepository(MovieDbContext context)
    {
        _context = context;   
    }

    public async Task<List<Director>> GetAllAsync()
    {
        if (_context == null || _context.Directors == null)
            throw new Exception("Invalid Database");

       return await _context.Directors
                .Include(a => a.Movies)
                .AsNoTracking()
                .OrderBy(a => a.Name)
                .ToListAsync();  
    }

    public async Task<PagedResult<Director>> GetPaginatedAsync(string? param, PaginationParameters paginationParams, bool exactMatch = false)
    {
        if (_context == null || _context.Directors == null)
            throw new Exception("Invalid Database");

        var pattern = exactMatch ? param : $"%{param}%";
        var query = _context.Directors
            .Where(m => EF.Functions.Like(m.Name, pattern))
            .Include(a => a.Movies)
            .OrderBy(a => a.Name)
            .AsNoTracking();

        var totalCount = await query.CountAsync();
        var items = await query
               .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
               .Take(paginationParams.PageSize)
               .ToListAsync();

        return new PagedResult<Director>
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

    public async Task<PagedResult<Director>> SearchByNameAsync(string name, PaginationParameters paginationParams, bool exactMatch = false)
    {
        if (_context == null || _context.Directors == null)
            throw new Exception("Invalid Database");

        var pattern = exactMatch ? name : $"%{name}%";
        var query = _context.Directors
            .Where(m => EF.Functions.Like(m.Name, pattern))
            .Include(a => a.Movies)
            .OrderBy(a => a.Name)
            .AsNoTracking();

        var totalCount = await query.CountAsync();
        var items = await query
           .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
           .Take(paginationParams.PageSize)
           .ToListAsync();

        return new PagedResult<Director>
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

    public async Task<Director?> GetByUniqueIdAsync(Guid uniqueId)
    {
        if (_context == null || _context.Directors == null)
            throw new Exception("Invalid Database");

        var director = await _context.Directors
                .Include(a => a.Movies)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);

        return director;
    }

    public async Task<Director> AddAsync(Director director)
    {
        if (_context == null || _context.Directors == null)
            throw new Exception("Invalid Database");

        if(director.Movies.Count > 0)
        {
            HashSet<Movie> movieSet = new HashSet<Movie>();
            foreach (var movie in director.Movies)
            {
                var existntMovie = await _context?.Movies?.FirstOrDefaultAsync(x => x.UniqueId == movie.UniqueId);
                if (existntMovie != null)
                    movieSet.Add(existntMovie);
            }

            director.Movies.Clear();
            director.Movies = movieSet.ToList();
        }

        var model = await _context.Directors.AddAsync(director);
        await _context.SaveChangesAsync();
        return model.Entity;
    }

    public async Task<bool> UpdateAsync(Director director)
    {
        if (_context == null || _context.Actors == null)
            throw new Exception("Invalid Database");

        var existingDirector = await _context.Directors
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(x => x.UniqueId == director.UniqueId);

        if (existingDirector == null)
            return false;

        existingDirector.Name = director.Name;

        existingDirector.Movies.Clear();
        foreach (var movie in director.Movies)
        {
            var existingMovie = await _context.Movies.FirstOrDefaultAsync(d => d.UniqueId == movie.UniqueId) ?? new Movie { Title = movie.Title };
            existingDirector.Movies.Add(existingMovie);
        }

        var model = _context.Directors.Update(existingDirector);
        bool updated = model?.State == EntityState.Modified;
        await _context.SaveChangesAsync();

        return updated;
    }

    public async Task<bool> DeleteAsync(Guid uniqueId)
    {

        if (_context == null || _context.Directors == null)
            throw new Exception("Invalid Database");

        var director = await _context.Directors.FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

        if (director == null)
            return false;

        var model = _context.Directors.Remove(director);
        bool deleted = model.State == EntityState.Deleted;
        await _context.SaveChangesAsync();

        return deleted;
    }
}
