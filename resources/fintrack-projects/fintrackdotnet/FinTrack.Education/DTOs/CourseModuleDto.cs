namespace FinTrack.Education.DTOs;

public class CourseModuleDto
{
    public Guid ModuleId { get; set; }
    public string Name { get; set; } = null!;
    public int OrderIndex { get; set; }
    public List<CourseLessonDto> Lessons { get; set; } = null!;
}