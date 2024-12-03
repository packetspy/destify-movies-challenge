using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Infra.Data;
using System.Net.Http.Json;

namespace MoviesChallenge.Tests.IntegrationTests;

public class MovieControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly MovieDbContext _context;

    public MovieControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _context = factory.Services.GetRequiredService<MovieDbContext>();
        SeedDatabase();
    }

    private void SeedDatabase()
    {
        if (_context == null || _context.Movies == null)
            throw new Exception("Invalid Database");

        _context?.Movies?.Add(new Movie { Id = 1, UniqueId = Guid.Parse("2d2b7758-c563-4a34-8ec2-1a6d4a0931f2"), Title = "Seed Movie" });
        _context?.SaveChanges();
    }

    [Fact]
    public async Task GetAllMovies_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/movies");
        response.EnsureSuccessStatusCode();
        var movies = await response.Content.ReadFromJsonAsync<List<Movie>>();
        Assert.NotEmpty(movies);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsMovie_WhenExists()
    {
        var response = await _client.GetAsync("/api/movies/2d2b7758-c563-4a34-8ec2-1a6d4a0931f2");
        response.EnsureSuccessStatusCode();
        var movie = await response.Content.ReadFromJsonAsync<Movie>();
        Assert.Equal("Seed Movie", movie.Title);
    }

    [Fact]
    public async Task CreateMovie_ReturnsCreated()
    {
        var newMovie = new { Title = "New Movie", ReleaseDate = DateTime.Now };
        var response = await _client.PostAsJsonAsync("/api/movies", newMovie);
        response.EnsureSuccessStatusCode();

        var createdMovie = await response.Content.ReadFromJsonAsync<Movie>();
        Assert.Equal("New Movie", createdMovie.Title);
    }
}