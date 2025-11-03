using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Contracts
{
    public interface IExpenseDao : IRepository<Expense>
    {
    }
}
