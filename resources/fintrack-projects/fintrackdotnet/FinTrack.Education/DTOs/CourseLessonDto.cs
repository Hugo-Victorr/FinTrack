namespace FinTrack.Education.DTOs;

public class CourseLessonDto
{
    public string Title { get; set; } = null!;
    public string VideoUrl { get; set; } = null!;
    public int Order { get; set; }
}