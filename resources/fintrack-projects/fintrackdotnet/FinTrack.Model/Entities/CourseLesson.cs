namespace FinTrack.Model.Entities;

public class CourseLesson : BaseEntity
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
    public TimeSpan Duration { get; set; }

    public Guid ModuleId { get; set; }
    public CourseModule Module { get; set; } = null!;
}
