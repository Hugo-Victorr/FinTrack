using System.Security.Claims;
using FinTrack.Education.DTOs;
using FinTrack.Education.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Education.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProgressController(ProgressService _progressService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdateLessonProgress([FromBody] LessonProgressDto dto)
    {
        Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);

        await _progressService.UpdateProgress(userId, dto);
        return Ok();
    }
}