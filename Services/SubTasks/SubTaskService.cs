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
        /// <summary>
        /// Create a new sub-task
        /// </summary>
        /// <param name="subTaskDTO"></param>
        /// <param name="implementerId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
        /// <summary>
        /// Get sub-task by ID
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> GetSubTaskByIdAsync(int subTaskId)
        {
            try
            {
                var subTask = await _subTaskRepository.GetSubTaskByIdAsync(subTaskId);
                if (subTask == null)
                {
                    return new ResponseDTO(404, "Sub-task not found", null);
                }

                var subTaskResponse = new SubTaskResponseDTO
                {
                    Id = subTask.Id,
                    TaskId = subTask.TaskId,
                    SubTaskName = subTask.SubTaskName,
                    SubTaskDescription = subTask.SubTaskDescription,
                    StartTime = subTask.StartTime,
                    Deadline = subTask.Deadline,
                    AmountBudget = subTask.AmountBudget,
                    Status = subTask.Status,
                    CreateBy = subTask.CreateBy
                };

                return new ResponseDTO(200, "Sub-task retrieved successfully", subTaskResponse);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurred while retrieving sub-task", ex.Message);
            }
        }
        /// <summary>
        /// Update sub-task
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="subTaskDTO"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateSubTaskAsync(int subTaskId, SubTaskUpdateRequestDTO subTaskDTO)
        {
            try
            {
                var existingSubTask = await _subTaskRepository.GetSubTaskByIdAsync(subTaskId);
                if (existingSubTask == null)
                {
                    return new ResponseDTO(404, "Sub-task not found.", null);
                }

                existingSubTask.SubTaskName = subTaskDTO.SubTaskName;
                existingSubTask.SubTaskDescription = subTaskDTO.SubTaskDescription;
                existingSubTask.StartTime = subTaskDTO.StartTime;
                existingSubTask.Deadline = subTaskDTO.Deadline;
                existingSubTask.AmountBudget = subTaskDTO.AmountBudget;

                var updatedSubTask = await _subTaskRepository.UpdateSubTaskAsync(subTaskId, existingSubTask);

                return new ResponseDTO(200, "Sub-task updated successfully!", updatedSubTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while updating sub-task!", ex.Message);
            }
        }
        /// <summary>
        /// update sub-task status
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateSubTaskStatusAsync(int subTaskId, int newStatus)
        {
            try
            {
                var existingSubTask = await _subTaskRepository.GetSubTaskByIdAsync(subTaskId);
                if (existingSubTask == null)
                {
                    return new ResponseDTO(404, "Sub-task not found.", null);
                }

                existingSubTask.Status = newStatus;

                var updatedSubTask = await _subTaskRepository.UpdateSubTaskAsync(subTaskId, existingSubTask);

                return new ResponseDTO(200, "Sub-task status updated successfully!", updatedSubTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while updating sub-task status!", ex.Message);
            }
        }

        /// <summary>
        /// Delete sub-task
        /// </summary>
        /// <param name="subTaskId"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> DeleteSubTaskAsync(int subTaskId)
        {
            try
            {
                var isDeleted = await _subTaskRepository.DeleteSubTaskAsync(subTaskId);
                if (!isDeleted)
                {
                    return new ResponseDTO(404, "Sub-task not found.", null);
                }

                return new ResponseDTO(200, "Sub-task deleted successfully!", null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while deleting sub-task!", ex.Message);
            }
        }
        /// <summary>
        /// Get sub-tasks by task ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task<ResponseDTO> GetSubTasksByTaskIdAsync(int taskId)
        {
            try
            {
                var subTasks = await _subTaskRepository.GetSubTasksByTaskIdAsync(taskId);
                if (subTasks == null || !subTasks.Any())
                {
                    return new ResponseDTO(404, "No sub-tasks found for this task", null);
                }

                var subTaskResponses = subTasks.Select(subTask => new SubTaskResponseDTO
                {
                    Id = subTask.Id,
                    TaskId = subTask.TaskId,
                    SubTaskName = subTask.SubTaskName,
                    SubTaskDescription = subTask.SubTaskDescription,
                    StartTime = subTask.StartTime,
                    Deadline = subTask.Deadline,
                    AmountBudget = subTask.AmountBudget,
                    Status = subTask.Status,
                    CreateBy = subTask.CreateBy
                }).ToList();

                return new ResponseDTO(200, "Sub-tasks retrieved successfully", subTaskResponses);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurred while retrieving sub-tasks", ex.Message);
            }
        }

    }
}
