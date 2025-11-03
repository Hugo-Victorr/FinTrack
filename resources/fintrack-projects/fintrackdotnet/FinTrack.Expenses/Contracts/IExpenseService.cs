using FinTrack.Model.DTO;

namespace FinTrack.Expenses.Contracts
{
    public interface IExpenseService
    {
        Task<List<ExpenseDTO>> AllAsync();
        Task<ExpenseDTO?> GetByIdAsync(Guid id);
        Task<ExpenseDTO> CreateAsync(ExpenseDTO dto);
        Task<bool> UpdateAsync(Guid id, ExpenseDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}
