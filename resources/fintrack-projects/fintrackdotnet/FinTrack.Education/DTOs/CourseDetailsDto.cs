using FinTrack.Model.Enums;

namespace FinTrack.Education.DTOs;

public class CourseDetailsDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
    public string? Aims { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string Instructor { get; set; } = null!;
    public CourseCategoryDto Category { get; set; } = null!;
    public CourseLevel Level { get; set; }
    
    public int DurationMinutes { get; set; }
    public int LessonsLength { get; set; }

    public List<CourseContentDto>? Modules { get; set; }
}
