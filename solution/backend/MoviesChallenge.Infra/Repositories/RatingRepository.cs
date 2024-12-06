using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Infra.Data;

namespace MoviesChallenge.Infra.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly MovieDbContext _context; 
    
    public RatingRepository(MovieDbContext context)
    {
        _context = context;  
    }
    public async Task<MovieRating?> GetBySourceAndValue(string source, string value)
    {
        if (_context == null || _context.Movies == null)
            throw new Exception("Invalid Database");

        var rating = await _context.MovieRatings
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Source == source && m.Value == value);

        return rating;
    }
}
