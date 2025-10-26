namespace FinTrack.Education.DTOs;

public record CourseUpdateDto(
    string Title,
    string Description,
    Guid CategoryId,
    bool IsPublished
);
