using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Domain.Models;
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
                    .AsNoTracking()
                    .OrderBy(m => m.Title)
                    .ToListAsync();
        }

        public async Task<PagedResult<Movie>> GetPaginatedAsync(string? param, PaginationParameters paginationParams, bool exactMatch = false)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var pattern = exactMatch ? param : $"%{param}%";
            var query = _context.Movies
                .Where(m => EF.Functions.Like(m.Title, $"%{param}%"))
                .Include(m => m.Actors)
                .Include(m => m.Directors)
                .Include(m => m.Ratings)
                .AsNoTracking()
                .OrderBy(m => m.Title);

            var totalCount = await query.CountAsync();
            var items = await query
               .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
               .Take(paginationParams.PageSize)
               .ToListAsync();

            return new PagedResult<Movie>
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
                
        public async Task<Movie?> GetByUniqueIdAsync(Guid uniqueId)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var movie = await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .Include(m => m.Ratings)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.UniqueId == uniqueId);

            return movie;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var existingMovie = await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .Include(m => m.Ratings)
                    .FirstOrDefaultAsync(m => m.UniqueId == movie.UniqueId) ?? new Movie();


            existingMovie.UniqueId = movie.UniqueId;
            existingMovie.Title = movie.Title;
            existingMovie.Country = movie.Country;
            existingMovie.Genre = movie.Genre;
            existingMovie.Language = movie.Language;
            existingMovie.Plot = movie.Plot;
            existingMovie.Poster = movie.Poster;
            existingMovie.Rated = movie.Rated;

            existingMovie.Actors.Clear();
            foreach (var actor in movie.Actors)
            {
                var existingActor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actor.Name) ?? new Actor { Name = actor.Name };
                existingMovie.Actors.Add(existingActor);
            }

            existingMovie.Directors.Clear();
            foreach (var director in movie.Directors)
            {
                var existingDirector = await _context.Directors.FirstOrDefaultAsync(d => d.Name == director.Name) ?? new Director { Name = director.Name };
                existingMovie.Directors.Add(existingDirector);
            }

            existingMovie.Ratings.Clear();
            foreach (var rating in movie.Ratings)
            {
                var existingRating = await _context.MovieRatings.FirstOrDefaultAsync(d => d.Source == rating.Source && d.Value == rating.Value) ?? new MovieRating { Source = rating.Source, Value = rating.Value };
                existingMovie.Ratings.Add(existingRating);
            }

            var model = _context.Movies.Update(existingMovie);
            bool updated = model?.State == EntityState.Modified;
            await _context.SaveChangesAsync();

            return model.Entity;
        }

        public async Task<bool> UpdateAsync(Movie movie)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");


            var existingMovie = await _context.Movies
                    .Include(m => m.Actors)
                    .Include(m => m.Directors)
                    .Include(m => m.Ratings)
                    .FirstOrDefaultAsync(m => m.UniqueId == movie.UniqueId);

            if (existingMovie == null)
                return false;

            existingMovie.UniqueId = movie.UniqueId;
            existingMovie.Title = movie.Title;
            existingMovie.Country = movie.Country;
            existingMovie.Genre = movie.Genre;
            existingMovie.Language = movie.Language;
            existingMovie.Plot = movie.Plot;
            existingMovie.Poster = movie.Poster;
            existingMovie.Rated = movie.Rated;

            existingMovie.Actors.Clear();
            foreach (var actor in movie.Actors)
            {
                var existingActor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actor.Name) ?? new Actor { Name = actor.Name };
                existingMovie.Actors.Add(existingActor);
            }

            existingMovie.Directors.Clear();
            foreach (var director in movie.Directors)
            {
                var existingDirector = await _context.Directors.FirstOrDefaultAsync(d => d.Name == director.Name) ?? new Director { Name = director.Name };
                existingMovie.Directors.Add(existingDirector);
            }

            existingMovie.Ratings.Clear();
            foreach (var rating in movie.Ratings)
            {
                var existingRating = await _context.MovieRatings.FirstOrDefaultAsync(d => d.Source == rating.Source && d.Value == rating.Value) ?? new MovieRating { Source = rating.Source, Value = rating.Value };
                existingMovie.Ratings.Add(existingRating);
            }

            var model = _context.Movies.Update(existingMovie);
            bool updated = model?.State == EntityState.Modified;
            await _context.SaveChangesAsync();
                
            return updated;
        }

        public async Task<bool> DeleteAsync(Guid uniqueId)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.UniqueId == uniqueId);

            if (movie == null)
                return false;

            var model = _context.Movies.Remove(movie);
            bool deleted = model.State == EntityState.Deleted;
            await _context.SaveChangesAsync();

            return deleted;
        }
    }
}
