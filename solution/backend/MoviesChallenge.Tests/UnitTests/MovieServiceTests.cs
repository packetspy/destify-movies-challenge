﻿using Moq;
using MoviesChallenge.Application.Services;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;

namespace MoviesChallenge.Tests.UnitTests;
public class MovieServiceTests
{
    [Fact]
    public async Task GetMovieByIdAsync_ShouldReturnMovie_WhenMovieExists()
    {
        // Arrange
        var mockRepo = new Mock<IMovieRepository>();
        mockRepo.Setup(repo => repo.GetByUniqueIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Movie { Id = 1, UniqueId = Guid.NewGuid(), Title = "Test Movie" });
        var service = new MovieService(mockRepo.Object);

        // Act
        var result = await service.GetMovieByUniqueIdAsync(Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Movie", result.Title);
    }
}