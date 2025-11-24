using FinTrack.Database.EFDao;
using FinTrack.Education.DTOs;

namespace FinTrack.Education.Contracts;

public interface ICourseCategoryService
{
    Task<List<CourseCategoryDto>> GetAllAsync(QueryOptions opts);
    Task<CourseCategoryDto?> GetByIdAsync(Guid id);
    Task<CourseCategoryDto> CreateAsync(CourseCategoryCreateDto dto);
    Task<CourseCategoryDto?> UpdateAsync(Guid id, CourseCategoryUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

