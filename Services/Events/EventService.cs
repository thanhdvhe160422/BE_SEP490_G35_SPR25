using Microsoft.AspNetCore.Http.HttpResults;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Services.Events;
using Planify_BackEnd.Services.GoogleDrive;
using Planify_BackEnd.Repositories.JoinGroups;
using Planify_BackEnd.Repositories.Categories;
using Planify_BackEnd.Repositories.Tasks;
using Microsoft.Extensions.Logging;
using Planify_BackEnd.DTOs.Medias;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITaskRepository _taskRepository;
    private readonly ISubTaskRepository _subTaskRepository;
    private readonly GoogleDriveService _googleDriveService;
    private readonly IJoinProjectRepository _joinProjectRepository;
    private readonly ICampusRepository _campusRepository;
    private readonly ICategoryRepository _categoryRepository;
    public EventService(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor, GoogleDriveService googleDriveService, IJoinProjectRepository joinProjectRepository, ICampusRepository campusRepository, ICategoryRepository categoryRepository, ISubTaskRepository subTaskRepository, ITaskRepository taskRepository)
    {
        _eventRepository = eventRepository;
        _httpContextAccessor = httpContextAccessor;
        _googleDriveService = googleDriveService;
        _joinProjectRepository = joinProjectRepository;
        _campusRepository = campusRepository;
        _categoryRepository = categoryRepository;
        _subTaskRepository = subTaskRepository;
        _taskRepository = taskRepository;
    }
    public async Task<IEnumerable<EventGetListResponseDTO>> GetAllEventAsync(int campusId, int page, int pageSize)
    {
        var events = await _eventRepository.GetAllEventAsync(campusId,page, pageSize);
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
            MeasuringSuccess = e.MeasuringSuccess,
            Goals = e.Goals,
            MonitoringProcess = e.MonitoringProcess,
            SizeParticipants = e.SizeParticipants,
            EventMedias = e.EventMedia == null ? null : e.EventMedia.Select(em => new Planify_BackEnd.DTOs.Medias.EventMediumViewMediaModel
            {
                Id = em.Id,
                EventId = em.Id,
                MediaId = em.Id,
                Status = em.Status,
                MediaDTO = new Planify_BackEnd.DTOs.Medias.MediaItemDTO
                {
                    Id = em.Media.Id,
                    MediaUrl = em.Media.MediaUrl
                },
            }).ToList()

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
            int campusId = int.Parse(campusIdClaim);

            var category = await _eventRepository.GetCategoryEventAsync(eventDTO.CategoryEventId, campusId);
            if (category == null)
            {
                return new ResponseDTO(400, "Category does not exist or does not belong to this campus.", null);
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
                Status = 0,
                CampusId = campusId,
                CategoryEventId = eventDTO.CategoryEventId,
                Placed = eventDTO.Placed,
                CreateBy = organizerId,
                CreatedAt = DateTime.UtcNow,
                MeasuringSuccess = eventDTO.MeasuringSuccess,
                Goals = eventDTO.Goals,
                MonitoringProcess = eventDTO.MonitoringProcess,
                SizeParticipants = eventDTO.SizeParticipants
            };

            await _eventRepository.CreateEventAsync(newEvent);

            if (eventDTO.Tasks != null && eventDTO.Tasks.Count > 0)
            {
                foreach (var task in eventDTO.Tasks)
                {
                    var newTask = new Planify_BackEnd.Models.Task
                    {
                        EventId = newEvent.Id,
                        TaskName = task.TaskName,
                        TaskDescription = task.Description,
                        StartTime = task.StartTime ?? newEvent.StartTime,
                        Deadline = task.Deadline,
                        AmountBudget = task.Budget,
                        CreateBy = organizerId,
                        CreateDate = DateTime.UtcNow
                    };
                    await _taskRepository.CreateTaskAsync(newTask);

                    if (task.SubTasks != null && task.SubTasks.Count > 0)
                    {
                        foreach (var subTask in task.SubTasks)
                        {
                            var newSubTask = new SubTask
                            {
                                TaskId = newTask.Id,
                                SubTaskName = subTask.SubTaskName,
                                SubTaskDescription = subTask.Description,
                                StartTime = subTask.StartTime ?? newTask.StartTime,
                                Deadline = subTask.Deadline,
                                AmountBudget = subTask.Budget,
                                CreateBy = organizerId
                            };
                            await _subTaskRepository.CreateSubTaskAsync(newSubTask);
                        }
                    }
                }
            }

            if (eventDTO.Risks != null && eventDTO.Risks.Count > 0)
            {
                foreach (var risk in eventDTO.Risks)
                {
                    if (string.IsNullOrWhiteSpace(risk.Name))
                    {
                        return new ResponseDTO(400, "Risk name is required.", null);
                    }

                    var newRisk = new Risk
                    {
                        EventId = newEvent.Id,
                        Name = risk.Name,
                        Reason = risk.Reason,
                        Solution = risk.Solution,
                        Description = risk.Description
                    };
                    await _eventRepository.CreateRiskAsync(newRisk);
                }
            }

            return new ResponseDTO(201, "Event created successfully!", newEvent);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "An error occurred while creating the event.", ex.Message);
        }
    }

    public async Task<ResponseDTO> UploadImageAsync(UploadImageRequestDTO imageDTO)
    {
        if (imageDTO.EventMediaFiles != null && imageDTO.EventMediaFiles.Any())
        {
            foreach (var file in imageDTO.EventMediaFiles)
            {
                using var stream = file.OpenReadStream();
                string contentType = file.ContentType;
                string driveUrl;
                driveUrl = await _googleDriveService.UploadFileAsync(stream, file.FileName, contentType);
                if (string.IsNullOrEmpty(driveUrl))
                {
                    Console.WriteLine($"❌ Upload thất bại cho file {file.FileName}");
                    throw new Exception("Upload failed, MediaURL is null or empty.");
                }
                Console.WriteLine($"✅ File {file.FileName} đã upload thành công: {driveUrl}");
                var mediaItem = new Medium { MediaUrl = driveUrl };
                await _eventRepository.CreateMediaItemAsync(mediaItem);
              
                var eventMedia = new EventMedium
                {
                    EventId = imageDTO.EventId,
                    MediaId = mediaItem.Id,
                    Status = 1
                };
                await _eventRepository.AddEventMediaAsync(eventMedia);
            }
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
                UpdatedAt = DateTime.Now,
                MeasuringSuccess = e.MeasuringSuccess,
                Goals = e.Goals,
                MonitoringProcess = e.MonitoringProcess,
                SizeParticipants = e.SizeParticipants
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
                MeasuringSuccess = updatedEvent.MeasuringSuccess,
                Goals = updatedEvent.Goals,
                MonitoringProcess = updatedEvent.Goals,
                SizeParticipants = updatedEvent.SizeParticipants,
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
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> DeleteEventAsync(int eventId)
    {
        try
        {
            return await _eventRepository.DeleteEventAsync(eventId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PageResultDTO<EventGetListResponseDTO>> SearchEventAsync(int page, int pageSize, 
        string? title, DateTime? startTime, DateTime? endTime, decimal? minBudget, decimal? maxBudget, 
        int? isPublic, int? status, int? CategoryEventId, string? placed, Guid userId, int campusId)
    {
        try
        {
            var resultEvents = await _eventRepository.SearchEventAsync(page, pageSize, title, startTime, endTime,
                minBudget, maxBudget, isPublic, status, CategoryEventId, placed, userId, campusId);
            var eventDTOs = resultEvents.Items.Select(e => new EventGetListResponseDTO
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
                EventMedias = e.EventMedia.Select(em=> new EventMediumViewMediaModel
                {
                    Id=em.Id,
                    MediaId = em.MediaId,
                    MediaDTO = new MediaItemDTO
                    {
                        Id = em.Media.Id,
                        MediaUrl = em.Media.MediaUrl,
                    }
                }).ToList(),
                isFavorite = e.FavouriteEvents.Count != 0,
            }).ToList();

            return new PageResultDTO<EventGetListResponseDTO>(eventDTOs,resultEvents.TotalCount,page,pageSize);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ResponseDTO> CreateSaveDraft(EventCreateRequestDTO eventDTO, Guid organizerId)
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
                Status = 10,
                CampusId = int.Parse(campusIdClaim),
                CategoryEventId = eventDTO.CategoryEventId,
                Placed = eventDTO.Placed,
                CreateBy = organizerId,
                CreatedAt = DateTime.UtcNow
            };

            await _eventRepository.CreateSaveDraft(newEvent);

            if (eventDTO.Tasks != null && eventDTO.Tasks.Count > 0)
            {
                foreach (var task in eventDTO.Tasks)
                {
                    var newTask = new Planify_BackEnd.Models.Task
                    {
                        EventId = newEvent.Id,
                        TaskName = task.TaskName,
                        TaskDescription = task.Description,
                        StartTime = task.StartTime ?? newEvent.StartTime,
                        Deadline = task.Deadline,
                        AmountBudget = task.Budget,
                        CreateBy = organizerId,
                        CreateDate = DateTime.UtcNow

                    };
                    await _taskRepository.CreateTaskAsync(newTask);

                    if (task.SubTasks != null && task.SubTasks.Count > 0)
                    {
                        foreach (var subTask in task.SubTasks)
                        {
                            var newSubTask = new SubTask
                            {
                                TaskId = newTask.Id,
                                SubTaskName = subTask.SubTaskName,
                                SubTaskDescription = subTask.Description,
                                StartTime = subTask.StartTime ?? newTask.StartTime,
                                Deadline = subTask.Deadline,
                                AmountBudget = subTask.Budget,
                                CreateBy = organizerId,
                            };
                            await _subTaskRepository.CreateSubTaskAsync(newSubTask);
                        }
                    }
                }
            }

            return new ResponseDTO(201, "Save draft successfully!", newEvent);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while save draft event!", ex.Message);
        }
    }
    public async Task<ResponseDTO> UpdateSaveDraft(EventDTO eventDTO)
    {

        try
        {
            var campus = await _campusRepository.GetCampusByName(eventDTO.CampusName);
            //var category = await _categoryRepository.GetCategoryByName(eventDTO.CategoryEventName, campus.Id);
            Event updateEvent = new Event
            {
                Id = eventDTO.Id,
                AmountBudget = eventDTO.AmountBudget,
                CampusId = campus.Id,
                CategoryEventId = (int)eventDTO.CategoryEventId,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                EventDescription = eventDTO.EventDescription,
                EventTitle = eventDTO.EventTitle,
                IsPublic = eventDTO.IsPublic,
                Placed = eventDTO.Placed,
                Status = eventDTO.Status,
                TimePublic = eventDTO.TimePublic,
                UpdateBy = eventDTO.UpdateBy,
                UpdatedAt = DateTime.Now
            };
            Event updatedEvent = await _eventRepository.UpdateSaveDraft(updateEvent);
            var e = new Event
            {
                EventTitle = updatedEvent.EventTitle,
                EventDescription = updatedEvent.EventDescription,
                StartTime = updatedEvent.StartTime,
                EndTime = updatedEvent.EndTime,
                AmountBudget = updatedEvent.AmountBudget,
                IsPublic = updatedEvent.IsPublic,
                TimePublic = updatedEvent.TimePublic,
                Status = updatedEvent.Status,
                CampusId = updatedEvent.CampusId,
                CategoryEventId = updatedEvent.CategoryEventId,
                Placed = updatedEvent.Placed,
                CreateBy = updatedEvent.CreateBy,
                CreatedAt = updatedEvent.CreatedAt
            };

            return new ResponseDTO(201, "Save draft successfully!", updatedEvent);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while save draft event!", ex.Message);
        }
    }
    public async Task<ResponseDTO> GetSaveDraft(Guid createBy)
    {
        try
        {
            var eventDetail = await _eventRepository.GetSaveDraft(createBy);
            if (eventDetail == null)
            {
                return new ResponseDTO(404, "Don't exist any save draft!", eventDetail);
            }
            return new ResponseDTO(200, "Get save draft successfully!", eventDetail);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error orcurs while getting save draft!", ex.Message);
        }
    }
}

