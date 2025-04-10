using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs.Dashboards;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Dashboards
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PlanifyContext _context;
        public DashboardRepository(PlanifyContext context)
        {
            _context = context;
        }
        public async Task<List<StatisticsByMonthDTO>> GetMonthlyStatsAsync()
        {
            var query = await _context.Events
                .Where(e => e.Status == 2 && e.EndTime < DateTime.UtcNow)
                .GroupBy(e => new { e.StartTime.Year, e.StartTime.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Events = g.ToList() 
                })
                .ToListAsync(); 

            
            var result = query.Select(g => new StatisticsByMonthDTO
            {
                Month = $"{g.Year}-{g.Month:D2}",
                TotalEvents = g.Events.Count,
                TotalParticipants = g.Events.Sum(e => e.Participants.Count)
            })
            .OrderBy(x => x.Month)
            .ToList();

            return result;
        }

        public async Task<List<CategoryUsageDTO>> GetUsedCategoriesAsync()
        {
            return await _context.Events
                .Where(e => e.Status == 2)
                .GroupBy(e => new { e.CategoryEventId, e.CategoryEvent.CategoryEventName })
                .Select(g => new CategoryUsageDTO
                {
                    CategoryEventId = g.Key.CategoryEventId,
                    CategoryEventName = g.Key.CategoryEventName,
                    TotalUsed = g.Count()
                })
                .OrderByDescending(x => x.TotalUsed)
                .ToListAsync();
        }


    }


}
