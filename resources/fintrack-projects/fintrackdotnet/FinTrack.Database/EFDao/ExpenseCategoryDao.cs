using FinTrack.Database.Contracts;
using FinTrack.Model.Entities;

namespace FinTrack.Database.EFDao
{
    public class ExpenseCategoryDao(FintrackDbContext context) : BaseDao<ExpenseCategory>(context), IExpenseCategoryDao
    {
        protected override Task ValidateEntityForInsert(params ExpenseCategory[] obj)
        {
            return Task.CompletedTask;
        }
        protected override Task ValidateEntityForUpdate(params ExpenseCategory[] obj)
        {
            return Task.CompletedTask;
        }
    }
}
