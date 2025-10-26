
using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Database.Interfaces;

public class CourseRepository : BaseDao<Course>, ICourseRepository
{
    public CourseRepository(FintrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Course>> GetPagedAsync(int skip, int take, string? search)
    {
        var query = _context.Courses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(c => c.Title.Contains(search));

        return await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    protected override Task ValidateEntityForInsert(params Course[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params Course[] obj)
    {
        return Task.CompletedTask;
    }
}
