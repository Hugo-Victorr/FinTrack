using FinTrack.Database.Contracts;
using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using FinTrack.Model.Entities;

namespace FinTrack.Expenses.Services
{
    public class ExpenseCategoryService(IExpenseCategoryDao expenseCategoryDao) : IExpenseCategoryService
    {
        private readonly IExpenseCategoryDao _expenseCategoryDao = expenseCategoryDao;

        public async Task<List<ExpenseCategoryDTO>> AllAsync()
        {
            try
            {
                List<ExpenseCategory> preferences = await _expenseCategoryDao.AllAsync();

                return preferences.Select(x => new ExpenseCategoryDTO(x)).ToList();
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
