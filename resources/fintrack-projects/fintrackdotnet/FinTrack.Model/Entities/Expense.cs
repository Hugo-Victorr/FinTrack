namespace FinTrack.Model.Entities
{
    public class Expense : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public Guid ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenseDate { get; set; }

        public Expense(double amount, string description, DateTime expenseDate) : base()
        {
            Amount = amount;
            Description = description;
            ExpenseDate = expenseDate;
        }
    }
}