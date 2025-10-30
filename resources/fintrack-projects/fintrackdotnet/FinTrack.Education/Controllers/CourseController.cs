using FinTrack.Database.EFDao;
using FinTrack.Education.DTOs;
using FinTrack.Education.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Education.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController(CourseService _courseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryOptions opts)
    {
        var result = await _courseService.GetAllAsync(opts);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromHeader(Name = "x-include-headers")] bool includeLessons = false)
    {
        var result = await _courseService.GetByIdAsync(id, includeLessons);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
    {
        var result = await _courseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("/module")]
    public async Task<IActionResult> AddModule([FromBody] CourseModuleCreateDto dto)
    {
        if (await _courseService.AddModuleAsync(dto))
        return StatusCode(StatusCodes.Status201Created);

        // TODO: informar mensagem de erro
        return BadRequest();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CourseUpdateDto dto)
    {
        var result = await _courseService.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _courseService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
