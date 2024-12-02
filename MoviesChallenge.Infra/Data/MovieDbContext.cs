using Microsoft.EntityFrameworkCore;
using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Infra.Data;

public class MovieDbContext : DbContext
{
    public DbSet<Movie>? Movies { get; set; }
    public DbSet<Actor>? Actors { get; set; }
    public DbSet<Director>? Directors { get; set; }
    public DbSet<MovieActor>? MovieActors { get; set; }
    public DbSet<MovieDirector>? MovieDirectors { get; set; }
    public DbSet<MovieRating>? MovieRatings { get; set; }

    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
           .HasMany(m => m.Actors)
           .WithMany(a => a.Movies)
           .UsingEntity<MovieActor>();

        modelBuilder.Entity<Movie>()
           .HasMany(m => m.Directors)
           .WithMany(a => a.Movies)
           .UsingEntity<MovieDirector>();

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Ratings)
            .WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
