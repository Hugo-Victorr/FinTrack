using FinTrack.Model.Enums;
using FinTrack.Model.DTO;

namespace FinTrack.Model.Entities
{
    public class Wallet : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Amount { get; set; } = 0;
        public CurrencyType Currency { get; set; } = CurrencyType.USD;
        public WalletType WalletCategory { get; set; } = WalletType.Cash;


        //public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public void AddAmount(double amount)
        {
            if (amount < 1)
            {
                throw new ArgumentException("Amount to add must be positive.");
            }
            Amount += amount;
        }

        public void SubtractAmount(double amount)
        {
            if (Amount - amount < 0)
            {
                throw new InvalidOperationException("Insufficient funds in wallet.");
            }
            Amount -= amount;
        }

        public double GetAmount()
        {
            return Amount;
        }

        public Wallet() : base() { }

        public Wallet(WalletDTO dto) : base()
        {
            Id = dto.Id;
            Name = dto.Name;
            Description = dto.Description;
            Amount = dto.Amount;
            Currency = dto.Currency;
            WalletCategory = dto.WalletCategory;
            CreatedAt = dto.CreatedAt;
            UpdatedAt = dto.UpdatedAt;
            User = dto.User;
        }
    }
}