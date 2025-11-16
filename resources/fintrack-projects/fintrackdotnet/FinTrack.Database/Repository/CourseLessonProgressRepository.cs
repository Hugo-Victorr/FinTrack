using FinTrack.Database.EFDao;
using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Database.Interfaces;

public class CourseLessonProgressRepository : BaseDao<CourseLessonProgress>, ICourseLessonProgressRepository
{
    public CourseLessonProgressRepository(FintrackDbContext context) : base(context)
    {
    }

    public async Task<CourseLessonProgress?> FindAsync(Guid userId, Guid lessonId)
    {
        return await _context.FindAsync<CourseLessonProgress>(userId, lessonId);
    }

    public async Task<List<CourseLessonProgress>?> GetCourseProgressionAsync(Guid userId, Guid courseId)
    {
        return await _context.CourseLessonProgresses
            .AsNoTracking()
            .Where(clp => clp.User == userId &&
                        clp.Lesson.Module.CourseId == courseId)
            .Include(clp => clp.Lesson)
                .ThenInclude(l => l.Module)
            .ToListAsync();
    }

    protected override Task ValidateEntityForInsert(params CourseLessonProgress[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseLessonProgress[] obj)
    {
        return Task.CompletedTask;
    }
}
