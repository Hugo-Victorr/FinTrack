using FinTrack.Model.DTO;

namespace FinTrack.Expenses.Contracts
{
    public interface IExpenseCategoryService
    {
        Task<List<ExpenseCategoryDTO>> AllAsync();
        Task<ExpenseCategoryDTO?> GetByIdAsync(Guid id);
        Task<ExpenseCategoryDTO> CreateAsync(ExpenseCategoryDTO dto);
        Task<bool> UpdateAsync(Guid id, ExpenseCategoryDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}
