using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration;

public class LessonProgressConfiguration : BaseConfiguration<CourseLessonProgress>
{
    public override void Configure(EntityTypeBuilder<CourseLessonProgress> builder)
    {
        base.Configure(builder);

        builder.HasKey(cp => new { cp.LessonId, cp.User });

        builder.HasOne(cp => cp.Lesson)
               .WithMany()
               .HasForeignKey(cp => cp.LessonId);
    }
}