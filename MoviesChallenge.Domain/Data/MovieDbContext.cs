using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Domain.Data;

public class MovieDbContext : DbContext
{
    public DbSet<Movie>? Movies { get; set; }
    public DbSet<Actor>? Actors { get; set; }
    public DbSet<MovieRating>? MovieRatings { get; set; }

    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }
}
