using FinTrack.Model.DTO;

namespace FinTrack.Analytics.Contracts
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetDashboardSummaryAsync(Guid userId, string period = "current");
    }
}







