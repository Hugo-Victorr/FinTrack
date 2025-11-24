using FinTrack.Education.DTOs;

namespace FinTrack.Education.Contracts;

public interface IProgressService
{
    Task UpdateProgress(Guid userId, LessonProgressDto dto);
    Task<CourseProgressDto?> GetCourseProgress(Guid userId, Guid courseId);
    Task<List<LessonProgressDto>?> GetLessonsProgress(Guid userId, Guid courseId);
}

