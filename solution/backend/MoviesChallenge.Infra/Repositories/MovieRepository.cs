﻿using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Infra.Data;
using System.Xml.Linq;

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
        
        public async Task<List<Movie>> SearchByTitleAsync(string title, bool exactMatch = false)
        {
            if (_context == null || _context.Movies == null)
                throw new Exception("Invalid Database");

            var pattern = exactMatch ? title : $"%{title}%";
            return await _context.Movies
            .Where(m => EF.Functions.Like(m.Title, $"%{title}%"))
            .AsNoTracking()
            .Include(m => m.Actors)
            .Include(m => m.Ratings)
            .Include(m => m.Directors)
            .ToListAsync();
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

            var originalMovie = await _context.Movies.FirstOrDefaultAsync(x => x.UniqueId == movie.UniqueId);
            if (originalMovie == null)
                return false;

            movie.Id = originalMovie.Id;
            var model = _context.Movies.Update(movie);
            bool updated = model.State == EntityState.Modified;
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
