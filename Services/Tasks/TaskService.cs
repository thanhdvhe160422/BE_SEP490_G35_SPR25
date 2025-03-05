using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Tasks;
using Planify_BackEnd.Repositories.Tasks;
using TaskModel = Planify_BackEnd.Models.Task;
namespace Planify_BackEnd.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskService(ITaskRepository taskRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDTO> CreateTaskAsync(TaskCreateRequestDTO taskDTO, Guid organizerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(taskDTO.TaskName))
                {
                    return new ResponseDTO(400, "Task name is required.", null);
                }

                if (taskDTO.StartTime >= taskDTO.Deadline)
                {
                    return new ResponseDTO(400, "Start time must be earlier than deadline.", null);
                }
                var newTask = new TaskModel
                {  
                    TaskName = taskDTO.TaskName,
                    TaskDescription = taskDTO.TaskDescription,
                    StartTime = taskDTO.StartTime,
                    Deadline = taskDTO.Deadline,
                    AmountBudget = taskDTO.AmountBudget,
                    GroupId = taskDTO.GroupId,
                    Progress = taskDTO.Progress,
                    Status = 0,
                    CreateBy = organizerId,
                    CreateDate = DateTime.UtcNow
                };
               

                await _taskRepository.CreateTaskAsync(newTask);

                return new ResponseDTO(201, "Task creates successfully!", newTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error orcurs while creating sub-task!", ex.Message);
            }
        }
        public List<TaskSearchResponeDTO> SearchTaskOrderByStartDate(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            try
            {
                List<TaskModel> tasks = _taskRepository.SearchTaskOrderByStartDate(page, pageSize, name, startDate, endDate);

                return tasks.Select(item => new TaskSearchResponeDTO
                {
                    Id = item.Id,
                    TaskName = item.TaskName,
                    TaskDescription = item.TaskDescription,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    AmountBudget = item.AmountBudget,
                    Progress = item.Progress,
                    Status = item.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("task - searchTask: " + ex.Message);
                return new List<TaskSearchResponeDTO>();
            }
        }

    }
}
