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
    public async Task<IActionResult> GetPaginatedMovies([FromQuery] string? query, [FromQuery] PaginationParameters paginationParams)
    {
        var pagedResult = await _movieService.GetPaginatedAsync(query, paginationParams);
        return Ok(pagedResult);
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
