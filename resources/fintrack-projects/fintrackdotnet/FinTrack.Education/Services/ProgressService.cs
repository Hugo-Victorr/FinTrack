using AutoMapper;
using FinTrack.Database;
using FinTrack.Database.Interfaces;
using FinTrack.Education.DTOs;
using FinTrack.Model.Entities;

namespace FinTrack.Education.Services;

public class ProgressService
{
    private readonly FintrackDbContext _context;
    private readonly ICourseProgressRepository _courseProgressRepository;
    private readonly ICourseLessonProgressRepository _courseLessonProgressRepository;
    private readonly ICourseLessonRepository _courseLessonRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public ProgressService(
        FintrackDbContext context,
        ICourseProgressRepository courseProgressRepository,
        ICourseLessonProgressRepository courseLessonProgressRepository,
        ICourseLessonRepository courseLessonRepository,
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _context = context;
        _courseProgressRepository = courseProgressRepository;
        _courseLessonProgressRepository = courseLessonProgressRepository;
        _courseLessonRepository = courseLessonRepository;
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task UpdateProgress(Guid userId, LessonProgressDto dto)
    {
        var lesson = await _courseLessonRepository.FindAsync(dto.LessonId, false, c => c.Module);
        if (lesson == null) return;

        // upsert lesson progress
        var lssPrg = await _courseLessonProgressRepository.FindAsync(userId, dto.LessonId);
        if (lssPrg == null)
        {
            lssPrg = new CourseLessonProgress
            {
                LessonId = dto.LessonId,
                User = userId,
                StartDate = DateTime.UtcNow
            };
            await _courseLessonProgressRepository.AddAsync(lssPrg);
        }

        lssPrg.PercentCompleted = dto.CurrentTimestamp / lesson.Duration.TotalSeconds;
        lssPrg.TimeWatchedSeconds = dto.CurrentTimestamp;

        if (lssPrg.PercentCompleted != 100)
        {
            await _context.SaveChangesAsync();
            return;
        }

        if (lssPrg.CompletionDate == null)
            lssPrg.CompletionDate = DateTime.UtcNow;

        Course? course = await _courseRepository.FindAsync(lesson.Module.CourseId, false);

        // upsert course progress
        var crsPrg = await _courseProgressRepository.FindAsync(userId, lesson.Module.CourseId);
        if (crsPrg == null)
        {
            crsPrg = new CourseProgress
            {
                CourseId = lesson.Module.CourseId,
                User = userId,
                StartDate = DateTime.UtcNow,
                LessonsCompleted = 1
            };
            await _courseProgressRepository.AddAsync(crsPrg);
        }
        else
        {
            crsPrg.LessonsCompleted += 1;
        }

        if (crsPrg.LessonsCompleted == course!.LessonsLength && crsPrg.CompletionDate == null)
            crsPrg.CompletionDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<CourseProgressDto?> GetCourseProgress(Guid userId, Guid courseId)
    {
        var cp = await _courseProgressRepository.FindAsync(userId, courseId);
        return _mapper.Map<CourseProgressDto>(cp);
    }

    public async Task<List<LessonProgressDto>?> GetLessonsProgress(Guid userId, Guid courseId)
    {
        var lpList = await _courseLessonProgressRepository.GetCourseProgressionAsync(userId, courseId);
        return _mapper.Map<List<LessonProgressDto>?>(lpList);
    }
}
