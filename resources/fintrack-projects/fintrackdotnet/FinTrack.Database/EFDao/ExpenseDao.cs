using FinTrack.Database.Contracts;
using FinTrack.Model.Entities;

namespace FinTrack.Database.EFDao
{
    public class ExpenseDao(FintrackDbContext context) : BaseDao<Expense>(context), IExpenseDao
    {
        protected override Task ValidateEntityForInsert(params Expense[] obj)
        {
            return Task.CompletedTask;
        }

        protected override Task ValidateEntityForUpdate(params Expense[] obj)
        {
            return Task.CompletedTask;
        }
    }
}
