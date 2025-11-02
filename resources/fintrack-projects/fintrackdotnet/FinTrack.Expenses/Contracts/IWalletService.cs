using FinTrack.Model.DTO;

namespace FinTrack.Expenses.Contracts
{
    public interface IWalletService
    {
        Task<List<WalletDTO>> AllAsync();
    }
}
