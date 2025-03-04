
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly PlanifyContext _context;
        public TaskRepository(PlanifyContext context)
        {
            _context = context;
        }

        public async Task<Models.Task> CreateTaskAsync(Models.Task newTask)
        {
            try
            {
                await _context.Tasks.AddAsync(newTask);
                await _context.SaveChangesAsync();
                return newTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}
