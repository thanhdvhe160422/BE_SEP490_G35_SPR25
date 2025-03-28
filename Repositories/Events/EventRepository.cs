
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;


public class EventRepository : IEventRepository
{
    private readonly PlanifyContext _context;

    public EventRepository(PlanifyContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllEventAsync(int page, int pageSize)
    {
        try
        {
            return await _context.Events
                .Where(e => e.Status != -1)
                .Include(e => e.EventMedia) 
                .ThenInclude(em => em.Media) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
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

    public async Task<EventDetailDto> GetEventDetailAsync(int eventId)
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
                    UpdatedAt = e.UpdatedAt,
                    MeasuringSuccess = e.MeasuringSuccess,
                    Goals = e.Goals,
                    MonitoringProcess = e.MonitoringProcess,
                    SizeParticipants = e.SizeParticipants,
                    CampusName = e.Campus.CampusName,
                    CategoryEventName = e.CategoryEvent.CategoryEventName,
                    CreatedBy = new UserDto
                    {
                        Id = e.CreateByNavigation.Id,
                        FirstName = e.CreateByNavigation.FirstName,
                        LastName = e.CreateByNavigation.LastName,
                        Email = e.CreateByNavigation.Email
                    },
                    Manager = e.Manager != null ? new UserDto
                    {
                        Id = e.Manager.Id,
                        FirstName = e.Manager.FirstName,
                        LastName = e.Manager.LastName,
                        Email = e.Manager.Email
                    } : null,
                    UpdatedBy = e.UpdateByNavigation != null ? new UserDto
                    {
                        Id = e.UpdateByNavigation.Id,
                        FirstName = e.UpdateByNavigation.FirstName,
                        LastName = e.UpdateByNavigation.LastName,
                        Email = e.UpdateByNavigation.Email
                    } : null,
                    EventMedia = e.EventMedia.Select(em => new EventMediaDto
                    {
                        Id = em.Id,
                        MediaUrl = em.Media.MediaUrl
                    }).ToList(),
                    FavouriteEvents = e.FavouriteEvents.Select(fe => new FavouriteEventDto
                    {
                        UserId = fe.UserId,
                        UserFullName = $"{fe.User.FirstName} {fe.User.LastName}"
                    }).ToList(),
                    JoinProjects = e.JoinProjects.Select(jp => new JoinProjectDto
                    {
                        UserId = jp.UserId,
                        UserFullName = $"{jp.User.FirstName} {jp.User.LastName}",
                        TimeJoinProject = jp.TimeJoinProject,
                        TimeOutProject = jp.TimeOutProject
                    }).ToList(),
                    Risks = e.Risks.Select(r => new RiskDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Reason = r.Reason,
                        Solution = r.Solution,
                        Description = r.Description
                    }).ToList(),
                    Tasks = e.Tasks.Select(t => new TaskDetailDto
                    {
                        Id = t.Id,
                        TaskName = t.TaskName,
                        TaskDescription = t.TaskDescription,
                        StartTime = t.StartTime,
                        Deadline = t.Deadline,
                        AmountBudget = t.AmountBudget,
                        CreatedAt = t.CreateDate,
                        CreatedBy = new UserDto
                        {
                            Id = t.CreateByNavigation.Id,
                            FirstName = t.CreateByNavigation.FirstName,
                            LastName = t.CreateByNavigation.LastName,
                            Email = t.CreateByNavigation.Email
                        },
                        SubTasks = t.SubTasks.Select(st => new SubTaskDetailDto
                        {
                            Id = st.Id,
                            SubTaskName = st.SubTaskName,
                            SubTaskDescription = st.SubTaskDescription,
                            StartTime = st.StartTime,
                            Deadline = st.Deadline,
                            AmountBudget = st.AmountBudget,
                            CreatedBy = new UserDto
                            {
                                Id = st.CreateByNavigation.Id,
                                FirstName = st.CreateByNavigation.FirstName,
                                LastName = st.CreateByNavigation.LastName,
                                Email = st.CreateByNavigation.Email
                            }
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return eventDetail;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while retrieving event details.", ex);
        }
    }

    public async Task<Event> UpdateEventAsync(Event e)
    {
        try
        {
            var updateEvent = await _context.Events.FirstOrDefaultAsync(ev=>ev.Id==e.Id);
            updateEvent.Id = e.Id;
            updateEvent.AmountBudget = e.AmountBudget;
            updateEvent.CampusId = e.CampusId;
            updateEvent.CategoryEventId = e.CategoryEventId;
            updateEvent.StartTime = e.StartTime;
            updateEvent.EndTime = e.EndTime;
            updateEvent.EventDescription = e.EventDescription;
            updateEvent.EventTitle = e.EventTitle;
            updateEvent.IsPublic = e.IsPublic;
            updateEvent.Placed = e.Placed;
            updateEvent.Status = e.Status;
            updateEvent.TimePublic = e.TimePublic;
            updateEvent.UpdateBy = e.UpdateBy;
            updateEvent.UpdatedAt = DateTime.Now;
            _context.Events.Update(updateEvent);
            await _context.SaveChangesAsync();
            var updatedEvent = _context.Events
                .Include(ue=>ue.Campus)
                .Include(ue=>ue.CategoryEvent)
                .Include(ue=>ue.CreateByNavigation)
                .Include(ue=>ue.EventMedia).ThenInclude(em=>em.Media)
                .FirstOrDefault(ue => ue.Id == e.Id);
            return updatedEvent;
        }catch(Exception ex)
        {
            Console.WriteLine("event repository - update event: " + ex.Message);
            return new Event();
        }
    }

    public async Task<bool> DeleteEventAsync(int eventId)
    {
        try
        {
            //var ev = await _context.Events
            //    .Include(e=>e.JoinProjects)
            //    .Include(e=>e.EventMedia).ThenInclude(em=>em.Media)
            //    .Include(e=>e.Groups)
            //    .Include(e=>e.SendRequests)
            //    .FirstOrDefaultAsync(e => e.Id == eventId);
            //ev.JoinProjects = null;
            //ev.EventMedia = null;
            //ev.Groups = null;
            //ev.SendRequests = null;
            //_context.Events.Remove(ev!);
            var ev = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            ev.Status = -1;
            await _context.SaveChangesAsync();
            return true;
        }catch(Exception ex)
        {
            Console.WriteLine("event repository - delete event: "+ex.Message);
            return false;
        }
    }

    public async Task<IEnumerable<Event>> SearchEventAsync(int page, int pageSize, string? title, 
        DateTime? startTime, DateTime? endTime, decimal? minBudget, decimal? maxBudget, int? isPublic, 
        int? status, int? CategoryEventId, string? placed)
    {
        try
        {
            var query = _context.Events.AsQueryable();
            if (!string.IsNullOrEmpty(title))
                query = query.Where(e => e.EventTitle.Contains(title));
            if (!string.IsNullOrEmpty(placed))
                query = query.Where(e => e.Placed.Contains(placed));
            if (startTime.HasValue)
                query = query.Where(e => e.StartTime >= startTime);
            if (endTime.HasValue)
                query = query.Where(e => e.EndTime <= endTime);
            if (minBudget.HasValue)
                query = query.Where(e => e.AmountBudget >= minBudget);
            if (maxBudget.HasValue)
                query = query.Where(e => e.AmountBudget <= maxBudget);
            if (isPublic.HasValue)
                query = query.Where(e => e.IsPublic == isPublic);
            if (status.HasValue)
                query = query.Where(e => e.Status == status);
            if (CategoryEventId.HasValue)
                query = query.Where(e => e.CategoryEventId == CategoryEventId); 
            var paginatedResult = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
            return paginatedResult;
        }
        catch(Exception ex)
        {
            Console.WriteLine("event repository - search event: " + ex.Message);
            return new List<Event>();
        }
    }
    public async System.Threading.Tasks.Task CreateRiskAsync(Risk risk)
    {
        _context.Risks.Add(risk);
        await _context.SaveChangesAsync();
    }
}

