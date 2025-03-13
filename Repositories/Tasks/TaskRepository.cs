
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

        public async Task<List<Models.Task>> SearchTaskOrderByStartDateAsync(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(name)) name = "";

                return await _context.Tasks
                    .Where(e => e.TaskName.Contains(name.Trim()) &&
                                e.StartTime >= startDate &&
                                e.Deadline <= endDate)
                    .OrderBy(e => e.StartTime)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(); 
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while searching tasks.", ex);
            }
        }

        public async Task<List<Models.Task>> GetAllTasksAsync()
        {
            try
            {
                return await _context.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving tasks.", ex);
            }
        }


        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(taskId);
                if (task == null)
                {
                    return false;
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
        public async Task<Models.Task?> UpdateTaskAsync(int taskId, Models.Task updatedTask)
        {
            try
            {
                var existingTask = await _context.Tasks.FindAsync(taskId);
                if (existingTask == null)
                {
                    return null;
                }

                existingTask.TaskName = updatedTask.TaskName;
                existingTask.TaskDescription = updatedTask.TaskDescription;
                existingTask.StartTime = updatedTask.StartTime;
                existingTask.Deadline = updatedTask.Deadline;
                existingTask.Status = updatedTask.Status;

                _context.Tasks.Update(existingTask);
                await _context.SaveChangesAsync();
                return existingTask;
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
    }
}
