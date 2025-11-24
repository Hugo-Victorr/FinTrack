using System.Linq.Expressions;
using AutoMapper;
using FinTrack.Database.EFDao;
using FinTrack.Database.Interfaces;
using FinTrack.Education.Contracts;
using FinTrack.Education.DTOs;
using FinTrack.Model.Entities;

namespace FinTrack.Education.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICourseModuleRepository _courseModuleRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository repository, IMapper mapper, ICourseModuleRepository courseModuleRepository)
    {
        _courseRepository = repository;
        _mapper = mapper;
        _courseModuleRepository = courseModuleRepository;
    }

    public async Task<List<CourseDto>> GetAllAsync(QueryOptions opts)
    {
        var entities = await _courseRepository.AllAsync(
            opts,
            false,
            c => c.Category
        );
        return _mapper.Map<List<CourseDto>>(entities);
    }

    public async Task<CourseDetailsDto?> GetByIdAsync(Guid id, bool includeLessons = false)
    {
        Course? entity;
        
        if (includeLessons)
            entity = await _courseRepository.GetCourseComplete(id);
        else
            entity = await _courseRepository.FindAsync(id, false, c=>c.Category);

        if (entity is null)
            return null;

        CourseDetailsDto? res = _mapper.Map<CourseDetailsDto>(entity);

        if (includeLessons)
            res.Modules = GetLessonsDto(entity);

        return res;
    }

    public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
    {
        var entity = _mapper.Map<Course>(dto);
        await _courseRepository.AddAsync(entity);
        // await unitOfWork.CommitAsync();
        return _mapper.Map<CourseDto>(entity);
    }

    public async Task<CourseDto?> UpdateAsync(Guid id, CourseUpdateDto dto)
    {
        var entity = await _courseRepository.GetCourseComplete(id);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _courseRepository.UpdateAsync(entity);
        // await unitOfWork.CommitAsync();
        return _mapper.Map<CourseDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _courseRepository.FindAsync(id);
        if (entity is null) return false;

        await _courseRepository.DeleteAsync(id);
        // await unitOfWork.CommitAsync();
        return true;
    }

    private List<CourseContentDto> GetLessonsDto(Course course)
    {
        List<CourseContentDto> res = [];

        foreach (CourseModule module in course.Modules.OrderBy(k => k.OrderIndex))
        {
            CourseContentDto mod = new CourseContentDto(module.Title, module.OrderIndex, module.Id);

            foreach (CourseLesson lesson in module.Lessons.OrderBy(k => k.Order))
            {
                mod.AddLessonToModule(lesson.Title, lesson.Order, lesson.VideoUrl!, lesson.Id);
            }

            res.Add(mod);
        }

        return res;
    }
}
