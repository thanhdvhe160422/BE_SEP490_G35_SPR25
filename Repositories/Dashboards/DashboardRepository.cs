using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs.Dashboards;
using Planify_BackEnd.DTOs.Events;
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
        public async Task<List<RecentEventDTO>> GetLatestEventsAsync()
        {
            return await _context.Events
                .Where(e => e.Status == 1 || e.Status == 2)
                .OrderByDescending(e => e.StartTime)
                .Take(5)
                .Select(e => new RecentEventDTO
                {
                    Id = e.Id,
                    EventTitle = e.EventTitle,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Status = e.Status,
                    EventDescription = e.EventDescription,
                    IsPublic = e.IsPublic,
                    CampusId = e.CampusId,
                    CategoryEventId = e.CategoryEventId,
                    Placed = e.Placed,
                    MeasuringSuccess = e.MeasuringSuccess,
                    Goals = e.Goals,
                    MonitoringProcess = e.MonitoringProcess,
                    SizeParticipants = e.SizeParticipants,
                    CreatedAt = e.CreatedAt,
                    AmountBudget = e.AmountBudget,
                    CreateBy = e.CreateBy,
                    TimePublic = e.TimePublic,
                    ManagerId = e.ManagerId,
                    
                })
                .ToListAsync();
        }
        public async Task<List<TopEventByParticipantsDTO>> GetTopEventsByParticipantsAsync()
        {
            return await _context.Events
                .Where(e => e.Status == 1 || e.Status == 2) 
                .OrderByDescending(e => e.Participants.Count)
                .Take(10)
                .Select(e => new TopEventByParticipantsDTO
                {
                    Id = e.Id,
                    EventTitle = e.EventTitle,
                    StartTime = e.StartTime,
                    TotalParticipants = e.Participants.Count
                })
                .ToListAsync();
        }

    }


}
