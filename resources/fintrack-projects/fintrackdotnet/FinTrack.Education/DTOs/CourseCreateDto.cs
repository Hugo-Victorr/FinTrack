namespace FinTrack.Education.DTOs;

public record CourseCreateDto(
    string Title,
    string Description,
    string CategoryId,
    bool IsPublished
);