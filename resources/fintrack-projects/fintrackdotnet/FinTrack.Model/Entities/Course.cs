namespace FinTrack.Model.Entities;

public class Course : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public int DurationMinutes { get; set; }
    public bool IsPublished { get; set; } = false;

    public Guid CategoryId { get; set; }
    public CourseCategory Category { get; set; } = null!;

    public ICollection<CourseLesson> Lessons { get; set; } = new List<CourseLesson>();
    public ICollection<CourseLearningPlan> LearningPlans { get; set; } = new List<CourseLearningPlan>();
    public ICollection<CourseTagAssignment> Tags { get; set; } = new List<CourseTagAssignment>();
}
