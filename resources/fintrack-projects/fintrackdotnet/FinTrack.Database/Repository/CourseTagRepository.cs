using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public class CourseTagRepository : BaseDao<CourseTag>, ICourseTagRepository
{
    public CourseTagRepository(FintrackDbContext context) : base(context)
    {
    }

    protected override Task ValidateEntityForInsert(params CourseTag[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseTag[] obj)
    {
        return Task.CompletedTask;
    }
}