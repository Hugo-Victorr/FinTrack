using AutoMapper;
using FinTrack.Database.EFDao;
using FinTrack.Database.Interfaces;
using FinTrack.Education.DTOs;
using FinTrack.Model.Entities;

namespace FinTrack.Education.Services;

public class CourseService(ICourseRepository repository, IMapper mapper)
{
    public async Task<IEnumerable<CourseDto>> GetAllAsync(QueryOptions opts)
    {
        var entities = await repository.AllAsync(
            opts,
            false,
            c => c.Category
        );
        return mapper.Map<IEnumerable<CourseDto>>(entities);
    }

    public async Task<CourseDto?> GetByIdAsync(Guid id)
    {
        var entity = await repository.FindAsync(id);
        return entity is null ? null : mapper.Map<CourseDto>(entity);
    }

    public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
    {
        var entity = mapper.Map<Course>(dto);
        await repository.AddAsync(entity);
        // await unitOfWork.CommitAsync();
        return mapper.Map<CourseDto>(entity);
    }

    public async Task<CourseDto?> UpdateAsync(Guid id, CourseUpdateDto dto)
    {
        var entity = await repository.FindAsync(id);
        if (entity is null) return null;

        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);
        // await unitOfWork.CommitAsync();
        return mapper.Map<CourseDto>(entity);
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
