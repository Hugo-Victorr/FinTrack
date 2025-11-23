using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration
{
    public class CourseLessonConfiguration : BaseConfiguration<CourseLesson>
    {
        public override void Configure(EntityTypeBuilder<CourseLesson> builder)
        {
            base.Configure(builder);

            builder.HasOne(m => m.Module)
                .WithMany(c => c.Lessons)
                .HasForeignKey(m => m.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
