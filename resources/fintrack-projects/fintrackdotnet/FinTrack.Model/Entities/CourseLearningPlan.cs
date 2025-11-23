namespace FinTrack.Model.Entities;

public class CourseLearningPlan : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    // Relationships
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public Guid UserId { get; set; }

    public double ProgressPercent { get; set; } = 0.0;
    public bool IsCompleted { get; set; } = false;
}