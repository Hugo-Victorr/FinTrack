using FinTrack.Model.Enums;

namespace FinTrack.Model.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Aims { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Instructor { get; set; }
    public CourseLevel Level { get; set; }

    public int DurationMinutes { get; set; }
    public bool IsPublished { get; set; } = false;
    public int LessonsLength { get; set; }

    public Guid CategoryId { get; set; }
    public CourseCategory Category { get; set; } = null!;

    public ICollection<CourseModule> Modules { get; set; } = [];
    public ICollection<CourseLearningPlan> LearningPlans { get; set; } = [];
    public ICollection<CourseTagAssignment> Tags { get; set; } = [];
}
