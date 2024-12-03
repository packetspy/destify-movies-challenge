using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Infra.Data;

namespace MoviesChallenge.Infra.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _context;

        public MovieRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            return await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .Include(m => m.Ratings)
                    .AsNoTracking().ToListAsync();
        }
        
        public async Task<Movie?> GetByUniqueIdAsync(Guid uniqueId)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var movie = await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .Include(m => m.Ratings)
                    .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);

            return movie;
        }
        
        public Task<List<Movie>> SearchByTitleAsync(string title)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Movie> AddAsync(Movie movie)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var model = await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return model.Entity;
        }

        public async Task<bool> UpdateAsync(Movie movie)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var originalMovie = await GetByUniqueIdAsync(movie.UniqueId);
            if (originalMovie == null)
                return false;

            movie.Id = originalMovie.Id;
            var model = _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            return model.State == EntityState.Modified;
        }
        
        public async Task<bool> DeleteAsync(Guid uniqueId)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var movie = await GetByUniqueIdAsync(uniqueId);

            if (movie == null)
                return false;

            var model = _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            
            return model.State == EntityState.Deleted;
        }
    }
}
