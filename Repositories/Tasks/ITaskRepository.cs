using TaskModel = Planify_BackEnd.Models.Task;
namespace Planify_BackEnd.Repositories.Tasks
{
    public interface ITaskRepository
    {
        Task<TaskModel> CreateTaskAsync(TaskModel newTask);
        Task<List<Models.Task>> SearchTaskOrderByStartDateAsync(int page, int pageSize, string? name, DateTime startDate, DateTime endDate);

        Task<Models.Task?> UpdateTaskAsync(int taskId, Models.Task updatedTask);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<List<Models.Task>> GetAllTasksAsync();

        bool IsTaskExists(int taskId);
        bool UpdateActualTaskAmount(int taskId, decimal amount);
    }
}
