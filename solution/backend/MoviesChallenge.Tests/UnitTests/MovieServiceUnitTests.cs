using Microsoft.AspNetCore.Mvc;
using Moq;
using MoviesChallenge.Api.Controllers;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Application.Services;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;

namespace MoviesChallenge.Tests.UnitTests;
public class MovieServiceUnitTests
{
    private readonly Mock<IMovieRepository> _mockMovieRepository;
    private readonly Mock<IActorRepository> _mockActorRepository;
    private readonly MovieService _movieService;

    public MovieServiceUnitTests()
    {
        _mockMovieRepository = new Mock<IMovieRepository>();
        _mockActorRepository = new Mock<IActorRepository>();
        _movieService = new MovieService(_mockMovieRepository.Object, _mockActorRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMovies()
    {
        // Arrange
        var movies = new List<Movie> { new Movie { Title = "Movie1" }, new Movie { Title = "Movie2" } };
        _mockMovieRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(movies);

        // Act
        var result = await _movieService.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchMoviesAsync_ShouldReturnMatchingMovies()
    {
        // Arrange
        var movies = new List<Movie> { new Movie { Title = "Movie1" }, new Movie { Title = "Movie2" } };
        _mockMovieRepository.Setup(repo => repo.SearchByTitleAsync("Movie1", false)).ReturnsAsync(movies.Where(m => m.Title == "Movie1").ToList());

        // Act
        var result = await _movieService.SearchMoviesAsync("Movie1");

        // Assert
        Assert.Single(result);
        Assert.Equal("Movie1", result.First().Title);
    }

    [Fact]
    public async Task GetByUniqueIdAsync_ShouldReturnMovie_WhenMovieExists()
    {
        // Arrange
        var movie = new Movie { UniqueId = Guid.NewGuid(), Title = "Movie1" };
        _mockMovieRepository.Setup(repo => repo.GetByUniqueIdAsync(movie.UniqueId)).ReturnsAsync(movie);

        // Act
        var result = await _movieService.GetByUniqueIdAsync(movie.UniqueId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(movie.Title, result.Title);
    }

    [Fact]
    public async Task GetByUniqueIdAsync_ShouldReturnNull_WhenMovieDoesNotExist()
    {
        // Arrange
        _mockMovieRepository.Setup(repo => repo.GetByUniqueIdAsync(It.IsAny<Guid>())).ReturnsAsync((Movie)null);

        // Act
        var result = await _movieService.GetByUniqueIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMovie()
    {
        // Arrange
        var movieDto = new MovieDto { Title = "Movie 1" };
        _mockMovieRepository.Setup(repo => repo.AddAsync(It.IsAny<Movie>())).ReturnsAsync(new Movie());

        // Act
        var result = await _movieService.AddAsync(movieDto);

        // Assert
        Assert.Equal(movieDto.Title, result.Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMovie_WhenMovieExists()
    {
        // Arrange
        var movie = new Movie { UniqueId = Guid.NewGuid(), Title = "Movie1" };
        var movieDto = new MovieDto { Title = "UpdatedMovie" };
        _mockMovieRepository.Setup(repo => repo.GetByUniqueIdAsync(movie.UniqueId)).ReturnsAsync(movie);
        _mockMovieRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Movie>())).ReturnsAsync(true);

        // Act
        var result = await _movieService.UpdateAsync(movie.UniqueId, movieDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenMovieDoesNotExist()
    {
        // Arrange
        var movieDto = new MovieDto { Title = "UpdatedMovie" };
        _mockMovieRepository.Setup(repo => repo.GetByUniqueIdAsync(It.IsAny<Guid>())).ReturnsAsync((Movie)null);

        // Act
        var result = await _movieService.UpdateAsync(Guid.NewGuid(), movieDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenMovieIsDeleted()
    {
        // Arrange
        _mockMovieRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        // Act
        var result = await _movieService.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenMovieIsNotDeleted()
    {
        // Arrange
        _mockMovieRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        // Act
        var result = await _movieService.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }
}