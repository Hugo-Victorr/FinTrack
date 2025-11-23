using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public interface ICourseLessonProgressRepository : IRepository<CourseLessonProgress>
{
    Task<CourseLessonProgress?> FindAsync(Guid userId, Guid lessonId);
    Task<List<CourseLessonProgress>?> GetCourseProgressionAsync(Guid userId, Guid courseId);
}