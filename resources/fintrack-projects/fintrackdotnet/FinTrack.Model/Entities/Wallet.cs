using FinTrack.Model.Enums;

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
    }
}