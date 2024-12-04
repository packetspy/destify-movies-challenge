using Microsoft.AspNetCore.Mvc;
using MoviesChallenge.Application.Dtos;
using MoviesChallenge.Application.Interfaces;
using MoviesChallenge.Domain.Models;

namespace MoviesChallenge.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllActors([FromQuery] PaginationParameters paginationParams)
    {
        var actors = await _actorService.GetAllAsync(paginationParams);
        return Ok(actors);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchActors([FromQuery] string param, [FromQuery] PaginationParameters paginationParams)
    {
        if (string.IsNullOrWhiteSpace(param))
            return BadRequest("Actor name is required for search.");

        var actors = await _actorService.SearchActorsAsync(param, paginationParams);
        return Ok(actors);
    }

    [HttpGet("{uniqueId}")]
    public async Task<IActionResult> GetActorByUniqueId(Guid uniqueId)
    {
        var actor = await _actorService.GetByUniqueIdAsync(uniqueId);
        return actor != null ? Ok(actor) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateActor([FromBody] ActorDto actorDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var createdActor = await _actorService.AddAsync(actorDto);
        return CreatedAtAction(nameof(GetActorByUniqueId), new { uniqueId = createdActor.Data.UniqueId }, createdActor);
    }

    [HttpPut("{uniqueId}")]
    public async Task<IActionResult> UpdateActor(Guid uniqueId, [FromBody] ActorDto actorDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await _actorService.UpdateAsync(uniqueId, actorDto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{uniqueId}")]
    public async Task<IActionResult> DeleteActor(Guid uniqueId)
    {
        var deleted = await _actorService.DeleteAsync(uniqueId);
        return deleted ? NoContent() : NotFound();
    }
}
