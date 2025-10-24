using FinTrack.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.RegularExpressions;

namespace FinTrack.Database.Configuration
{
    public class BaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            _ = builder.HasKey(x => x.Id);
            _ = builder.HasIndex(x => x.CreatedAt);
            _ = builder.HasIndex(x => x.UpdatedAt);
            _ = builder.HasIndex(x => x.DeletedAt);

            _ = builder.ToTable(Regex.Replace(GetType().Name.ToString().Replace("Configuration", ""), "([a-z])([A-Z])", "$1_$2").ToLower());

            _ = builder.Property(p => p.Id)
                .HasColumnType("uuid")
                .HasColumnName("id")
                .IsRequired();

            _ = builder.Property(p => p.CreatedAt)
                   .HasColumnName("created_at")
                   .HasDefaultValueSql("now()")
                   .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                   .IsRequired();

            _ = builder.Property(p => p.UpdatedAt)
                   .IsRequired(false)
                   .HasColumnName("updated_at")
                   .HasConversion(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            _ = builder.Property(p => p.DeletedAt)
                   .IsRequired(false)
                   .HasColumnName("deleted_at");

            _ = builder.Property(p => p.User)
                .HasColumnType("uuid")
                .HasColumnName("user_id")
                .IsRequired();

            _ = builder.HasIndex(p => p.User);
        }
    }
}
