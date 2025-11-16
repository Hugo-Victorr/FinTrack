using FinTrack.Database.EFDao;
using FinTrack.Education.DTOs;
using FinTrack.Education.Services;
using FinTrack.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Education.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseController(CourseService _courseService, IStorageService _storage) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryOptions opts)
    {
        var result = await _courseService.GetAllAsync(opts);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, [FromHeader(Name = "x-include-lessons")] bool includeLessons = false)
    {
        var result = await _courseService.GetByIdAsync(id, includeLessons);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "manager,admin")]
    public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
    {
        var result = await _courseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "manager,admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CourseUpdateDto dto)
    {
        var result = await _courseService.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "manager,admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _courseService.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("upload")]
    [Authorize(Roles = "manager,admin")]
    public async Task<IActionResult> GetUploadUrl([FromBody] UploadRequest req)
    {
        var url = await _storage.GenerateUploadUrl(req.Key, TimeSpan.FromMinutes(10));
        return Ok(new { url });
    }

    [HttpGet("play")]
    public IActionResult GetPlaybackUrl([FromQuery] string key)
    {
        var url = _storage.GenerateDownloadUrl(key, TimeSpan.FromHours(1));
        return Ok(new { url });
    }
}
