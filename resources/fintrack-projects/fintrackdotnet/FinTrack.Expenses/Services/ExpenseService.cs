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
    }
}
