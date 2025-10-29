namespace FinTrack.Model.Entities;

public class CourseModule : BaseEntity
{
    public string Name { get; set; } = null!;
    public int OrderIndex { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public ICollection<CourseLesson> Lessons { get; set; } = [];
}