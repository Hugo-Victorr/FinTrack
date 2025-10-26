namespace FinTrack.Model.Entities;

public class CourseTag : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<CourseTagAssignment> CourseTags { get; set; } = new List<CourseTagAssignment>();
}
