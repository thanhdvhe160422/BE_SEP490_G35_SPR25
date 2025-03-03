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
    }
}
