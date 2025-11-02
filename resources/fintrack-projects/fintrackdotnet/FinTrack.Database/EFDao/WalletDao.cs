using FinTrack.Database.Contracts;
using FinTrack.Model.Entities;

namespace FinTrack.Database.EFDao
{
    public class WalletDao(FintrackDbContext context) : BaseDao<Wallet>(context), IWalletDao
    {
        protected override Task ValidateEntityForInsert(params Wallet[] obj)
        {
            return Task.CompletedTask;
        }

        protected override Task ValidateEntityForUpdate(params Wallet[] obj)
        {
            return Task.CompletedTask;
        }
    }
}
