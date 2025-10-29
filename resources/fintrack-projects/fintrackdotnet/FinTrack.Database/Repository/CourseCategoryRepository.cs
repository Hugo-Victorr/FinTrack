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

    protected override Task ValidateEntityForInsert(params CourseCategory[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseCategory[] obj)
    {
        return Task.CompletedTask;
    }
}
