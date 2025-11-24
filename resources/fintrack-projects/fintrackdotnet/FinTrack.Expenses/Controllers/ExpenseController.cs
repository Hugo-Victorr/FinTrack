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
public class ExpenseController(ExpenseService _expenseService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryOptions opts)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _expenseService.GetAllAsync(opts, userId);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _expenseService.GetByIdAsync(id, userId);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseCreateDto dto)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _expenseService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ExpenseUpdateDto dto)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _expenseService.UpdateAsync(id, dto, userId);
        return result is null ? NotFound() : Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var success = await _expenseService.DeleteAsync(id, userId);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/restore")]
    public async Task<IActionResult> Restore(Guid id)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var success = await _expenseService.RestoreAsync(id, userId);
        return success ? NoContent() : NotFound();
    }
}
