using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;
using MoviesChallenge.Domain.Interfaces;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Application.Services;

public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository _directorRepository;
    private readonly IMovieRepository _movieRepository;

    public DirectorService(IDirectorRepository directorRepository, IMovieRepository movieRepository)
    {
        _directorRepository = directorRepository;
        _movieRepository = movieRepository;
    }

    public async Task<IEnumerable<DirectorDto>> GetAllAsync()
    {
        var directors = await _directorRepository.GetAllAsync();

        return directors.Select(a => new DirectorDto
        {
            UniqueId = a.UniqueId,
            Name = a.Name,
            Movies = a?.Movies?.Select(m => new MovieDto
            {
                UniqueId = m.UniqueId,
                Title = m.Title,
                Country = m.Country,
                Genre = m.Genre,
                Language = m.Language,
                Plot = m.Plot,
                Poster = m.Poster,
                Rated = m.Rated,
                Year = m.Year,
            }).ToList()
        });
    }

    public async Task<PagedResult<DirectorDto>> GetPaginatedAsync(string? param, PaginationParameters paginationParams)
    {
        var pagedDirectors = await _directorRepository.GetPaginatedAsync(param, paginationParams);
        return new PagedResult<DirectorDto>
        {
            Data = pagedDirectors?.Data?.Select(a => new DirectorDto
            {
                UniqueId = a.UniqueId,
                Name = a.Name,
                Movies = a?.Movies?.Select(m => new MovieDto
                {
                    UniqueId = m.UniqueId,
                    Title = m.Title,
                    Country = m.Country,
                    Genre = m.Genre,
                    Language = m.Language,
                    Plot = m.Plot,
                    Poster = m.Poster,
                    Rated = m.Rated,
                    Year = m.Year,
                }).ToList()
            }),
            Meta = pagedDirectors?.Meta != null ? new PagedMetadata
            {
                Page = paginationParams.Page,
                PageSize = paginationParams.PageSize,
                TotalCount = pagedDirectors.Meta.TotalCount,
                TotalPages = pagedDirectors.Meta.TotalPages
            } : new PagedMetadata { Page = 1, PageSize = 1, TotalCount = 1, TotalPages = 1 }
        };
    }

    public async Task<Result<DirectorDto>?> GetByUniqueIdAsync(Guid uniqueId)
    {
        var director = await _directorRepository.GetByUniqueIdAsync(uniqueId);
        return director == null ? null : new Result<DirectorDto>
        {
            Data = new DirectorDto
            {
                UniqueId = director.UniqueId,
                Name = director.Name,
                Movies = director?.Movies?.Select(m => new MovieDto
                {
                    UniqueId = m.UniqueId,
                    Title = m.Title,
                    Country = m.Country,
                    Genre = m.Genre,
                    Language = m.Language,
                    Plot = m.Plot,
                    Poster = m.Poster,
                    Rated = m.Rated,
                    Year = m.Year,
                }).ToList()
            }
        };
    }

    public async Task<Result<DirectorDto>> AddAsync(DirectorDto directorDto)
    {
        var director = new Director
        {
            UniqueId = directorDto.UniqueId,
            Name = directorDto.Name ?? string.Empty,
            Movies = await GetMovies(directorDto.Movies),
        };

        director = await _directorRepository.AddAsync(director);
        return new Result<DirectorDto>
        {
            Data = new DirectorDto
            {
                UniqueId = director.UniqueId,
                Name = director.Name,
                Movies = director.Movies.Select(m => new MovieDto { UniqueId = m.UniqueId, Title = m.Title }).ToList(),
            }
        };
    }
    public async Task<bool> UpdateAsync(Guid uniqueId, DirectorDto directorDto)
    {
        var director = await _directorRepository.GetByUniqueIdAsync(uniqueId);
        if (director == null) return false;

        director.UniqueId = directorDto.UniqueId;
        director.Name = directorDto.Name ?? string.Empty;
        director.Movies = await GetMovies(directorDto.Movies);

        return await _directorRepository.UpdateAsync(director);
    }

    public async Task<bool> DeleteAsync(Guid uniqueId)
    {
        return await _directorRepository.DeleteAsync(uniqueId);
    }

    private async Task<List<Movie>> GetMovies(List<MovieDto> movies)
    {
        HashSet<Movie> listMovies = new HashSet<Movie>();
        if (movies == null) return new List<Movie>();

        foreach (var movie in movies)
        {
            var result = (await _movieRepository.GetPaginatedAsync(movie.Title, new PaginationParameters { Page = 1, PageSize = 100 }, true)).Data?.FirstOrDefault();
            if (result == null)
                listMovies.Add(new Movie { Title = movie.Title });
            else
                listMovies.Add(new Movie { Id = result.Id, UniqueId = result.UniqueId, Title = result.Title });
        }

        return listMovies.ToList();
    }
}
