using FinTrack.Database.EFDao;
using FinTrack.Database.Repository;
using FinTrack.Model.Entities;

namespace FinTrack.Database.Interfaces;

public class CourseLessonRepository : BaseDao<CourseLesson>, ICourseLessonRepository
{
    public CourseLessonRepository(FintrackDbContext context) : base(context)
    {
    }

    protected override Task ValidateEntityForInsert(params CourseLesson[] obj)
    {
        throw new NotImplementedException();
    }

    protected override Task ValidateEntityForUpdate(params CourseLesson[] obj)
    {
        throw new NotImplementedException();
    }
}