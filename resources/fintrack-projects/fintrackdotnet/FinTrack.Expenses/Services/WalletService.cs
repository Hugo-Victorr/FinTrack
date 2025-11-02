using FinTrack.Database.Contracts;
using FinTrack.Expenses.Contracts;
using FinTrack.Model.DTO;
using FinTrack.Model.Entities;

namespace FinTrack.Expenses.Services
{
    public class WalletService(IWalletDao walletDao) : IWalletService
    {
        private readonly IWalletDao _walletDao = walletDao;

        public async Task<List<WalletDTO>> AllAsync()
        {
            try
            {
                List<Wallet> preferences = await _walletDao.AllAsync();

                return preferences.Select(x => new WalletDTO(x)).ToList();
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
