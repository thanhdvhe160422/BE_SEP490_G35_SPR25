using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.Groups;
using Planify_BackEnd.Services.Events;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGroupRepository _groupRepository;
    public EventService(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor, IGroupRepository groupRepository)
    {
        _eventRepository = eventRepository;
        _groupRepository = groupRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IEnumerable<EventGetListResponseDTO>> GetAllEvent()
    {
        var events =  await _eventRepository.GetAllEvent();
        var eventDTOs = events.Select(e => new EventGetListResponseDTO
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
            CampusId = e.CampusId,
            CategoryEventId = e.CategoryEventId,
            Placed = e.Placed,
            CreateBy = e.CreateBy,
            CreatedAt = e.CreatedAt,
            EndOfEvent = e.EndOfEvent,
            ManagerId = e.ManagerId,
            TimeOfEvent = e.TimeOfEvent

        }).ToList();
        
      return eventDTOs;
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

            if (eventDTO.TimePublic.HasValue && eventDTO.TimePublic < DateTime.Now)
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

            foreach (var group in eventDTO.Groups)
            {
                var newGroup = new Group
                {
                    EventId = newEvent.Id,
                    GroupName = group.GroupName,
                    CreateBy = organizerId,
                };
                await _groupRepository.CreateGroupAsync(newGroup);

                foreach (var implementerId in group.ImplementerIds)
                {
                    var joinGroup = new JoinGroup
                    {
                        GroupId = newGroup.Id,
                        ImplementerId = implementerId,
                        TimeJoin = DateTime.UtcNow,
                        Status = 1,
                    };
                    await _groupRepository.AddImplementerToGroupAsync(joinGroup);
                }
            }

            foreach (var image in eventDTO.EventMediaUrls)
            {
                var mediaItem = new MediaItem
                {
                    MediaUrl = image
                };
                await _eventRepository.CreateMediaItemAsync(mediaItem);

                var eventMedia = new EventMedium
                {
                    EventId = newEvent.Id,
                    MediaId = mediaItem.Id,
                    Status = 1
                };
                await _eventRepository.AddEventMediaAsync(eventMedia);
            }         

            return new ResponseDTO(201, "Event creates successfully!", newEvent);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while creating event!", ex.Message);
        }
    }
}

