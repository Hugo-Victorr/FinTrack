namespace FinTrack.Education.DTOs;

public record CourseDto(
    Guid Id,
    string Title,
    string Description,
    CourseCategoryDto Category,
    bool IsPublished,
    DateTime CreatedAt,
    int LessonsLength
);
