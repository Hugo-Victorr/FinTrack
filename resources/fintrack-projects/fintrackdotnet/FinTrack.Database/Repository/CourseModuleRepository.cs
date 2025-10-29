using FinTrack.Database;
using FinTrack.Database.EFDao;
using FinTrack.Database.Interfaces;
using FinTrack.Model.Entities;

public class CourseModuleRepository : BaseDao<CourseModule>, ICourseModuleRepository
{
    public CourseModuleRepository(FintrackDbContext context) : base(context)
    {
    }

    protected override Task ValidateEntityForInsert(params CourseModule[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseModule[] obj)
    {
        return Task.CompletedTask;
    }
}