using FinTrack.Model.DTO;

namespace FinTrack.Expenses.Contracts
{
    public interface IExpenseCategoryService
    {
        Task<List<ExpenseCategoryDTO>> AllAsync();
    }
}
