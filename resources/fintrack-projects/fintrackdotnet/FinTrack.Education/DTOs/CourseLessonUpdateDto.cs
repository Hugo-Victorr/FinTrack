namespace FinTrack.Education.DTOs;

public class CourseLessonUpdateDto
{
    public string Title { get; set; } = null!;
    public string? VideoUrl { get; set; }
    public int Order { get; set; }
}