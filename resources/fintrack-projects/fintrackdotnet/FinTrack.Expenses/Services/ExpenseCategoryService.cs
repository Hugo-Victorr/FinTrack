using FinTrack.Database.Contracts;
using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using FinTrack.Model.Entities;

namespace FinTrack.Expenses.Services
{
    public class ExpenseCategoryService(IExpenseCategoryDao expenseCategoryDao) : IExpenseCategoryService
    {
        private readonly IExpenseCategoryDao _dao = expenseCategoryDao;

        public async Task<List<ExpenseCategoryDTO>> AllAsync()
        {
            List<ExpenseCategory> list = await _dao.AllAsync();
            return list.Select(x => new ExpenseCategoryDTO(x)).ToList();
        }

        public async Task<ExpenseCategoryDTO?> GetByIdAsync(Guid id)
        {
            ExpenseCategory? entity = await _dao.FindAsync(id);
            return entity == null ? null : new ExpenseCategoryDTO(entity);
        }

        public async Task<ExpenseCategoryDTO> CreateAsync(ExpenseCategoryDTO dto)
        {
            ExpenseCategory entity = dto.ToExpenseCategory();
            await _dao.AddAsync(entity);
            return new ExpenseCategoryDTO(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, ExpenseCategoryDTO dto)
        {
            ExpenseCategory? existing = await _dao.FindAsync(id, track: true);
            if (existing == null) return false;
            existing.Description = dto.Description;
            existing.Color = dto.Color;
            existing.User = dto.User;
            int updated = await _dao.UpdateAsync(existing);
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            int deleted = await _dao.DeleteAsync(id);
            return deleted > 0;
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            ExpenseCategory? existing = await _dao.FindAsync(id, track: true);
            if (existing == null) return false;
            int restored = await _dao.RestoreAsync(existing);
            return restored > 0;
        }
    }
}
