namespace FinTrack.Education.DTOs;

public class CourseLessonDto
{
    public Guid LessonId { get; set; }
    public string Title { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public int Order { get; set; }
}