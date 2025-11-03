using FinTrack.Model.DTO;

namespace FinTrack.Model.Entities
{
    public class ExpenseCategory : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#FFFFFF"; // Default color white

        public ExpenseCategory(string description, string color) : base()
        {
            Description = description;
            Color = color;
        }
        public ExpenseCategory(string description) : base()
        {
            Description = description;
        }

        public ExpenseCategory() : base() { }

        public ExpenseCategory(ExpenseCategoryDTO dto) : base()
        {
            Id = dto.Id;
            Description = dto.Description;
            Color = dto.Color;
            CreatedAt = dto.CreatedAt;
            UpdatedAt = dto.UpdatedAt;
            User = dto.User;
        }
    }
}