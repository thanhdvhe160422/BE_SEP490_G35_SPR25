
using Microsoft.EntityFrameworkCore;
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

        public List<Models.Task> SearchTaskOrderByStartDate(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            try
            {
                if(string.IsNullOrEmpty(name)) name ="";
                return _context.Tasks
                    .Where(e => e.TaskName.Contains(name.Trim()) && 
                           /*e.StartTime.HasValue && */e.StartTime >= startDate && 
                           /*e.Deadline.HasValue &&  */e.Deadline  <= endDate)
                    .OrderBy(e => e.StartTime)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        
        }
        public bool IsTaskExists(int taskId)
        {
            return _context.Tasks.Any(g => g.Id == taskId);
        }

        public bool UpdateActualTaskAmount(int taskId, decimal amount)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
                task.AmountBudget = amount;
                _context.Update(task);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
