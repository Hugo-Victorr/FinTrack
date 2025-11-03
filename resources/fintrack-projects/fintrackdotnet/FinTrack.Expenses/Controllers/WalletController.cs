using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Expenses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController(IWalletService service) : ControllerBase
    {
        private readonly IWalletService _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(List<WalletDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await _service.AllAsync();
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WalletDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var item = await _service.GetByIdAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(WalletDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] WalletDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] WalletDTO dto)
        {
            var ok = await _service.UpdateAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreAsync(Guid id)
        {
            var ok = await _service.RestoreAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
