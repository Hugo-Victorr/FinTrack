using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public class ExpenseRepository : BaseDao<Expense>, IExpenseRepository
{
    public ExpenseRepository(FintrackDbContext context) : base(context)
    {
    }

    protected override Task ValidateEntityForInsert(params Expense[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params Expense[] obj)
    {
        return Task.CompletedTask;
    }
}





