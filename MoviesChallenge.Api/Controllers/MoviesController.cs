using Microsoft.AspNetCore.Mvc;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Entities;

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
        var movies = await _movieService.GetAllAsync();
        return Ok(movies);
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetMovieById(Guid uniqueId)
    {
        var movie = await _movieService.GetByUniqueIdAsync(uniqueId);
        return movie != null ? Ok(movie) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie([FromBody] Movie movie)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdMovie = await _movieService.AddAsync(movie);
        return CreatedAtAction(nameof(GetMovieById), new { uniqueId = createdMovie.UniqueId }, createdMovie);
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateMovie(Guid uniqueId, [FromBody] Movie movie)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _movieService.UpdateAsync(uniqueId, movie);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteMovie(Guid uniqueId)
    {
        var deleted = await _movieService.DeleteAsync(uniqueId);
        return deleted ? NoContent() : NotFound();
    }
}
