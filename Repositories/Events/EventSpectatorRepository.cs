using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Events
{
    public class EventSpectatorRepository : IEventSpectatorRepository
    {
        private readonly PlanifyContext _context;
        public EventSpectatorRepository(PlanifyContext context)
        {
            _context = context;
        }
        public Event GetEventById(int id)
        {
            return _context.Events
                .Include(e=>e.Campus)
                .Include(e=>e.CategoryEvent)
                .Include(e=>e.EventMedia)
                .ThenInclude(e=>e.Media)
                .FirstOrDefault(em => em.Id == id);
        }

        public List<Event> GetEventsOrderByStartDate(int page, int pageSize)
        {
            return _context.Events
                .Include (e=>e.Campus)
                .Include(e=>e.CategoryEvent)
                .Include(e=>e.EventMedia) .ThenInclude(em=>em.Media)
                .OrderBy(e=>e.StartTime)
                .Skip((page-1)*pageSize).Take(pageSize).ToList();
        }

        public List<Event> SearchEventOrderByStartDate(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            if (name == null) name = "";
            return _context.Events
                .Include(e => e.Campus)
                .Include(e => e.CategoryEvent)
                .Include(e => e.EventMedia).ThenInclude(em => em.Media)
                .Where(e=>e.EventTitle.Contains(name)
                &&e.StartTime>=startDate&&e.EndTime<=endDate)
                .OrderBy(e => e.StartTime)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
