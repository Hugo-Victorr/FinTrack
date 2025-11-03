using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Expenses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController(IExpenseService expenseService) : ControllerBase
    {
        private readonly IExpenseService _expenseService = expenseService;

        [HttpGet("health")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Health() => Ok("Expense Service is running...");

        [HttpGet]
        [ProducesResponseType(typeof(List<ExpenseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExpensesAsync()
        {
            List<ExpenseDTO> expenses = await _expenseService.AllAsync();
            return Ok(expenses);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ExpenseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            ExpenseDTO? expense = await _expenseService.GetByIdAsync(id);
            return expense == null ? NotFound() : Ok(expense);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ExpenseDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] ExpenseDTO dto)
        {
            ExpenseDTO created = await _expenseService.CreateAsync(dto);
            return Ok(created);
            //return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ExpenseDTO dto)
        {
            bool ok = await _expenseService.UpdateAsync(id, dto);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            bool ok = await _expenseService.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/restore")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RestoreAsync(Guid id)
        {
            bool ok = await _expenseService.RestoreAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
