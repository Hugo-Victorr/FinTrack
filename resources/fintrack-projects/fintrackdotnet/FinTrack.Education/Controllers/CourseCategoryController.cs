using FinTrack.Education.DTOs;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Education.Services;
using FinTrack.Database.EFDao;
using Microsoft.AspNetCore.Authorization;

namespace Fintrack.Education.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CourseCategoryController(CourseCategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryOptions opts)
    {
        var result = await service.GetAllAsync(opts);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await service.GetByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CourseCategoryCreateDto dto)
    {
        var result = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CourseCategoryUpdateDto dto)
    {
        var result = await service.UpdateAsync(id, dto);
        return result is null ? NotFound() : Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
