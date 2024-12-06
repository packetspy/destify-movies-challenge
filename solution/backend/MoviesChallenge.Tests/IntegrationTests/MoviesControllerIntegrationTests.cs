using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Domain.Models;
using System.Net;
using System.Net.Http.Json;

namespace MoviesChallenge.Tests.IntegrationTests;

public class MoviesControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;
    private readonly HttpClient _authenticatedClient;

    public MoviesControllerIntegrationTests(WebApplicationFactory<Program> fixture)
    {
        _configuration = fixture.Services.GetRequiredService<IConfiguration>();
        _client = fixture.CreateClient();
        _authenticatedClient = fixture.CreateClient();
        _authenticatedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["ApiSecret"]}");
    }

    [Fact]
    public async Task GetAllMovies_ReturnsOkResult_WithListOfMovies()
    {
        // Act
        var response = await _client.GetAsync("/api/movies");

        // Assert
        response.EnsureSuccessStatusCode();
        var movies = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
        Assert.NotNull(movies);
    }

    [Fact]
    public async Task SearchMovies_ReturnsOkResult_WithListOfMovies()
    {
        // Act
        var response = await _client.GetAsync("/api/movies/search?param=Test");

        // Assert
        response.EnsureSuccessStatusCode();
        var movies = await response.Content.ReadFromJsonAsync<PagedResult<MovieDto>>();
        Assert.NotNull(movies);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"/api/movies/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsOkResult_WithMovie()
    {
        // Arrange
        var movie = new MovieDto { Title = "Test Movie" };
        var result = await _authenticatedClient.PostAsJsonAsync("/api/movies", movie);
        var createdMovie = await result.Content.ReadFromJsonAsync<Result<MovieDto>>();

        // Act
        var response = await _client.GetAsync($"/api/movies/{createdMovie?.Data?.UniqueId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var returnMovie = await response.Content.ReadFromJsonAsync<Result<MovieDto>>();
        Assert.Equal(createdMovie?.Data?.UniqueId, returnMovie?.Data?.UniqueId);
    }

    [Fact]
    public async Task CreateMovie_ReturnsCreatedAtAction_WithCreatedMovie()
    {
        // Arrange
        var movie = new MovieDto { Title = "Test Movie 123" };

        // Act
        var response = await _authenticatedClient.PostAsJsonAsync("/api/movies", movie);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdMovie = await response.Content.ReadFromJsonAsync<Result<MovieDto>>();
        Assert.Equal(movie.Title, createdMovie?.Data?.Title);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var movie = new MovieDto { Title = null };

        // Act
        var response = await _authenticatedClient.PutAsJsonAsync($"/api/movies/a", movie);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var movie = new MovieDto { Title = "Test Movie" };
        var result = await _authenticatedClient.PostAsJsonAsync("/api/movies", movie);
        var createdMovie = await result.Content.ReadFromJsonAsync<Result<MovieDto>>();
        movie.UniqueId = createdMovie.Data.UniqueId;
        movie.Title = "Updated Test Movie";

        // Act
        var response = await _authenticatedClient.PutAsJsonAsync($"/api/movies/{createdMovie.Data.UniqueId}", movie);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        var movie = new MovieDto { Title = "Test Movie" };

        // Act
        var response = await _authenticatedClient.PutAsJsonAsync($"/api/movies/{Guid.NewGuid()}", movie);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        var movie = new MovieDto { Title = "Test Movie" };
        var result = await _authenticatedClient.PostAsJsonAsync("/api/movies", movie);
        var createdMovie = await result.Content.ReadFromJsonAsync<Result<MovieDto>>();

        // Act
        var response = await _authenticatedClient.DeleteAsync($"/api/movies/{createdMovie?.Data?.UniqueId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Act
        var response = await _authenticatedClient.DeleteAsync($"/api/movies/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
