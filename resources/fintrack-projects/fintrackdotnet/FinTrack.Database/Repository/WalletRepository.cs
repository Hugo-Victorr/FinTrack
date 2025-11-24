using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public class WalletRepository : BaseDao<Wallet>, IWalletRepository
{
    public WalletRepository(FintrackDbContext context) : base(context)
    {
    }

    protected override Task ValidateEntityForInsert(params Wallet[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params Wallet[] obj)
    {
        return Task.CompletedTask;
    }
}





