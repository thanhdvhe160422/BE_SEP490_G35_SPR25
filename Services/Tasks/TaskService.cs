using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.DTOs.Tasks;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Groups;
using Planify_BackEnd.Repositories.Tasks;
using TaskModel = Planify_BackEnd.Models.Task;
using Planify_BackEnd.DTOs.Groups;
namespace Planify_BackEnd.Services.Tasks
{
    public class TaskService : ITaskService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TaskService(ITaskRepository taskRepository, IHttpContextAccessor httpContextAccessor, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _taskRepository = taskRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDTO> CreateTaskAsync(TaskCreateRequestDTO taskDTO, Guid organizerId)
        {
            try
            {
                if (!_groupRepository.IsGroupExists(taskDTO.GroupId))
                {
                    throw new ArgumentException("Group does not exists.");
                }
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
                try
                {
                    await _taskRepository.CreateTaskAsync(newTask);
                }
                catch (Exception dbEx)
                {
                    return new ResponseDTO(500, "Database error while creating task!", dbEx.Message);
                }


                return new ResponseDTO(201, "Task creates successfully!", newTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error orcurs while creating task!", ex.Message);
            }
        }
        public async Task<List<TaskSearchResponeDTO>> SearchTaskOrderByStartDateAsync(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            try
            {
                var tasks = await _taskRepository.SearchTaskOrderByStartDateAsync(page, pageSize, name, startDate, endDate);

                return tasks.Select(item => new TaskSearchResponeDTO
                {
                    Id = item.Id,
                    TaskName = item.TaskName,
                    TaskDescription = item.TaskDescription,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    GroupId = item.GroupId,
                    AmountBudget = item.AmountBudget,
                    Progress = item.Progress,
                    Status = item.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("task - SearchTaskOrderByStartDateAsync: " + ex.Message);
                return new List<TaskSearchResponeDTO>();
            }
        }

        public async Task<List<TaskSearchResponeDTO>> GetAllTasksAsync(int groupId)
        {
            try
            {
                var tasks = await _taskRepository.GetAllTasksAsync(groupId);
                return tasks.Select(item => new TaskSearchResponeDTO
                {
                    Id = item.Id,
                    TaskName = item.TaskName,
                    TaskDescription = item.TaskDescription,
                    StartTime = item.StartTime,
                    Deadline = item.Deadline,
                    GroupId = item.GroupId,
                    AmountBudget = item.AmountBudget,
                    Progress = item.Progress,
                    Status = item.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("task - GetAllTasksAsync: " + ex.Message);
                return new List<TaskSearchResponeDTO>();
            }
        }


        public async Task<ResponseDTO> UpdateTaskAsync(int taskId, TaskUpdateRequestDTO taskDTO)
        {
            try
            {
                var existingTask = await _taskRepository.UpdateTaskAsync(taskId, new TaskModel
                {
                    TaskName = taskDTO.TaskName,
                    TaskDescription = taskDTO.TaskDescription,
                    StartTime = taskDTO.StartTime,
                    Deadline = taskDTO.Deadline,
                    AmountBudget = taskDTO.AmountBudget,
                    //GroupId = taskDTO.GroupId,
                    Progress = taskDTO.Progress,
                    Status = taskDTO.Status
                });

                if (existingTask == null)
                {
                    return new ResponseDTO(404, "Task not found.", null);
                }

                return new ResponseDTO(200, "Task updated successfully!", existingTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while updating task!", ex.Message);
            }
        }

        public async Task<ResponseDTO> DeleteTaskAsync(int taskId)
        {
            try
            {
                var isDeleted = await _taskRepository.DeleteTaskAsync(taskId);
                if (!isDeleted)
                {
                    return new ResponseDTO(404, "Task not found or already deleted.", null);
                }

                return new ResponseDTO(200, "Task deleted successfully!", null);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while deleting task!", ex.Message);
            }
        }



        public bool UpdateActualTaskAmount(int taskId, decimal amount)
        {
            try
            {
                return _taskRepository.UpdateActualTaskAmount(taskId, amount);
            }catch(Exception ex)
            {
                Console.WriteLine("task - update actual task amount" + ex.Message);
                return false;
            }
        }

        public TaskDetailVM GetTaskById(int taskId)
        {
            try
            {
                var task = _taskRepository.GetTaskById(taskId);
                TaskDetailVM taskDetailVM = new TaskDetailVM
                {
                    Id = task.Id,
                    TaskName = task.TaskName,
                    TaskDescription = task.TaskDescription,
                    AmountBudget = task.AmountBudget,
                    CreateBy = task.CreateBy,
                    CreateByNavigation = task.CreateByNavigation == null ? new DTOs.Users.UserNameVM() : new DTOs.Users.UserNameVM
                    {
                        Id = task.CreateByNavigation.Id,
                        Email = task.CreateByNavigation.Email,
                        FirstName = task.CreateByNavigation.FirstName,
                        LastName = task.CreateByNavigation.LastName
                    },
                    CreateDate = task.CreateDate,
                    Deadline = task.Deadline,
                    GroupId = task.GroupId,
                    Group = task.Group == null ? new GroupNameVM() : new GroupNameVM
                    {
                        Id = task.Group.Id,
                        GroupName = task.Group.GroupName
                    },
                    Progress = task.Progress,
                    StartTime = task.StartTime,
                    Status = task.Status,
                    SubTasks = task.SubTasks == null ? new List<SubTaskVM>() : task.SubTasks.Select(st => new SubTaskVM
                    {
                        Id = st.Id,
                        SubTaskName = st.SubTaskName,
                        SubTaskDescription = st.SubTaskDescription,
                        AmountBudget = st.AmountBudget,
                        StartTime = st.StartTime,
                        Deadline = st.Deadline,
                        Status = st.Status,
                        TaskId = st.TaskId,
                        CreateBy = st.CreateBy,
                        CreateByNavigation = st.CreateByNavigation == null ? null : new DTOs.Users.UserNameVM
                        {
                            Id = st.CreateByNavigation.Id,
                            Email = st.CreateByNavigation.Email,
                            FirstName = st.CreateByNavigation.FirstName,
                            LastName = st.CreateByNavigation.LastName
                        }
                    }).ToList(),
                };
                return taskDetailVM;
            }
            catch
            {
                return new TaskDetailVM();
            }
        }
    }
}
