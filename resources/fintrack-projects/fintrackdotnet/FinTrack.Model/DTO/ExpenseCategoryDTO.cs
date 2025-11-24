using FinTrack.Model.Entities;

namespace FinTrack.Model.DTO
{
    public class ExpenseCategoryDTO
    {
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime? UpdatedAt { get; set; }

        public ExpenseCategoryDTO() { }

        public ExpenseCategoryDTO(ExpenseCategory category)
        {
            Id = category.Id;
            Description = category.Description;
            Color = category.Color;
            CreatedAt = category.CreatedAt;
            UpdatedAt = category.UpdatedAt;
        }

        public ExpenseCategory ToExpenseCategory() => new(Description, Color)
        {
            Id = Id,
            Description = Description,
            Color = Color,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };
    }
}
