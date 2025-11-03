using FinTrack.Database.Contracts;
using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using FinTrack.Model.Entities;

namespace FinTrack.Expenses.Services
{
    public class WalletService(IWalletDao walletDao) : IWalletService
    {
        private readonly IWalletDao _dao = walletDao;

        public async Task<List<WalletDTO>> AllAsync()
        {
            try
            {
                List<Wallet> preferences = await _dao.AllAsync();

                return preferences.Select(x => new WalletDTO(x)).ToList();
            }
            catch (Exception ex)
            {
                string errorMsg = $"Erro ao consultar as preferências do portal";
                //_logger.LogError(ex, errorMsg);
                throw new Exception(errorMsg, ex);
            }
        }

        public async Task<WalletDTO?> GetByIdAsync(Guid id)
        {
            Wallet? entity = await _dao.FindAsync(id);
            return entity == null ? null : new WalletDTO(entity);
        }

        public async Task<WalletDTO> CreateAsync(WalletDTO dto)
        {
            Wallet entity = dto.ToWallet();
            await _dao.AddAsync(entity);
            return new WalletDTO(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, WalletDTO dto)
        {
            Wallet? existing = await _dao.FindAsync(id, track: true);
            if (existing == null) return false;
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Amount = dto.Amount;
            existing.Currency = dto.Currency;
            existing.WalletCategory = dto.WalletCategory;
            existing.User = dto.User;
            int updated = await _dao.UpdateAsync(existing);
            return updated > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            int deleted = await _dao.DeleteAsync(id);
            return deleted > 0;
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            Wallet? existing = await _dao.FindAsync(id, track: true);
            if (existing == null) return false;
            int restored = await _dao.RestoreAsync(existing);
            return restored > 0;
        }
    }
}
