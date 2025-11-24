using FinTrack.Database.EFDao;
using FinTrack.Education.DTOs;

namespace FinTrack.Education.Contracts;

public interface ICourseService
{
    Task<List<CourseDto>> GetAllAsync(QueryOptions opts);
    Task<CourseDetailsDto?> GetByIdAsync(Guid id, bool includeLessons = false);
    Task<CourseDto> CreateAsync(CourseCreateDto dto);
    Task<CourseDto?> UpdateAsync(Guid id, CourseUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

