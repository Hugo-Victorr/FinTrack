namespace FinTrack.Model.Entities
{
    public class ExpenseCategory : BaseEntity
    {
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; }

        public ExpenseCategory(string description, string color) : base()
        {
            Description = description;
            Color = color;
        }
        public ExpenseCategory(string description) : base()
        {
            Description = description;
            Color = "#FFFFFF"; // Default color white
        }
    }
}