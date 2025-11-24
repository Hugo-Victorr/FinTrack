using AutoMapper;
using FinTrack.Education.DTOs;
using FinTrack.Model.Entities;

namespace Fintrack.Education.Mappings;

public class EducationProfile : Profile
{
    public EducationProfile()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<Course, CourseDetailsDto>()
            .ForMember(dest => dest.Modules, opt => opt.Ignore());
        CreateMap<CourseLessonDto, CourseLesson>();

        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseModuleDto, CourseModule>();
        CreateMap<CourseUpdateDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
        CreateMap<CourseContentDto, CourseModule>()
            .ForMember(dest => dest.OrderIndex, opt => opt.MapFrom(src => src.Order));
        CreateMap<CourseContentDto, CourseLesson>()
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
            
        CreateMap<CourseCategory, CourseCategoryDto>();

        CreateMap<CourseCategoryCreateDto, CourseCategory>();
        CreateMap<CourseCategoryUpdateDto, CourseCategory>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<CourseProgressDto, CourseProgress>();
        CreateMap<LessonProgressDto, CourseLessonProgress>();
        
        CreateMap<CourseLessonProgress, LessonProgressDto>()
            .ForMember(dest => dest.LessonId, opt => opt.MapFrom(src => src.LessonId))
            .ForMember(dest => dest.CurrentTimestamp, opt => opt.MapFrom(src => src.TimeWatchedSeconds))
            .ForMember(dest => dest.ProgressPercentage, opt => opt.MapFrom(src => (int)Math.Round(src.PercentCompleted * 100)));
    }
}
