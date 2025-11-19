namespace OpenFinance.Model.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToLocalTime();
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
