namespace FinTrack.Education.DTOs;

public class LessonProgressDto
{
    public Guid LessonId { get; set; }
    public int CurrentTimestamp { get; set; }
    public int ProgressPercentage { get; set; }
}
