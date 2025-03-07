using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<IEnumerable<EventGetListResponseDTO>> GetAllEvent(int page, int pageSize)
    {
        var events =  await _eventRepository.GetAllEvent( page,  pageSize);
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
            ManagerId = e.ManagerId,

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

            var campusIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("campusId")?.Value;
            if (string.IsNullOrEmpty(campusIdClaim))
            {
                return new ResponseDTO(400, "Invalid campus ID.", null);
            }

            var category = await _eventRepository.GetCategoryEventAsync(eventDTO.CategoryEventId, int.Parse(campusIdClaim));
            if (category == null)
            {
                return new ResponseDTO(400, "Category is not existed.", null);
            }

            var newEvent = new Event
            {
                EventTitle = eventDTO.EventTitle,
                EventDescription = eventDTO.EventDescription,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                AmountBudget = eventDTO.AmountBudget,
                IsPublic = 0,
                TimePublic = null,
                Status =  0,
                CampusId = int.Parse(campusIdClaim),
                CategoryEventId = eventDTO.CategoryEventId,
                Placed = eventDTO.Placed,
                CreateBy = organizerId,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.CreateEventAsync(newEvent);

            if (eventDTO.Groups != null && eventDTO.Groups.Count > 0)
            {
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
            }
            
            if (eventDTO.EventMediaUrls != null && eventDTO.EventMediaUrls.Any())
            {
                foreach (var image in eventDTO.EventMediaUrls)
                {
                    var mediaItem = new Medium
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
            }

            return new ResponseDTO(201, "Event creates successfully!", null);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while creating event!", ex.Message);
        }
    }

    public async Task<ResponseDTO> GetEventDetailAsync(int eventId)
    {
        try
        {
            var eventDetail = await _eventRepository.GetEventDetailAsync(eventId);
            if (eventDetail == null)
            {
                return new ResponseDTO(404, $"Không tìm thấy sự kiện với ID là {eventId}.", eventDetail);
            }
            return new ResponseDTO(200, "Get event detail successfully!", eventDetail);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while getting event detail!", ex.Message);
        }
    }
}

