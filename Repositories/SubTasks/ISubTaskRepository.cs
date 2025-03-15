using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories
{
    public interface ISubTaskRepository
    {
        Task<SubTask> CreateSubTaskAsync(SubTask newSubTask);
        bool UpdateActualSubTaskAmount(int subTaskId, decimal amount);
    }
}
