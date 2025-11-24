using System.Security.Claims;
using FinTrack.Database.EFDao;
using FinTrack.Expenses.DTOs;
using FinTrack.Expenses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Expenses.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseCategoryController(ExpenseCategoryService _service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryOptions opts)
    {
        Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _service.GetAllAsync(opts, userId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _service.GetByIdAsync(id, userId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseCategoryCreateDto dto)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ExpenseCategoryUpdateDto dto)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _service.UpdateAsync(id, dto, userId);
        return result is null ? NotFound() : Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var success = await _service.DeleteAsync(id, userId);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var success = await _service.RestoreAsync(id, userId);
        return success ? NoContent() : NotFound();
    }
}
