using FinTrack.Database.EFDao;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public class CourseProgressRepository : BaseDao<CourseProgress>, ICourseProgressRepository
{
    public CourseProgressRepository(FintrackDbContext context) : base(context)
    {
    }

    public async Task<CourseProgress?> FindAsync(Guid userId, Guid courseId)
    {
        return await _context.FindAsync<CourseProgress>(userId, courseId);
    }

    protected override Task ValidateEntityForInsert(params CourseProgress[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseProgress[] obj)
    {
        return Task.CompletedTask;
    }
}
