using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Domain.Models;
using System.Net.Http.Json;

namespace MoviesChallenge.Tests.IntegrationTests;

public class ActorsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;
    private readonly HttpClient _authenticatedClient;

    public ActorsControllerIntegrationTests(WebApplicationFactory<Program> fixture)
    {
        _configuration = fixture.Services.GetRequiredService<IConfiguration>();
        _client = fixture.CreateClient();
        _authenticatedClient = fixture.CreateClient();
        _authenticatedClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configuration["ApiSecret"]}");
    }

    [Fact]
    public async Task GetAllActors_ReturnsOkResult_WithListOfActors()
    {
        // Act
        var response = await _client.GetAsync("/api/actors");

        // Assert
        response.EnsureSuccessStatusCode();
        var actors = await response.Content.ReadFromJsonAsync<List<ActorDto>>();
        Assert.NotNull(actors);
    }

    [Fact]
    public async Task SearchActors_ReturnsBadRequest_WhenTitleIsNullOrEmpty()
    {
        // Act
        var response = await _client.GetAsync("/api/actors/search?name=");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SearchActors_ReturnsOkResult_WithListOfActors()
    {
        // Act
        var response = await _client.GetAsync("/api/actors/search?param=Test");

        // Assert
        response.EnsureSuccessStatusCode();
        var actors = await response.Content.ReadFromJsonAsync<PagedResult<ActorDto>>();
        Assert.NotNull(actors);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync($"/api/actors/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsOkResult_WithMovie()
    {
        // Arrange
        var actorDto = new ActorDto { UniqueId = Guid.NewGuid(), Name = "Actor 1" };
        var result = await _authenticatedClient.PostAsJsonAsync("/api/actors", actorDto);
        var createdActor = await result.Content.ReadFromJsonAsync<Result<ActorDto>>();
        actorDto.UniqueId = createdActor.Data.UniqueId;

        // Act
        var response = await _client.GetAsync($"/api/actors/{createdActor.Data.UniqueId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var returnMovie = await response.Content.ReadFromJsonAsync<Result<ActorDto>>();
        Assert.Equal(actorDto.UniqueId, returnMovie?.Data?.UniqueId);
    }

    [Fact]
    public async Task CreateActor_ReturnsCreatedAtAction_WithCreatedMovie()
    {
        // Arrange
        var actorDto = new ActorDto { Name = "Actor 1" };

        // Act
        var response = await _authenticatedClient.PostAsJsonAsync("/api/actors", actorDto);
        var createdActor = await response.Content.ReadFromJsonAsync<ActorDto>();
        actorDto.UniqueId = createdActor.UniqueId;

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(actorDto.UniqueId, createdActor.UniqueId);
    }

    [Fact]
    public async Task UpdateActor_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var actorDto = new ActorDto { Name = "Actor Name" };
        var result = await _authenticatedClient.PostAsJsonAsync("/api/actors", actorDto);
        var createdActor = await result.Content.ReadFromJsonAsync<Result<ActorDto>>();
        actorDto.UniqueId = createdActor.Data.UniqueId;
        actorDto.Name = "Actor Updated";

        // Act
        var response = await _authenticatedClient.PutAsJsonAsync($"/api/actors/{createdActor.Data.UniqueId}", actorDto);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateActor_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        var actorDto = new ActorDto { UniqueId = Guid.NewGuid(), Name = "Actor Name" };

        // Act
        var response = await _authenticatedClient.PutAsJsonAsync($"/api/actors/{Guid.NewGuid()}", actorDto);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteActor_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        var actorDto = new ActorDto { Name = "Actor 1" };
        var result = await _authenticatedClient.PostAsJsonAsync("/api/actors", actorDto);
        var createdActor = await result.Content.ReadFromJsonAsync<Result<ActorDto>>();

        // Act
        var response = await _authenticatedClient.DeleteAsync($"/api/actors/{createdActor.Data.UniqueId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteActor_ReturnsNoContent_WhenDeleteIsUnauthorized()
    {
        // Arrange
        var actorDto = new ActorDto { UniqueId = Guid.NewGuid(), Name = "Actor Name" };
        await _client.PostAsJsonAsync("/api/actors", actorDto);

        // Act
        var response = await _client.DeleteAsync($"/api/actors/{actorDto.UniqueId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DeleteActor_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Act
        var response = await _authenticatedClient.DeleteAsync($"/api/actors/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
