using FinTrack.Infrastructure.DTO;

namespace FinTrack.Infrastructure.Interfaces;

public interface IStorageService
{
    Task<string> GenerateUploadUrl(string key, TimeSpan expiresIn);
    string GenerateDownloadUrl(string key, TimeSpan expiresIn);
}
