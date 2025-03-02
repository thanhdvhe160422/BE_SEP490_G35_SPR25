using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Services.Events;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EventService(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor)
    {
        _eventRepository = eventRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public ResponseDTO GetAllEvent()
    {
        var events = _eventRepository.GetAllEvent();
        return new ResponseDTO(200, "Events retrieved successfully", events);
    }

    public async Task<ResponseDTO> CreateEventAsync(EventCreateRequestDTO eventDTO, Guid organizerId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(eventDTO.EventTitle))
            {
                return new ResponseDTO(400, "Event title is required.", null);
            }

            if (eventDTO.StartTime >= eventDTO.EndTime)
            {
                return new ResponseDTO(400, "Start time must be earlier than end time.", null);
            }

            if (eventDTO.TimePublic < DateTime.Now)
            {
                return new ResponseDTO(400, "Public time must be later than current time.", null);
            }

            var campusIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("campusId")?.Value;
            if (string.IsNullOrEmpty(campusIdClaim))
            {
                return new ResponseDTO(400, "Invalid campus ID.", null);
            }

            var newEvent = new Event
            {
                EventTitle = eventDTO.EventTitle,
                EventDescription = eventDTO.EventDescription,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                AmountBudget = eventDTO.AmountBudget,
                IsPublic = eventDTO.IsPublic ? 1 : 0,
                TimePublic = eventDTO.TimePublic,
                Status =  0,
                CampusId = int.Parse(campusIdClaim),
                CategoryEventId = eventDTO.CategoryEventId,
                Placed = eventDTO.Placed,
                CreateBy = organizerId,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.CreateEventAsync(newEvent);

            return new ResponseDTO(201, "Event creates successfully!", newEvent);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while creating event!", ex.Message);
        }
    }
}

