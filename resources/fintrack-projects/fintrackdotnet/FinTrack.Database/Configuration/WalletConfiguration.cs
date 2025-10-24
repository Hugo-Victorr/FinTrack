using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration
{
    public class WalletConfiguration : BaseConfiguration<Wallet>
    {
        public override void Configure(EntityTypeBuilder<Wallet> builder)
        {
            base.Configure(builder);

            builder
                .Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired();

            builder
                .Property(x => x.Amount)
                .HasColumnName("amount")
                .IsRequired()
                .HasDefaultValue(0);

            builder
                .Property(x => x.Currency)
                .HasColumnName("currency")
                .IsRequired()
                .HasDefaultValue(Model.Enums.CurrencyType.USD)
                .HasSentinel(0);

            builder
                .Property(x => x.WalletCategory)
                .HasColumnName("wallet_category")
                .IsRequired()
                .HasDefaultValue(Model.Enums.WalletType.Cash)
                .HasSentinel(0);
        }
    }
}
