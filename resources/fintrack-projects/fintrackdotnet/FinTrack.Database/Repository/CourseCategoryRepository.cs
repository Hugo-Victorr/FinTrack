using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Database.Interfaces;

public class CourseCategoryRepository : BaseDao<CourseCategory>, ICourseCategoryRepository
{
    public CourseCategoryRepository(FintrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CourseCategory>> GetPagedAsync(int skip, int take, string? search)
    {
        var query = _context.CourseCategories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Name.Contains(search));

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    protected override Task ValidateEntityForInsert(params CourseCategory[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseCategory[] obj)
    {
        return Task.CompletedTask;
    }
}
