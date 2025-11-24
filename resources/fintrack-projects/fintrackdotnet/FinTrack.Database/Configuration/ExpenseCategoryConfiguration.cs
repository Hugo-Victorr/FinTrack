using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration
{
    public class ExpenseCategoryConfiguration : BaseConfiguration<ExpenseCategory>
    {
        public override void Configure(EntityTypeBuilder<ExpenseCategory> builder)
        {
            base.Configure(builder);

            builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired();

            builder
            .Property(x => x.Color)
            .HasColumnName("color")
            .IsRequired();

            builder
            .Property(x => x.OperationType)
            .HasColumnName("operation_type")
            .IsRequired();
        }
    }
}
