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
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    /// <summary>
    /// get all event by campusId and paging
    /// </summary>
    /// <param name="campusId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public PageResultDTO<EventGetListResponseDTO> GetAllEvent(int campusId, int page, int pageSize)
    {
        try
        {
            PageResultDTO<Event> events = _eventRepository.GetAllEvent(campusId, page, pageSize);
            if (events.TotalCount == 0)

                return new PageResultDTO<EventGetListResponseDTO>(new List<EventGetListResponseDTO>(), 0, page, pageSize);
            List<EventGetListResponseDTO> eventList = new List<EventGetListResponseDTO>();
            foreach (var item in events.Items)
            {
                EventGetListResponseDTO eventDTO = new EventGetListResponseDTO
                {
                    Id = item.Id,
                    EventTitle = item.EventTitle,
                    EventDescription = item.EventDescription,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    AmountBudget = item.AmountBudget,
                    IsPublic = item.IsPublic,
                    TimePublic = item.TimePublic,
                    Status = item.Status,
                    CampusId = item.CampusId,
                    CategoryEventId = item.CategoryEventId,
                    Placed = item.Placed,
                    CreateBy = item.CreateBy,
                    CreatedAt = item.CreatedAt,
                    ManagerId = item.ManagerId,
                    MeasuringSuccess = item.MeasuringSuccess,
                    Goals = item.Goals,
                    MonitoringProcess = item.MonitoringProcess,
                    SizeParticipants = item.SizeParticipants,
                    EventMedias = item.EventMedia == null ? null : item.EventMedia.Select(em => new Planify_BackEnd.DTOs.Medias.EventMediumViewMediaModel
                    {
                        Id = em.Id,
                        EventId = em.Id,
                        MediaId = em.Id,
                        Status = em.Status,
                        MediaDTO = em.Media == null ? null : new Planify_BackEnd.DTOs.Medias.MediaItemDTO
                        {
                            Id = em.Media.Id,
                            MediaUrl = em.Media.MediaUrl
                        }
                    }).ToList()

                };
                eventList.Add(eventDTO);
                
            }
            return new PageResultDTO<EventGetListResponseDTO>(eventList, events.TotalCount, page, pageSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString()); 
            throw;
        }
    }

    public async Task<ResponseDTO> CreateEventAsync(EventCreateRequestDTO eventDTO, Guid organizerId)
    {
        using var transaction = await _eventRepository.BeginTransactionAsync();
        try
        {
            if (eventDTO == null)
                return new ResponseDTO(400, "Dữ liệu không hợp lệ", null);

            if (string.IsNullOrWhiteSpace(eventDTO.EventTitle))
                return new ResponseDTO(400, "Tên sự kiện là bắt buộc.", null);

            if (eventDTO.StartTime < DateTime.UtcNow.AddMonths(2))
                return new ResponseDTO(400, "Thời gian bắt đầu phải cách thời gian hiện tại ít nhất 2 tháng.", null);

            if (eventDTO.StartTime >= eventDTO.EndTime)
                return new ResponseDTO(400, "Thời gian bắt đầu phải sớm hơn thời gian kết thúc.", null);

            if (eventDTO.SizeParticipants < 0)
                return new ResponseDTO(400, "Số lượng người tham gia không thể âm.", null);

            var campusIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("campusId")?.Value;
            if (string.IsNullOrEmpty(campusIdClaim) || !int.TryParse(campusIdClaim, out int campusId))
                return new ResponseDTO(400, "ID campus không hợp lệ.", null);

            var category = await _eventRepository.GetCategoryEventAsync(eventDTO.CategoryEventId, campusId);
            if (category == null)
                return new ResponseDTO(400, "Danh mục không tồn tại hoặc không thuộc campus này.", null);

            decimal totalBudget = 0;
            if (eventDTO.CostBreakdowns != null && eventDTO.CostBreakdowns.Any())
            {
                foreach (var costBreakdown in eventDTO.CostBreakdowns)
                {
                    if (costBreakdown.Quantity.HasValue && costBreakdown.PriceByOne.HasValue)
                    {
                        totalBudget += costBreakdown.Quantity.Value * costBreakdown.PriceByOne.Value;
                    }
                }
            }

            // Tạo sự kiện mới
            var newEvent = new Event
            {
                EventTitle = eventDTO.EventTitle,
                EventDescription = eventDTO.EventDescription,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                AmountBudget = totalBudget,
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
                SizeParticipants = eventDTO.SizeParticipants,
                PromotionalPlan = eventDTO.PromotionalPlan,
                TargetAudience = eventDTO.TargetAudience,
                SloganEvent = eventDTO.SloganEvent
            };

            await _eventRepository.CreateEventAsync(newEvent);

            // Tạo các Task và SubTask
            if (eventDTO.Tasks != null && eventDTO.Tasks.Any())
            {
                foreach (var task in eventDTO.Tasks)
                {
                    if (string.IsNullOrWhiteSpace(task.TaskName))
                        return new ResponseDTO(400, "Tên task là bắt buộc.", null);

                    if (task.Deadline.HasValue && task.Deadline <= DateTime.UtcNow)
                        return new ResponseDTO(400, $"Hạn chót của task '{task.TaskName}' phải sau thời gian hiện tại.", null);

                    if (task.Budget < 0)
                        return new ResponseDTO(400, $"Ngân sách của task '{task.TaskName}' không thể âm.", null);

                    var newTask = new Planify_BackEnd.Models.Task
                    {
                        EventId = newEvent.Id,
                        TaskName = task.TaskName,
                        TaskDescription = task.Description,
                        StartTime = task.StartTime ?? newEvent.StartTime,
                        Deadline = task.Deadline,
                        AmountBudget = task.Budget,
                        CreateBy = organizerId,
                        CreateDate = DateTime.UtcNow,
                        Status = 1
                    };
                    await _taskRepository.CreateTaskAsync(newTask);

                    if (task.SubTasks != null && task.SubTasks.Any())
                    {
                        foreach (var subTask in task.SubTasks)
                        {
                            if (string.IsNullOrWhiteSpace(subTask.SubTaskName))
                                return new ResponseDTO(400, "Tên subtask là bắt buộc.", null);

                            if (subTask.StartTime.HasValue && subTask.Deadline.HasValue && subTask.StartTime >= subTask.Deadline)
                                return new ResponseDTO(400, $"Thời gian bắt đầu của subtask '{subTask.SubTaskName}' phải sớm hơn hạn chót.", null);

                            if (subTask.Budget < 0)
                                return new ResponseDTO(400, $"Ngân sách của subtask '{subTask.SubTaskName}' không thể âm.", null);

                            var newSubTask = new SubTask
                            {
                                TaskId = newTask.Id,
                                SubTaskName = subTask.SubTaskName,
                                SubTaskDescription = subTask.Description,
                                StartTime = subTask.StartTime ?? newTask.StartTime,
                                Deadline = subTask.Deadline,
                                AmountBudget = subTask.Budget,
                                CreateBy = organizerId,
                                Status = 1
                            };
                            await _subTaskRepository.CreateSubTaskAsync(newSubTask);
                        }
                    }
                }
            }

            // Tạo các rủi ro
            if (eventDTO.Risks != null && eventDTO.Risks.Any())
            {
                foreach (var risk in eventDTO.Risks)
                {
                    if (string.IsNullOrWhiteSpace(risk.Name))
                        return new ResponseDTO(400, "Tên rủi ro là bắt buộc.", null);

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
            Console.WriteLine("sang "+eventDTO.CostBreakdowns.Count());
            // Tạo các chi phí
            if (eventDTO.CostBreakdowns != null && eventDTO.CostBreakdowns.Any())
            {
                foreach (var costBreakdown in eventDTO.CostBreakdowns)
                {
                    if (string.IsNullOrWhiteSpace(costBreakdown.Name))
                        return new ResponseDTO(400, "Tên chi phí là bắt buộc.", null);

                    if (costBreakdown.Quantity.HasValue && costBreakdown.Quantity < 0)
                        return new ResponseDTO(400, $"Số lượng của chi phí '{costBreakdown.Name}' không thể âm.", null);

                    if (costBreakdown.PriceByOne.HasValue && costBreakdown.PriceByOne < 0)
                        return new ResponseDTO(400, $"Đơn giá của chi phí '{costBreakdown.Name}' không thể âm.", null);

                    var newCostBreakdown = new CostBreakdown
                    {
                        EventId = newEvent.Id,
                        Name = costBreakdown.Name,
                        Quantity = costBreakdown.Quantity,
                        PriceByOne = costBreakdown.PriceByOne
                    };
                    Console.WriteLine("thanh :"+newCostBreakdown.EventId);
                   await _eventRepository.CreateCostBreakdownAsync(newCostBreakdown);
                }
            }

            await transaction.CommitAsync();
            return new ResponseDTO(201, "Tạo sự kiện thành công!", newEvent);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Lỗi khi tạo sự kiện: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return new ResponseDTO(500, "Đã xảy ra lỗi khi tạo sự kiện.", ex.Message);
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

    public async Task<ResponseDTO> UpdateEventAsync(EventUpdateDTO e)
    {
        try
        {
            Event updateEvent = new Event
            {
                Id = e.Id,
                AmountBudget = e.AmountBudget,
                CampusId = e.CampusId,
                CategoryEventId = e.CategoryEventId,
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
                UpdatedAt = updatedEvent.UpdatedAt,
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
                UpdatedBy = new UserDto
                {
                    Id = updatedEvent.UpdateByNavigation.Id,
                    FirstName = updatedEvent.UpdateByNavigation.FirstName,
                    LastName = updatedEvent.UpdateByNavigation.LastName,
                    Email = updatedEvent.UpdateByNavigation.Email
                },
                EventMedia = updatedEvent.EventMedia.Select(em => new EventMediaDto
                {
                    Id = em.Id,
                    MediaUrl = em.Media.MediaUrl
                }).ToList()
            };
            return new ResponseDTO(200, "Update event successfully!", eventDetailResponseDTO);
        }
        catch (Exception ex)
        {
            return new ResponseDTO(500, "Error update event!", ex.Message);
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
        using var transaction = await _eventRepository.BeginTransactionAsync();
        try
        {
            if (eventDTO == null)
                return new ResponseDTO(400, "Dữ liệu không hợp lệ", null);

            if (string.IsNullOrWhiteSpace(eventDTO.EventTitle))
                return new ResponseDTO(400, "Tên sự kiện là bắt buộc.", null);

            if (eventDTO.StartTime < DateTime.UtcNow.AddMonths(2))
                return new ResponseDTO(400, "Thời gian bắt đầu phải cách thời gian hiện tại ít nhất 2 tháng.", null);

            if (eventDTO.StartTime >= eventDTO.EndTime)
                return new ResponseDTO(400, "Thời gian bắt đầu phải sớm hơn thời gian kết thúc.", null);

            if (eventDTO.SizeParticipants < 0)
                return new ResponseDTO(400, "Số lượng người tham gia không thể âm.", null);

            var campusIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("campusId")?.Value;
            if (string.IsNullOrEmpty(campusIdClaim) || !int.TryParse(campusIdClaim, out int campusId))
                return new ResponseDTO(400, "ID campus không hợp lệ.", null);

            var category = await _eventRepository.GetCategoryEventAsync(eventDTO.CategoryEventId, campusId);
            if (category == null)
                return new ResponseDTO(400, "Danh mục không tồn tại hoặc không thuộc campus này.", null);

            decimal totalBudget = 0;
            if (eventDTO.CostBreakdowns != null && eventDTO.CostBreakdowns.Any())
            {
                foreach (var costBreakdown in eventDTO.CostBreakdowns)
                {
                    if (costBreakdown.Quantity.HasValue && costBreakdown.PriceByOne.HasValue)
                    {
                        totalBudget += costBreakdown.Quantity.Value * costBreakdown.PriceByOne.Value;
                    }
                }
            }

            var newEvent = new Event
            {
                EventTitle = eventDTO.EventTitle,
                EventDescription = eventDTO.EventDescription,
                StartTime = eventDTO.StartTime,
                EndTime = eventDTO.EndTime,
                AmountBudget = totalBudget,
                IsPublic = 0,
                TimePublic = null,
                Status = 0, // Bản nháp
                CampusId = campusId,
                CategoryEventId = eventDTO.CategoryEventId,
                Placed = eventDTO.Placed,
                CreateBy = organizerId,
                CreatedAt = DateTime.UtcNow,
                MeasuringSuccess = eventDTO.MeasuringSuccess,
                Goals = eventDTO.Goals,
                MonitoringProcess = eventDTO.MonitoringProcess,
                SizeParticipants = eventDTO.SizeParticipants,
                PromotionalPlan = eventDTO.PromotionalPlan,
                TargetAudience = eventDTO.TargetAudience,
                SloganEvent = eventDTO.SloganEvent
            };

            // Lưu sự kiện
            await _eventRepository.CreateEventAsync(newEvent);

            // Tạo Tasks và SubTasks
            if (eventDTO.Tasks != null && eventDTO.Tasks.Any())
            {
                foreach (var task in eventDTO.Tasks)
                {
                    if (string.IsNullOrWhiteSpace(task.TaskName))
                        return new ResponseDTO(400, "Tên task là bắt buộc.", null);

                    if (task.Deadline > DateTime.UtcNow)
                        return new ResponseDTO(400, $"Deadline of task '{task.TaskName}' must be after now.", null);

                    if (task.Budget < 0)
                        return new ResponseDTO(400, $"Ngân sách của task '{task.TaskName}' không thể âm.", null);

                    var newTask = new Planify_BackEnd.Models.Task
                    {
                        EventId = newEvent.Id,
                        TaskName = task.TaskName,
                        TaskDescription = task.Description,
                        StartTime = task.StartTime ?? newEvent.StartTime,
                        Deadline = task.Deadline,
                        AmountBudget = task.Budget,
                        CreateBy = organizerId,
                        CreateDate = DateTime.UtcNow,
                        Status = 1
                    };
                    await _taskRepository.CreateTaskAsync(newTask);

                    if (task.SubTasks != null && task.SubTasks.Any())
                    {
                        foreach (var subTask in task.SubTasks)
                        {
                            if (string.IsNullOrWhiteSpace(subTask.SubTaskName))
                                return new ResponseDTO(400, "Tên subtask là bắt buộc.", null);

                            if (subTask.StartTime.HasValue && subTask.Deadline.HasValue && subTask.StartTime >= subTask.Deadline)
                                return new ResponseDTO(400, $"Thời gian bắt đầu của subtask '{subTask.SubTaskName}' phải sớm hơn thời hạn.", null);

                            if (subTask.Budget < 0)
                                return new ResponseDTO(400, $"Ngân sách của subtask '{subTask.SubTaskName}' không thể âm.", null);

                            var newSubTask = new SubTask
                            {
                                TaskId = newTask.Id,
                                SubTaskName = subTask.SubTaskName,
                                SubTaskDescription = subTask.Description,
                                StartTime = subTask.StartTime ?? newTask.StartTime,
                                Deadline = subTask.Deadline,
                                AmountBudget = subTask.Budget,
                                CreateBy = organizerId,
                                Status = 1
                            };
                            await _subTaskRepository.CreateSubTaskAsync(newSubTask);
                        }
                    }
                }
            }

            // Tạo Risks
            if (eventDTO.Risks != null && eventDTO.Risks.Any())
            {
                foreach (var risk in eventDTO.Risks)
                {
                    if (string.IsNullOrWhiteSpace(risk.Name))
                        return new ResponseDTO(400, "Tên rủi ro là bắt buộc.", null);

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

            // Tạo CostBreakdowns
            if (eventDTO.CostBreakdowns != null && eventDTO.CostBreakdowns.Any())
        {
                foreach (var costBreakdown in eventDTO.CostBreakdowns)
                {
                    if (string.IsNullOrWhiteSpace(costBreakdown.Name))
                        return new ResponseDTO(400, "Tên chi phí là bắt buộc.", null);

                    if (costBreakdown.Quantity.HasValue && costBreakdown.Quantity < 0)
                        return new ResponseDTO(400, $"Số lượng của chi phí '{costBreakdown.Name}' không thể âm.", null);

                    if (costBreakdown.PriceByOne.HasValue && costBreakdown.PriceByOne < 0)
                        return new ResponseDTO(400, $"Đơn giá của chi phí '{costBreakdown.Name}' không thể âm.", null);

                    var newCostBreakdown = new CostBreakdown
                    {
                        EventId = newEvent.Id,
                        Name = costBreakdown.Name,
                        Quantity = costBreakdown.Quantity,
                        PriceByOne = costBreakdown.PriceByOne
                    };
                    await _eventRepository.CreateCostBreakdownAsync(newCostBreakdown);
                }
            }

            await transaction.CommitAsync();
            return new ResponseDTO(201, "Lưu bản nháp thành công!", newEvent);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Lỗi khi lưu bản nháp: {ex.Message}\nStackTrace: {ex.StackTrace}");
            return new ResponseDTO(500, "Đã xảy ra lỗi khi lưu bản nháp.", ex.Message);
        }
    }
    //public async Task<ResponseDTO> UpdateSaveDraft(EventUpdateDTO eventDTO)
    //{

    //    try
    //    {
    //        var campus = await _campusRepository.GetCampusByName(eventDTO.CampusName);
    //        //var category = await _categoryRepository.GetCategoryByName(eventDTO.CategoryEventName, campus.Id);
    //        Event updateEvent = new Event
    //        {
    //            Id = eventDTO.Id,
    //            AmountBudget = eventDTO.AmountBudget,
    //            CampusId = campus.Id,
    //            CategoryEventId = (int)eventDTO.CategoryEventId,
    //            StartTime = eventDTO.StartTime,
    //            EndTime = eventDTO.EndTime,
    //            EventDescription = eventDTO.EventDescription,
    //            EventTitle = eventDTO.EventTitle,
    //            IsPublic = eventDTO.IsPublic,
    //            Placed = eventDTO.Placed,
    //            Status = eventDTO.Status,
    //            TimePublic = eventDTO.TimePublic,
    //            UpdateBy = eventDTO.UpdateBy,
    //            UpdatedAt = DateTime.Now
    //        };
    //        Event updatedEvent = await _eventRepository.UpdateSaveDraft(updateEvent);
    //        var e = new Event
    //        {
    //            EventTitle = updatedEvent.EventTitle,
    //            EventDescription = updatedEvent.EventDescription,
    //            StartTime = updatedEvent.StartTime,
    //            EndTime = updatedEvent.EndTime,
    //            AmountBudget = updatedEvent.AmountBudget,
    //            IsPublic = updatedEvent.IsPublic,
    //            TimePublic = updatedEvent.TimePublic,
    //            Status = updatedEvent.Status,
    //            CampusId = updatedEvent.CampusId,
    //            CategoryEventId = updatedEvent.CategoryEventId,
    //            Placed = updatedEvent.Placed,
    //            CreateBy = updatedEvent.CreateBy,
    //            CreatedAt = updatedEvent.CreatedAt
    //        };

    //        return new ResponseDTO(201, "Save draft successfully!", updatedEvent);
    //    }
    //    catch (Exception ex)
    //    {
    //        return new ResponseDTO(500, "Error orcurs while save draft event!", ex.Message);
    //    }
    //}
    //public async Task<ResponseDTO> GetSaveDraft(Guid createBy)
    //{
    //    try
    //    {
    //        var eventDetail = await _eventRepository.GetSaveDraft(createBy);
    //        if (eventDetail == null)
    //        {
    //            return new ResponseDTO(404, "Don't exist any save draft!", eventDetail);
    //        }
    //        return new ResponseDTO(200, "Get save draft successfully!", eventDetail);
    //    }
    //    catch (Exception ex)
    //    {
    //        return new ResponseDTO(500, "Error orcurs while getting save draft!", ex.Message);
    //    }
    //}
}

