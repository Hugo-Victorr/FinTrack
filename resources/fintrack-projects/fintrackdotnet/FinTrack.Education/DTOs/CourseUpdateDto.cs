using FinTrack.Model.Enums;

namespace FinTrack.Education.DTOs;

public record CourseUpdateDto(
    string Title,
    string Description,
    string Aims,
    Guid CategoryId,
    bool IsPublished,
    CourseLevel Level,
    List<CourseModuleDto> Modules
);
