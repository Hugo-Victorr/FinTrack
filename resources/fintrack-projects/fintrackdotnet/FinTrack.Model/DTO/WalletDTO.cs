using FinTrack.Model.Entities;
using FinTrack.Model.Enums;

namespace FinTrack.Model.DTO
{
    public class WalletDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Amount { get; set; }
        public CurrencyType Currency { get; set; } = CurrencyType.USD;
        public WalletType WalletCategory { get; set; } = WalletType.Cash;
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime? UpdatedAt { get; set; }
        public Guid User { get; set; }

        public WalletDTO() { }

        public WalletDTO(Wallet wallet)
        {
            Id = wallet.Id;
            Name = wallet.Name;
            Description = wallet.Description;
            Amount = wallet.Amount;
            Currency = wallet.Currency;
            WalletCategory = wallet.WalletCategory;
            CreatedAt = wallet.CreatedAt;
            UpdatedAt = wallet.UpdatedAt;
            User = wallet.User;
        }

        public Wallet ToWallet() => new()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Amount = Amount,
            Currency = Currency,
            WalletCategory = WalletCategory,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            User = User
        };
    }
}
