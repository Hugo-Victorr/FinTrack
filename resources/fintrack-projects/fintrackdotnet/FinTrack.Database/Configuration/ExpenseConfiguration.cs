using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration
{
    public class ExpenseConfiguration : BaseConfiguration<Expense>
    {
        public override void Configure(EntityTypeBuilder<Expense> builder)
        {
            base.Configure(builder);

            builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired();

            builder
            .Property(x => x.ExpenseCategoryId)
            .HasColumnName("expense_category_id")
            .IsRequired();

            builder
                .HasOne(x => x.ExpenseCategory)
                .WithMany()
                .HasForeignKey(x => x.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
            .Property(x => x.WalletId)
            .HasColumnName("wallet_id")
            .IsRequired();

            builder
                .HasOne(x => x.Wallet)
                .WithMany()
                .HasForeignKey(x => x.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
            .Property(x => x.Amount)
            .HasColumnName("amount")
            .IsRequired();

            builder
            .Property(x => x.ExpenseDate)
            .HasColumnName("expense_date")
            .HasDefaultValueSql("now()")
                   .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
            .IsRequired();

            builder
            .Property(x => x.OperationType)
            .HasColumnName("operation_type")
            .IsRequired();
        }
    }
}
