using FinTrack.Database;
using FinTrack.Database.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly FintrackDbContext _db;
    public UnitOfWork(FintrackDbContext db) => _db = db;
    public Task<int> CommitAsync() => _db.SaveChangesAsync();
}
