using AutoMapper;
using FinTrack.Database.EFDao;
using FinTrack.Database.Interfaces;
using FinTrack.Education.DTOs;
using FinTrack.Model.Entities;

namespace FinTrack.Education.Services;

public class CourseCategoryService(ICourseCategoryRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<CourseCategoryDto>> GetAllAsync(QueryOptions opts)
    {
        var entities = await repository.AllAsync(opts, false);
        return mapper.Map<IEnumerable<CourseCategoryDto>>(entities);
    }

    public async Task<CourseCategoryDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.FindAsync(id);
        return entity is null ? null : mapper.Map<CourseCategoryDto>(entity);
    }

    public async Task<CourseCategoryDto> CreateAsync(CourseCategoryCreateDto dto)
    {
        var entity = mapper.Map<CourseCategory>(dto);
        await repository.AddAsync(entity);
        // await unitOfWork.CommitAsync();
        return mapper.Map<CourseCategoryDto>(entity);
    }

    public async Task<CourseCategoryDto?> UpdateAsync(Guid id, CourseCategoryUpdateDto dto)
    {
        var entity = await repository.FindAsync(id);
        if (entity is null) return null;

        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);
        // await unitOfWork.CommitAsync();
        return mapper.Map<CourseCategoryDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await repository.FindAsync(id);
        if (entity is null) return false;

        await repository.DeleteAsync(id);
        // await unitOfWork.CommitAsync();
        return true;
    }
}
