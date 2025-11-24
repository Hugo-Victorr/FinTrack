using FinTrack.Model.DTO;
using FinTrack.Model.Enums;

namespace FinTrack.Model.Entities
{
    public class Expense : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public Guid ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public Expense() : base() { }

        public Expense(double amount, string description, DateTime expenseDate) : base()
        {
            Amount = amount;
            Description = description;
            ExpenseDate = expenseDate;
        }

        public Expense(ExpenseDTO expense) : base()
        {
            Amount = expense.Amount;
            Description = expense.Description;
            ExpenseDate = expense.ExpenseDate;
            ExpenseCategoryId = expense.ExpenseCategoryId;
            WalletId = expense.WalletId;
            Id = expense.Id;
            CreatedAt = expense.CreatedAt;
            UpdatedAt = expense.UpdatedAt;
            User = expense.User;
        }
    }
}