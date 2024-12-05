using Microsoft.AspNetCore.Mvc;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMovies()
    {
        var pagedResult = await _movieService.GetAllAsync();
        return Ok(pagedResult);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginatedMovies([FromQuery] PaginationParameters paginationParams)
    {
        var pagedResult = await _movieService.GetPaginatedAsync(paginationParams);
        return Ok(pagedResult);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies([FromQuery] string param, [FromQuery] PaginationParameters paginationParams)
    {
        if (string.IsNullOrWhiteSpace(param))
            return BadRequest("Movie title is required for search.");

        var movies = await _movieService.SearchMoviesAsync(param, paginationParams);
        return Ok(movies);
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetMovieByUniqueId(Guid uniqueId)
    {
        var movie = await _movieService.GetByUniqueIdAsync(uniqueId);
        return movie?.Data != null ? Ok(movie) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromBody] MovieDto movieDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdMovie = await _movieService.AddAsync(movieDto);
        return CreatedAtAction(nameof(GetMovieByUniqueId), new { uniqueId = createdMovie?.Data?.UniqueId }, createdMovie);
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateMovie(Guid uniqueId, [FromBody] MovieDto movieDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _movieService.UpdateAsync(uniqueId, movieDto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteMovie(Guid uniqueId)
    {
        var deleted = await _movieService.DeleteAsync(uniqueId);
        return deleted ? NoContent() : NotFound();
    }
}
