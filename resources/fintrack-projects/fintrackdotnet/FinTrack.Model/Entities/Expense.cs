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
        public OperationTypeEnum OperationType { get; set; } = OperationTypeEnum.Expense;
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
            ExpenseCategory = expense.ExpenseCategory;
            Id = expense.Id;
            CreatedAt = expense.CreatedAt;
            UpdatedAt = expense.UpdatedAt;
            User = expense.User;
        }
    }
}