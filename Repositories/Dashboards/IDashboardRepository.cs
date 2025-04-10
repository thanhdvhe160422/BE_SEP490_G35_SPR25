using Planify_BackEnd.DTOs.Dashboards;

namespace Planify_BackEnd.Repositories.Dashboards
{
    public interface IDashboardRepository
    {
        Task<List<StatisticsByMonthDTO>> GetMonthlyStatsAsync();
        Task<List<CategoryUsageDTO>> GetUsedCategoriesAsync();
        Task<List<RecentEventDTO>> GetLatestEventsAsync();
        Task<List<TopEventByParticipantsDTO>> GetTopEventsByParticipantsAsync();
    }
}
