namespace FinTrack.Model.Entities;

public class CourseTagAssignment : BaseEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public Guid TagId { get; set; }
    public CourseTag Tag { get; set; } = null!;
}