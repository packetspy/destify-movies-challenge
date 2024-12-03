using Microsoft.AspNetCore.Mvc;
using Moq;
using MoviesChallenge.Api.Controllers;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;

namespace MoviesChallenge.Tests.IntegrationTests;

public class MovieControllerUnitTests
{
    private readonly Mock<IMovieService> _mockService;
    private readonly MoviesController _controller;

    public MovieControllerUnitTests()
    {
        _mockService = new Mock<IMovieService>();
        _controller = new MoviesController(_mockService.Object);
    }

    [Fact]
    public async Task GetAllMovies_ReturnsOkWithMovies()
    {
        // Arrange
        var mockMovies = new List<Movie>
        {
            new Movie { Title = "Movie 1"},
            new Movie { Title = "Movie 2"}
        };
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockMovies);

        // Act
        var result = await _controller.GetAllMovies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedMovies = Assert.IsAssignableFrom<IEnumerable<Movie>>(okResult.Value);
        Assert.Equal(2, returnedMovies.Count());
    }

    [Fact]
    public async Task GetMovieById_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockService.Setup(s => s.GetByUniqueIdAsync(It.IsAny<Guid>())).ReturnsAsync((Movie?)null);

        // Act
        var result = await _controller.GetMovieByUniqueId(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateMovie_ReturnsCreatedAtAction_WhenSuccessful()
    {
        // Arrange
        var newMovie = new Movie { Title = "New Movie" };
        var createdMovie = new Movie { Title = "New Movie", Id = 1 };
        _mockService.Setup(s => s.AddAsync(newMovie)).ReturnsAsync(createdMovie);

        // Act
        var result = await _controller.CreateMovie(newMovie);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetMovieByUniqueId", createdResult.ActionName);
        Assert.Equal(1, ((Movie)createdResult.Value).Id);
    }
}