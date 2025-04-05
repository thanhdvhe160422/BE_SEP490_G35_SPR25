using Azure.Core;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.SignalR;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.Hub;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.Tasks;
using System.Threading.Tasks;

namespace Planify_BackEnd.Services.SubTasks
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<NotificationHub> _hubContext;
        public SubTaskService(ISubTaskRepository subTaskRepository, IHttpContextAccessor httpContextAccessor, ITaskRepository taskRepository, IHubContext<NotificationHub> hubContext)
        {
            _taskRepository = taskRepository;
            _subTaskRepository = subTaskRepository;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
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
                //notification
                var listJoinUserId = await _subTaskRepository.GetJoinedIdBySubTaskIdAsync(subTaskId);
                var eventId = await _subTaskRepository.GetEventIdBySubtaskId(subTaskId);
                var subtask = await _subTaskRepository.GetSubTaskByIdAsync(subTaskId);
                var message = "Subtask " + subtask.SubTaskName + " has been deleted!";
                var link = "/event-detail-EOG/" + eventId;
                foreach (var id in listJoinUserId)
                {
                    await _hubContext.Clients.User(id + "").SendAsync("ReceiveNotification",
                        message,
                        link);
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

        public async Task<bool> AssignSubTask(Guid assignUserId, Guid userId, int subtaskId)
        {
            try
            {
                var subtask = await _subTaskRepository.GetSubTaskByIdAsync(subtaskId);
                if (subtask == null) throw new Exception("Not found subtask!");
                var task = _taskRepository.GetTaskById(subtask.TaskId);
                if (!task.CreateBy.Equals(assignUserId)) throw new Exception("Assign user must be create user!");
                JoinTask newJoinTask = new JoinTask
                {
                    UserId = userId,
                    TaskId = subtaskId,
                    CreatedAt = DateTime.Now
                };
                var response = await _subTaskRepository.AssignSubTask(newJoinTask);
                //notification
                if (response)
                    await _hubContext.Clients.User(userId + "").SendAsync("ReceiveNotification",
                        "You has been assign to subtask "+ subtask.SubTaskName+ "!",
                        "/event-detail-EOG/" + task.EventId);
                return response;

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DeleteAssignedUser(Guid userId, int subTaskId)
        {
            try
            {
                return await _subTaskRepository.DeleteAssignedUser(userId, subTaskId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<PageResultDTO<SubTaskResponseDTO>> SearchSubTaskByImplementerId(Guid implementerId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var resultTasks = await _subTaskRepository.SearchSubTaskByImplementerId(implementerId, startDate, endDate);
                var tastDtos = resultTasks.Items.Select(item => new SubTaskResponseDTO
                {
                    Id = item.Id,
                    SubTaskName = item.SubTaskName,
                    SubTaskDescription = item.SubTaskDescription,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    TaskId = item.TaskId,
                    AmountBudget = item.AmountBudget,
                    Status = item.Status,
                    CreateBy = item.CreateBy
                }).ToList();
                return new PageResultDTO<SubTaskResponseDTO>(tastDtos, resultTasks.TotalCount, 0, 0);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
