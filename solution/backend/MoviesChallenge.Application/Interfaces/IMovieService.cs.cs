﻿using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Application.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync();
    Task<PagedResult<MovieDto>> GetPaginatedAsync(string? param, PaginationParameters paginationParams);
    Task<Result<MovieDto>?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Result<MovieDto>> AddAsync(MovieDto movieDto);
    Task<bool> UpdateAsync(Guid uniqueId, MovieDto movieDto);
    Task<bool> DeleteAsync(Guid uniqueId);
}
