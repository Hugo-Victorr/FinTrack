using FinTrack.Model.DTO;

namespace FinTrack.Expenses.Contracts
{
    public interface IWalletService
    {
        Task<List<WalletDTO>> AllAsync();
        Task<WalletDTO?> GetByIdAsync(Guid id);
        Task<WalletDTO> CreateAsync(WalletDTO dto);
        Task<bool> UpdateAsync(Guid id, WalletDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> RestoreAsync(Guid id);
    }
}
