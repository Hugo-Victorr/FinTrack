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
    public CourseContentDto(string title, int key)
    {
        Title = string.Format("{0}. {1}", key, title);
        Children = [];
        ModuleKey = key;
        Key = key.ToString();
    }

    // Lesson
    public CourseContentDto(string title, int key, int moduleKey, string videoUrl)
    {
        Title = string.Format("{0}.{1} - {2}", key, moduleKey, title);
        VideoUrl = videoUrl;
        Key = string.Format("{0}-{1}", moduleKey, key);
    }

    public void AddLessonToModule(string title, int key, string videoUrl)
    {
        this.Children!.Add(new CourseContentDto(title, key, this.ModuleKey, videoUrl));
    }
}