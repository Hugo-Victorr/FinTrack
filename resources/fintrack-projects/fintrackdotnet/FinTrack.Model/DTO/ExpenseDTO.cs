using FinTrack.Model.Entities;

namespace FinTrack.Model.DTO
{
    public class ExpenseDTO
    {
        public string Description { get; set; } = string.Empty;
        public Guid ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
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
            ExpenseCategory = expense.ExpenseCategory;
            Amount = expense.Amount;
            ExpenseDate = expense.ExpenseDate;
            CreatedAt = expense.CreatedAt;
            UpdatedAt = expense.UpdatedAt;
            User = expense.User;
        }

        public Expense ToExpense() => new()
        {
            Id = Id,
            Description = Description,
            ExpenseCategoryId = ExpenseCategoryId,
            ExpenseCategory = ExpenseCategory,
            Amount = Amount,
            ExpenseDate = ExpenseDate,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            User = User
        };

    }
}
