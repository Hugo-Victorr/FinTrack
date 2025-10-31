using System.Text.Json.Serialization;

namespace FinTrack.Education.DTOs;

public class CourseContentDto
{
    public string Title { get; set; } = null!;
    public string Key { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? VideoUrl { get; set; } = null!;

    private int ModuleKey { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<CourseContentDto>? Children { get; }

    // Module
    public CourseContentDto(string title, int moduleIdx, Guid id)
    {
        Title = string.Format("{0}. {1}", moduleIdx, title);
        ModuleKey = moduleIdx;
        Children = [];
        Key = id.ToString();
    }

    // Lesson
    public CourseContentDto(string title, int lessonIdx, int moduleIdx, string videoUrl, Guid id)
    {
        Title = string.Format("{0}.{1} - {2}", lessonIdx, moduleIdx, title);
        VideoUrl = videoUrl;
        Key = id.ToString();
    }

    public void AddLessonToModule(string title, int idx, string videoUrl, Guid id)
    {
        this.Children!.Add(new CourseContentDto(title, idx, this.ModuleKey, videoUrl, id));
    }
}