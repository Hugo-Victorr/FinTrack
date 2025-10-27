using FinTrack.Database.Configuration;
using FinTrack.Model.Entities;
using FinTrack.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FinTrack.Database
{
    public class FintrackDbContext : DbContext
    {
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; } = null!;
        public DbSet<Wallet> Wallets { get; set; } = null!;

        static FintrackDbContext() { }

        public FintrackDbContext(DbContextOptions<FintrackDbContext> options) : base(options)
        {
#pragma warning disable CS0618 // Obsoleto, mas na versao 7.0 ainda nao tem como configurar junto com o EF nas APIs
            _ = NpgsqlConnection.GlobalTypeMapper.MapEnum<CurrencyType>("currency_type");
            _ = NpgsqlConnection.GlobalTypeMapper.MapEnum<WalletType>("wallet_type");
#pragma warning restore CS0618 // Obsoleto, mas na versao 7.0 ainda nao tem como configurar junto com o EF nas APIs
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.HasPostgresEnum<CurrencyType>(name: "currency_type");
            _ = modelBuilder.HasPostgresEnum<WalletType>(name: "wallet_type");

            _ = modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
            _ = modelBuilder.ApplyConfiguration(new ExpenseCategoryConfiguration());
            _ = modelBuilder.ApplyConfiguration(new WalletConfiguration());

            // _ = modelBuilder.Ignore<BaseEntity>();
        }
    }
}
