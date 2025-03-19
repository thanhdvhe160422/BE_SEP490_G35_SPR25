using Microsoft.AspNetCore.Http.HttpResults;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.Groups;
using Planify_BackEnd.Services.Events;
using static Planify_BackEnd.DTOs.Events.EventDetailResponseDTO;
using Planify_BackEnd.Services.GoogleDrive;
using Planify_BackEnd.Repositories.JoinGroups;
using Planify_BackEnd.Repositories.Categories;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGroupRepository _groupRepository;
    private readonly GoogleDriveService _googleDriveService;
    private readonly IJoinProjectRepository _joinProjectRepository;
    private readonly ICampusRepository _campusRepository;
    private readonly ICategoryRepository _categoryRepository;
    public EventService(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor, IGroupRepository groupRepository, GoogleDriveService googleDriveService, IJoinProjectRepository joinProjectRepository, ICampusRepository campusRepository, ICategoryRepository categoryRepository)
    {
        _eventRepository = eventRepository;
        _groupRepository = groupRepository;
        _httpContextAccessor = httpContextAccessor;
        _googleDriveService = googleDriveService;
        _joinProjectRepository = joinProjectRepository;
        _campusRepository = campusRepository;
        _categoryRepository = categoryRepository;
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
                        bool result = await _groupRepository.AddImplementerToGroupAsync(joinGroup);
                        if (result)
                        {
                            await _joinProjectRepository.AddImplementerToProject(implementerId, newEvent.Id);
                            await _joinProjectRepository.AddRoleImplementer(implementerId);
                        }
                    }
                }
            }
            return new ResponseDTO(201, "Event creates successfully!", newEvent);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while creating event!", ex.Message);
        }
    }

    public async Task<ResponseDTO> UploadImageAsync(UploadImageRequestDTO imageDTO)
    {
        if (imageDTO.EventMediaFiles != null && imageDTO.EventMediaFiles.Any())
        {
            var uploadTasks = imageDTO.EventMediaFiles.Select(async file =>
            {
                using var stream = file.OpenReadStream();
                string contentType = file.ContentType; // Lấy loại file (image/jpeg, image/png,...)
                string driveUrl = await _googleDriveService.UploadFileAsync(stream, file.FileName, contentType);

                var mediaItem = new Medium { MediaUrl = driveUrl };
                await _eventRepository.CreateMediaItemAsync(mediaItem);

                var eventMedia = new EventMedium
                {
                    EventId = imageDTO.EventId,
                    MediaId = mediaItem.Id,
                    Status = 1
                };
                await _eventRepository.AddEventMediaAsync(eventMedia);
            });

            await System.Threading.Tasks.Task.WhenAll(uploadTasks); // Chạy tất cả các task upload cùng lúc
        }

        return new ResponseDTO(201, "Upload Image successfully!", null);
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

    public async Task<EventDetailDto> UpdateEventAsync(EventDTO e)
    {
        try
        {
            var campus = await _campusRepository.GetCampusByName(e.CampusName);
            var category = await _categoryRepository.GetCategoryByName(e.CategoryEventName, campus.Id);
            Event updateEvent = new Event
            {
                Id = e.Id,
                AmountBudget = e.AmountBudget,
                CampusId = campus.Id,
                CategoryEventId = category.Id,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                EventDescription = e.EventDescription,
                EventTitle = e.EventTitle,
                IsPublic = e.IsPublic,
                Placed = e.Placed,
                Status = e.Status,
                TimePublic = e.TimePublic,
                UpdateBy = e.UpdateBy,
                UpdatedAt = DateTime.Now
            };
            Event updatedEvent = await _eventRepository.UpdateEventAsync(updateEvent);
            EventDetailDto eventDetailResponseDTO = new EventDetailDto
            {
                Id = updatedEvent.Id,
                EventTitle = updatedEvent.EventTitle,
                EventDescription = updatedEvent.EventDescription,
                StartTime = updatedEvent.StartTime,
                EndTime = updatedEvent.EndTime,
                AmountBudget = updatedEvent.AmountBudget,
                IsPublic = updatedEvent.IsPublic,
                TimePublic = updatedEvent.TimePublic,
                Status = updatedEvent.Status,
                Placed = updatedEvent.Placed,
                CreatedAt = updatedEvent.CreatedAt,
                CampusName = updatedEvent.Campus.CampusName,
                CategoryEventName = updatedEvent.CategoryEvent.CategoryEventName,
                CreatedBy = new UserDto
                {
                    Id = updatedEvent.CreateByNavigation.Id,
                    FirstName = updatedEvent.CreateByNavigation.FirstName,
                    LastName = updatedEvent.CreateByNavigation.LastName,
                    Email = updatedEvent.CreateByNavigation.Email
                },
                EventMedia = updatedEvent.EventMedia.Select(em => new EventMediaDto
                {
                    Id = em.Id,
                    MediaUrl = em.Media.MediaUrl
                }).ToList()
            };
            return eventDetailResponseDTO;
        }catch(Exception ex)
        {
            Console.WriteLine("event service - update event: " + ex.Message);
            return new EventDetailDto();
        }
    }

    public async Task<bool> DeleteEventAsync(int eventId)
    {
        try
        {
            return await _eventRepository.DeleteEventAsync(eventId);
        }catch(Exception ex)
        {
            Console.WriteLine("event service - delete event: " + ex.Message);
            return false;
        }
    }

    public async Task<IEnumerable<EventGetListResponseDTO>> SearchEventAsync(int page, int pageSize, string? title, DateTime? startTime, DateTime? endTime, decimal? minBudget, decimal? maxBudget, int? isPublic, int? status, int? CategoryEventId, string? placed)
    {
        try
        {
            var events = await _eventRepository.SearchEventAsync(page,pageSize,title,startTime,endTime,
                minBudget,maxBudget,isPublic,status,CategoryEventId,placed);
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
        }catch(Exception ex)
        {
            Console.WriteLine("event service - search event: " + ex.Message);
            return new List<EventGetListResponseDTO>();
        }
    }
}

