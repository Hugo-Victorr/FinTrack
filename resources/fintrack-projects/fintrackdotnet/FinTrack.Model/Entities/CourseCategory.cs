namespace FinTrack.Model.Entities;

public class CourseCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
