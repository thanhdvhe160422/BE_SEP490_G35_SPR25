using TaskModel = Planify_BackEnd.Models.Task;
namespace Planify_BackEnd.Repositories.Tasks
{
    public interface ITaskRepository
    {
        Task<TaskModel> CreateTaskAsync(TaskModel newTask);
        List<TaskModel> SearchTaskOrderByStartDate(int page, int pageSize, string? name, DateTime startDate, DateTime endDate);
        Task<Models.Task?> UpdateTaskAsync(int taskId, Models.Task updatedTask);
        Task<bool> DeleteTaskAsync(int taskId);
        bool IsTaskExists(int taskId);
    }
}
