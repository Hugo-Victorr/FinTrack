namespace FinTrack.Model.Entities;

public class CourseLesson : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public TimeSpan Duration { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
