using Microsoft.AspNetCore.Mvc;
using Moq;
using MoviesChallenge.Api.Controllers;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Tests.UnitTests;

public class MoviesControllerUnitTests
{
    private readonly Mock<IMovieService> _mockMovieService;
    private readonly MoviesController _controller;
    private readonly PaginationParameters paginationParams = new PaginationParameters { Page = 1, PageSize = 100 };

    public MoviesControllerUnitTests()
    {
        _mockMovieService = new Mock<IMovieService>();
        _controller = new MoviesController(_mockMovieService.Object);
    }

    [Fact]
    public async Task GetAllMovies_ReturnsOkResult_WithListOfMovies()
    {
        // Arrange
        var movies = new List<MovieDto> { new MovieDto { UniqueId = Guid.NewGuid(), Title = "Test Movie" } };
        _mockMovieService.Setup(service => service.GetAllAsync()).ReturnsAsync(movies);

        // Act
        var result = await _controller.GetAllMovies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnMovies = Assert.IsType<List<MovieDto>>(okResult.Value);
        Assert.Single(returnMovies);
    }

    [Fact]
    public async Task SearchMovies_ReturnsBadRequest_WhenTitleIsNullOrEmpty()
    {
        // Act
        var result = await _controller.SearchMovies(string.Empty, paginationParams);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Movie title is required for search.", badRequestResult.Value);
    }

    [Fact]
    public async Task SearchMovies_ReturnsOkResult_WithListOfMovies()
    {
        // Arrange
        var movies = new List<MovieDto> { new MovieDto { UniqueId = Guid.NewGuid(), Title = "Test Movie" } };
        _mockMovieService.Setup(service => service.SearchMoviesAsync(It.IsAny<string>(), It.IsAny<PaginationParameters>())).ReturnsAsync(new PagedResult<MovieDto>
        {
            Data = movies.Skip(0).Take(10),
            Meta = new PagedMetadata
            {
                TotalCount = movies.Count,
                Page = 1,
                PageSize = 10
            }
        });

        // Act
        var result = await _controller.SearchMovies("Test", paginationParams);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnMovies = Assert.IsType<PagedResult<MovieDto>>(okResult.Value);
        Assert.Single(returnMovies?.Data);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Act
        var result = await _controller.GetMovieByUniqueId(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetMovieByUniqueId_ReturnsOkResult_WithMovie()
    {
        // Arrange
        var movie = new MovieDto { UniqueId = Guid.NewGuid(), Title = "Test Movie" };
        _mockMovieService.Setup(service => service.GetByUniqueIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Result<MovieDto> { Data = movie });

        // Act
        var result = await _controller.GetMovieByUniqueId(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnMovie = Assert.IsType<Result<MovieDto>>(okResult.Value);
        Assert.Equal(movie.UniqueId, returnMovie?.Data?.UniqueId);
    }

    [Fact]
    public async Task CreateMovie_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.CreateMovie(new MovieDto());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreateMovie_ReturnsCreatedAtAction_WithCreatedMovie()
    {
        // Arrange
        var movie = new MovieDto { UniqueId = Guid.NewGuid(), Title = "Test Movie" };
        var resultMovie = new Result<MovieDto> { Data = movie };
        _mockMovieService.Setup(service => service.AddAsync(It.IsAny<MovieDto>())).ReturnsAsync(resultMovie);

        // Act
        var result = await _controller.CreateMovie(movie);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnMovie = Assert.IsType<Result<MovieDto>>(createdAtActionResult.Value);
        Assert.Equal(movie.UniqueId, returnMovie.Data?.UniqueId);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _controller.UpdateMovie(Guid.NewGuid(), new MovieDto());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        _mockMovieService.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<MovieDto>())).ReturnsAsync(true);

        // Act
        var result = await _controller.UpdateMovie(Guid.NewGuid(), new MovieDto());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockMovieService.Setup(service => service.UpdateAsync(It.IsAny<Guid>(), It.IsAny<MovieDto>())).ReturnsAsync(false);

        // Act
        var result = await _controller.UpdateMovie(Guid.NewGuid(), new MovieDto());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteMovie_ReturnsNoContent_WhenDeleteIsSuccessful()
    {
        // Arrange
        _mockMovieService.Setup(service => service.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteMovie(Guid.NewGuid());

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteMovie_ReturnsNotFound_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockMovieService.Setup(service => service.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteMovie(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
