namespace FinTrack.Model.Entities;

public class CourseLessonProgress : BaseEntity
{
    public Guid LessonId { get; set; }
    public CourseLesson Lesson { get; set; } = null!;

    public int TimeWatchedSeconds { get; set; }
    public double PercentCompleted { get; set; } // 0–100
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
}
