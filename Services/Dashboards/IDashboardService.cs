using Planify_BackEnd.DTOs.Dashboards;

namespace Planify_BackEnd.Services.Dashboards
{
    public interface IDashboardService
    {
        Task<List<StatisticsByMonthDTO>> GetMonthlyStatsAsync();
        Task<List<CategoryUsageDTO>> GetUsedCategoriesAsync();
        Task<List<RecentEventDTO>> GetLatestEventsAsync();
    }
}
