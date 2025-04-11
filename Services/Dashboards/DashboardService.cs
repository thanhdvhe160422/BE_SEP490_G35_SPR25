using Planify_BackEnd.DTOs.Dashboards;
using Planify_BackEnd.Repositories.Dashboards;

namespace Planify_BackEnd.Services.Dashboards
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly ICampusRepository _campusRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<List<StatisticsByMonthDTO>> GetMonthlyStatsAsync(int year)
        {
            return await _dashboardRepository.GetMonthlyStatsAsync(year);
        }
        public async Task<List<CategoryUsageDTO>> GetUsedCategoriesAsync()
        {
            try
            {
                var list = await _dashboardRepository.GetUsedCategoriesAsync();
                var total = list.Sum(c => c.TotalUsed);
                foreach (var item in list)
                {
                    item.Percentage = total > 0 ? (item.TotalUsed * 100) / total : 0;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<RecentEventDTO>> GetLatestEventsAsync(string campusName)
        {
            try
            {
                var campusId = await _campusRepository.GetCampusByName(campusName);
                return await _dashboardRepository.GetLatestEventsAsync(campusId.Id);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<TopEventByParticipantsDTO>> GetTopEventsByParticipantsAsync()
        {
            return await _dashboardRepository.GetTopEventsByParticipantsAsync();
        }
    }
    
}
