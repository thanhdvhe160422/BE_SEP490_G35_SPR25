
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;


public class EventRepository : IEventRepository
{
    private readonly PlanifyContext _context;

    public EventRepository(PlanifyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllEvent()
    {
        try
        {
            return await _context.Events.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<Event> CreateEventAsync(Event newEvent)
    {
        try
        {
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            return newEvent;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }
    public async System.Threading.Tasks.Task CreateMediaItemAsync(MediaItem mediaItem)
    {
        _context.MediaItems.Add(mediaItem);
        await _context.SaveChangesAsync();
    }
    public async System.Threading.Tasks.Task AddEventMediaAsync(EventMedium eventMedia)
    {
        _context.EventMedia.Add(eventMedia);
        await _context.SaveChangesAsync();
    }
}

