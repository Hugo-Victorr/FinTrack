using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public interface ICourseProgressRepository : IRepository<CourseProgress>
{
    Task<CourseProgress?> FindAsync(Guid userId, Guid courseId);
}