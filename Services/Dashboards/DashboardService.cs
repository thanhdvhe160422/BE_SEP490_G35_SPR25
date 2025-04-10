using Planify_BackEnd.DTOs.Dashboards;
using Planify_BackEnd.Repositories.Dashboards;

namespace Planify_BackEnd.Services.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<List<StatisticsByMonthDTO>> GetMonthlyStatsAsync()
        {
            return await _dashboardRepository.GetMonthlyStatsAsync();
        }
        public async Task<List<CategoryUsageDTO>> GetUsedCategoriesAsync()
        {
            return await _dashboardRepository.GetUsedCategoriesAsync();
        }
        public async Task<List<RecentEventDTO>> GetLatestEventsAsync()
        {
            return await _dashboardRepository.GetLatestEventsAsync();
        }
        public async Task<List<TopEventByParticipantsDTO>> GetTopEventsByParticipantsAsync()
        {
            return await _dashboardRepository.GetTopEventsByParticipantsAsync();
        }
    }
    
}
