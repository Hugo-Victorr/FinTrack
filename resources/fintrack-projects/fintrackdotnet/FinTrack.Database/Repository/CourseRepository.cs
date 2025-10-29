
using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Database.Interfaces;

public class CourseRepository : BaseDao<Course>, ICourseRepository
{
    public CourseRepository(FintrackDbContext context) : base(context)
    {
    }

    public async Task<Course?> GetCourseComplete(Guid Id)
    {
        var course = await _context.Courses
            .Include(c => c.Category)
            .Include(c => c.Modules)
                .ThenInclude(m => m.Lessons)
            .FirstOrDefaultAsync(c => c.Id == Id);

        return course;
    }

    protected override Task ValidateEntityForInsert(params Course[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params Course[] obj)
    {
        return Task.CompletedTask;
    }
}
