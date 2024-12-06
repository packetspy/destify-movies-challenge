using Microsoft.AspNetCore.Mvc;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DirectorsController : ControllerBase
{
    private readonly IDirectorService _directorService;

    public DirectorsController(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActors()
    {
        var actors = await _directorService.GetAllAsync();
        return Ok(actors);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginatedActors([FromQuery] string? query, [FromQuery] PaginationParameters paginationParams)
    {
        var actors = await _directorService.GetPaginatedAsync(query, paginationParams);
        return Ok(actors);
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetActorByUniqueId(Guid uniqueId)
    {
        var actor = await _directorService.GetByUniqueIdAsync(uniqueId);
        return actor != null ? Ok(actor) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDirector([FromBody] DirectorDto directorDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdActor = await _directorService.AddAsync(directorDto);
        return CreatedAtAction(nameof(GetActorByUniqueId), new { uniqueId = createdActor.Data.UniqueId }, createdActor);
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateDirector(Guid uniqueId, [FromBody] DirectorDto directorDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _directorService.UpdateAsync(uniqueId, directorDto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteDirector(Guid uniqueId)
    {
        var deleted = await _directorService.DeleteAsync(uniqueId);
        return deleted ? NoContent() : NotFound();
    }
}
