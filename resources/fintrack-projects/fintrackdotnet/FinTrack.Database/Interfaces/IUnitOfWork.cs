namespace FinTrack.Database.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}
