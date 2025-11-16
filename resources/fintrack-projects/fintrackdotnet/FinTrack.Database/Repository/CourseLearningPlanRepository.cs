using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public class CourseLearningPlanRepository : BaseDao<CourseLearningPlan>, ICourseLearningPlanRepository
{
    public CourseLearningPlanRepository(FintrackDbContext context) : base(context)
    {
    }

    protected override Task ValidateEntityForInsert(params CourseLearningPlan[] obj)
    {
        return Task.CompletedTask;
    }

    protected override Task ValidateEntityForUpdate(params CourseLearningPlan[] obj)
    {
        return Task.CompletedTask;
    }
}