using MoviesChallenge.Application.Dtos;

namespace MoviesChallenge.Application.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieDto>> GetAllAsync();

    Task<IEnumerable<MovieDto>> SearchMoviesAsync(string title);

    Task<MovieDto?> GetByUniqueIdAsync(Guid uniqueId);

    Task<MovieDto> AddAsync(MovieDto movieDto);

    Task<bool> UpdateAsync(Guid uniqueId, MovieDto movieDto);

    Task<bool> DeleteAsync(Guid uniqueId);
}
