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

        public Task AddAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid uniqueId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public async Task<Movie?> GetByUniqueIdAsync(Guid uniqueId)
        {
            var movie = new Movie();
            if (_context is not null && _context.Movies is not null)
                movie = await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);

            return movie;
        }

        public async Task<List<Movie>> GetAllAsync()
        {
            if (_context is not null && _context.Movies is not null)
                return await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .AsNoTracking().ToListAsync();

            return [];
        }

        public Task<List<Movie>> SearchByTitleAsync(string title)
        {
            throw new NotImplementedException();
        }
    }
}
