using FinTrack.Model.Enums;

namespace FinTrack.Education.DTOs;

public record CourseCreateDto(
    string Title,
    string CategoryId,
    string Description,
    bool IsPublished,
    string Aims,
    string Instructor,
    CourseLevel Level
);