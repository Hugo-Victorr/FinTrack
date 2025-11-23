using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTrack.Database.Configuration
{
    public class CourseModuleConfiguration : BaseConfiguration<CourseModule>
    {
        public override void Configure(EntityTypeBuilder<CourseModule> builder)
        {
            base.Configure(builder);
            
            builder.HasOne(m => m.Course)
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
