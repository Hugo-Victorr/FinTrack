using FinTrack.Database.Contracts;
using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using FinTrack.Model.Entities;

namespace FinTrack.Expenses.Services
{
    public class ExpenseService(IExpenseDao expenseDao) : IExpenseService
    {
        private readonly IExpenseDao _expenseDao = expenseDao;

        public async Task<List<ExpenseDTO>> AllAsync()
        {
            try
            {
                List<Expense> preferences = await _expenseDao.AllAsync();

                return preferences.Select(x => new ExpenseDTO(x)).ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Erro ao consultar as preferências do portal";
                //_logger.LogError(ex, errorMsg);
                throw new Exception(errorMsg, ex);
            }
        }

        public async Task<ExpenseDTO?> GetByIdAsync(Guid id)
        {
            Expense? entity = await _expenseDao.FindAsync(id);
            return entity == null ? null : new ExpenseDTO(entity);
        }

        public async Task<ExpenseDTO> CreateAsync(ExpenseDTO dto)
        {
            Expense entity = dto.ToExpense();
            await _expenseDao.AddAsync(entity);
            return new ExpenseDTO(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, ExpenseDTO dto)
        {
            Expense? existing = await _expenseDao.FindAsync(id, track: true);
            if (existing == null) return false;

            existing.Description = dto.Description;
            existing.ExpenseCategoryId = dto.ExpenseCategoryId;
            existing.Amount = dto.Amount;
            existing.ExpenseDate = dto.ExpenseDate;
            existing.User = dto.User;
            int updated = await _expenseDao.UpdateAsync(existing);
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            int deleted = await _expenseDao.DeleteAsync(id);
            return deleted > 0;
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            Expense? existing = await _expenseDao.FindAsync(id, track: true);
            if (existing == null) return false;
            int restored = await _expenseDao.RestoreAsync(existing);
            return restored > 0;
        }
    }
}
