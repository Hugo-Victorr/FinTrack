using FinTrack.Model.Enums;

namespace FinTrack.Education.DTOs;

public record CourseUpdateDto(
    string Title,
    string Description,
    string ThumbnailUrl,
    string Aims,
    Guid CategoryId,
    bool IsPublished,
    CourseLevel Level,
    List<CourseContentDto> Modules
);
