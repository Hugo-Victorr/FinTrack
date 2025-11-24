using FinTrack.Model.Entities;

namespace FinTrack.Model.DTO
{
    public class ExpenseDTO
    {
        public string Description { get; set; } = string.Empty;
        public Guid ExpenseCategoryId { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid WalletId { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime? UpdatedAt { get; set; }
        public Guid User { get; set; }

        public ExpenseDTO() { }

        public ExpenseDTO(Expense expense)
        {
            Id = expense.Id;
            Description = expense.Description;
            ExpenseCategoryId = expense.ExpenseCategoryId;
            Amount = expense.Amount;
            ExpenseDate = expense.ExpenseDate;
            WalletId = expense.WalletId;
            CreatedAt = expense.CreatedAt;
            UpdatedAt = expense.UpdatedAt;
            User = expense.User;
        }

        public Expense ToExpense() => new()
        {
            Id = Id,
            Description = Description,
            ExpenseCategoryId = ExpenseCategoryId,
            Amount = Amount,
            ExpenseDate = ExpenseDate,
            WalletId = WalletId,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            User = User
        };

    }
}
