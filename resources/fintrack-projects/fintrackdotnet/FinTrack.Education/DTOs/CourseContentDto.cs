using System.Text.Json.Serialization;

namespace FinTrack.Education.DTOs;

public class CourseContentDto
{
    public string Title { get; set; } = null!;
    public string Id { get; set; }
    public int Order { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? VideoUrl { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CourseContentDto>? Lessons { get; set; }

    public CourseContentDto()
    {
    }    

    // Module
    public CourseContentDto(string title, int moduleIdx, Guid id)
    {
        Title = title;
        Order = moduleIdx;
        Lessons = [];
        Id = id.ToString();
    }

    // Lesson
    public CourseContentDto(string title, int lessonIdx, string videoUrl, Guid id)
    {
        Title = title;
        Order = lessonIdx;
        VideoUrl = videoUrl;
        Id = id.ToString();
    }

    public void AddLessonToModule(string title, int idx, string videoUrl, Guid id)
    {
        Lessons!.Add(new CourseContentDto(title, idx, videoUrl, id));
    }
}