using FinTrack.Database.EFDao;
using FinTrack.Expenses.DTOs;
using FinTrack.Expenses.Services;
using FinTrack.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace FinTrack.Expenses.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WalletController(WalletService _service) : ControllerBase
{
    [HttpGet("/accounts/{cpf:string}")]
    public async Task<IActionResult> GetAccountsByCPF(string cpf)
    {
        try
        {
            using var client = new HttpClient(); // Ideal: usar IHttpClientFactory
            var url = $"http://openfinance-api:8080/api/account/cpf/{cpf}";
            var response = await client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, json);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<OpenFinanceCustomerAccountsDto>(json, options);

            if (data is null)
                return BadRequest("Formato inesperado");

            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, inner = ex.InnerException?.Message });
        }
    }

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
    public async Task<IActionResult> Create([FromBody] WalletCreateDto dto)
    {
        _ = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value, out Guid userId);
        var result = await _service.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] WalletUpdateDto dto)
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
