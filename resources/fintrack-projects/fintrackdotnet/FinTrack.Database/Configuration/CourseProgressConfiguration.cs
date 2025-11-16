
using FinTrack.Database.Configuration;
using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration;

public class CourseProgressConfiguration : BaseConfiguration<CourseProgress>
{
    public override void Configure(EntityTypeBuilder<CourseProgress> builder)
    {
        base.Configure(builder);

        builder.HasKey(cp => new { cp.CourseId, cp.User });

        builder.HasOne(cp => cp.Course)
               .WithMany()
               .HasForeignKey(cp => cp.CourseId);
    }
}