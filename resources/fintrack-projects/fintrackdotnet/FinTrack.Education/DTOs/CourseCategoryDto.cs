namespace FinTrack.Education.DTOs;

public record CourseCategoryDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt
);
