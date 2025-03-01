using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;


public class EventRepository : IEventRepository
{
    private readonly PlanifyContext _context;

    public EventRepository(PlanifyContext context)
    {
        _context = context;
    }

    public List<Event> GetAllEvent()
    {
        try
        {
            return _context.Events
                        .Include(e => e.Campus)
                        .Include(e => e.CategoryEvent)
                        .Include(e => e.CreateByNavigation)
                        .Include(e => e.EventMedia)
                        .Include(e => e.Groups)
                        .Include(e => e.JoinProjects)
                        .Include(e => e.Manager)
                        .Include(e => e.SendRequests)
                        .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
    }
}

