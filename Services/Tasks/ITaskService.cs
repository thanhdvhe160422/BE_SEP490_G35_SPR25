using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Tasks;

namespace Planify_BackEnd.Services.Tasks
{
    public interface ITaskService
    {
        Task<ResponseDTO> CreateTaskAsync(TaskCreateRequestDTO taskDTO, Guid organizerId);
        bool UpdateActualTaskAmount(int taskId, decimal amount);
        Task<List<TaskSearchResponeDTO>> SearchTaskOrderByStartDateAsync(int page, int pageSize, string? name, DateTime startDate, DateTime endDate);
        Task<ResponseDTO> UpdateTaskAsync(int taskId, TaskUpdateRequestDTO taskDTO);
        Task<ResponseDTO> DeleteTaskAsync(int taskId);
        Task<List<TaskSearchResponeDTO>> GetAllTasksAsync(int groupId);
        public TaskDetailVM GetTaskById(int taskId);

    }
}
