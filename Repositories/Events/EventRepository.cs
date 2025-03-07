
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using static Planify_BackEnd.DTOs.Events.EventDetailResponseDTO;


public class EventRepository : IEventRepository
{
    private readonly PlanifyContext _context;

    public EventRepository(PlanifyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllEvent(int page, int pageSize)
    {
        try
        {
            return await _context.Events
                 .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();
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

    public async System.Threading.Tasks.Task CreateMediaItemAsync(Medium mediaItem)
    {
        _context.Media.Add(mediaItem);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task AddEventMediaAsync(EventMedium eventMedia)
    {
        _context.EventMedia.Add(eventMedia);
        await _context.SaveChangesAsync();
    }

    public async Task<CategoryEvent> GetCategoryEventAsync(int categoryId, int campusId)
    {
        try
        {
            return await _context.CategoryEvents.FirstOrDefaultAsync(c => c.Id == categoryId && c.CampusId == campusId);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }

    public async Task<EventDetailDto?> GetEventDetailAsync(int eventId)
    {
        if (eventId <= 0)
        {
            throw new ArgumentException("ID sự kiện phải là số nguyên dương.", nameof(eventId));
        }

        try
        {
            var eventDetail = await _context.Events
            .Where(e => e.Id == eventId)
            .Select(e => new EventDetailDto
            {
                Id = e.Id,
                EventTitle = e.EventTitle,
                EventDescription = e.EventDescription,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                AmountBudget = e.AmountBudget,
                IsPublic = e.IsPublic,
                TimePublic = e.TimePublic,
                Status = e.Status,
                Placed = e.Placed,
                CreatedAt = e.CreatedAt,
                CampusName = e.Campus.CampusName,
                CategoryEventName = e.CategoryEvent.CategoryEventName,
                CreatedBy = new UserDto
                {
                    Id = e.CreateByNavigation.Id,
                    FirstName = e.CreateByNavigation.FirstName,
                    LastName = e.CreateByNavigation.LastName,
                    Email = e.CreateByNavigation.Email
                },
                EventMedia = e.EventMedia.Select(em => new EventMediaDto
                {
                    Id = em.Id,
                    MediaUrl = em.Media.MediaUrl
                }).ToList(),
                Groups = e.Groups.Select(g => new GroupDto
                {
                    Id = g.Id,
                    GroupName = g.GroupName,
                    AmountBudget = g.AmountBudget,
                    JoinGroups = g.JoinGroups.Select(jg => new JoinGroupDto
                    {
                        Id = jg.Id,
                        ImplementerId = jg.ImplementerId,
                        ImplementerFirstName = jg.Implementer.FirstName,
                        ImplementerLastName = jg.Implementer.LastName,
                        TimeJoin = jg.TimeJoin,
                        TimeOut = jg.TimeOut,
                        Status = jg.Status
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

            return eventDetail;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }
}

