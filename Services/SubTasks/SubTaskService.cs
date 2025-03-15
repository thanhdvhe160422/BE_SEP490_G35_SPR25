using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.Tasks;

namespace Planify_BackEnd.Services.SubTasks
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SubTaskService(ISubTaskRepository subTaskRepository, IHttpContextAccessor httpContextAccessor, ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
            _subTaskRepository = subTaskRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDTO> CreateSubTaskAsync(SubTaskCreateRequestDTO subTaskDTO, Guid implementerId)
        {
            try
            {
                if (!_taskRepository.IsTaskExists(subTaskDTO.TaskId))
                {
                    throw new ArgumentException("Task does not exists.");
                }
                if (string.IsNullOrWhiteSpace(subTaskDTO.SubTaskName))
                {
                    return new ResponseDTO(400, "Sub-task name is required.", null);
                }

                if (subTaskDTO.StartTime >= subTaskDTO.Deadline)
                {
                    return new ResponseDTO(400, "Start time must be earlier than deadline.", null);
                }
                var newSubTask = new SubTask
                {
                    TaskId = subTaskDTO.TaskId,
                    SubTaskName = subTaskDTO.SubTaskName,
                    SubTaskDescription = subTaskDTO.SubTaskDescription,
                    StartTime = subTaskDTO.StartTime,
                    Deadline = subTaskDTO.Deadline,
                    AmountBudget = subTaskDTO.AmountBudget,
                    Status = 0,
                    CreateBy = implementerId,
               
                };

                try
                {
                    await _subTaskRepository.CreateSubTaskAsync(newSubTask);
                }
                catch (Exception dbEx)
                {
                    return new ResponseDTO(500, "Database error while creating sub-task!", dbEx.Message);
                }

                return new ResponseDTO(201, "Sub-task creates successfully!", newSubTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error orcurs while creating sub-task!", ex.Message);
            }
        }

        public bool UpdateActualSubTaskAmount(int subTaskId, decimal amount)
        {
            try
            {
                return _subTaskRepository.UpdateActualSubTaskAmount(subTaskId, amount);
            }catch(Exception ex)
            {
                Console.WriteLine("subtask service - update actual subtask amount: " + ex.Message);
                return false;
            }
        }
    }
}
