﻿using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Domain.Interfaces;

public interface IActorRepository
{
    Task<PagedResult<Actor>> GetAllAsync(PaginationParameters paginationParams);
    Task<PagedResult<Actor>> SearchByNameAsync(string name, PaginationParameters paginationParams, bool exactMatch = false);
    Task<Actor?> GetByUniqueIdAsync(Guid uniqueId);
    Task<Actor> AddAsync(Actor actor);
    Task<bool> UpdateAsync(Actor actor);
    Task<bool> DeleteAsync(Guid uniqueId);
}
