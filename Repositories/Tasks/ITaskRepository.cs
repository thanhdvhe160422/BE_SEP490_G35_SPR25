using Planify_BackEnd.DTOs;
using TaskModel = Planify_BackEnd.Models.Task;
namespace Planify_BackEnd.Repositories.Tasks
{
    public interface ITaskRepository
    {
        Task<TaskModel> CreateTaskAsync(TaskModel newTask);
        Task<List<Models.Task>> SearchTaskOrderByStartDateAsync(int page, int pageSize, string? name, DateTime startDate, DateTime endDate);

        Task<Models.Task?> UpdateTaskAsync(int taskId, Models.Task updatedTask);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<List<Models.Task>> GetAllTasksAsync(int eventId);

        bool IsTaskExists(int taskId);
        bool UpdateActualTaskAmount(int taskId, decimal amount);
        public Models.Task GetTaskById(int taskId);
        public Task<bool> changeStatus(int taskId, int status);
        Task<PageResultDTO<Models.Task>> SearchTaskByImplementerId(int page, int pageSize, Guid implementerId, DateTime startDate, DateTime endDate);
        Task<bool> DeleteTaskV2(int taskId);

    }
}
