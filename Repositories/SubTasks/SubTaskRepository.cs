
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;


public class SubTaskRepository : ISubTaskRepository
    {
        private readonly PlanifyContext _context;
    public SubTaskRepository(PlanifyContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Create a new subtask
    /// </summary>
    /// <param name="newSubTask"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async  Task<SubTask> CreateSubTaskAsync(SubTask newSubTask)
    {
        try
        {
            await _context.SubTasks.AddAsync(newSubTask);
            await _context.SaveChangesAsync();
            return newSubTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }
    /// <summary>
    /// Update a subtask
    /// </summary>
    /// <param name="subTaskId"></param>
    /// <param name="updatedSubTask"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<SubTask?> UpdateSubTaskAsync(int subTaskId, SubTask updatedSubTask)
    {
        try
        {
            var existingSubTask = await _context.SubTasks.FindAsync(subTaskId);
            if (existingSubTask == null)
            {
                return null; 
            }
            existingSubTask.SubTaskName = updatedSubTask.SubTaskName;
            existingSubTask.SubTaskDescription = updatedSubTask.SubTaskDescription;
            existingSubTask.Status = updatedSubTask.Status;
            existingSubTask.StartTime = updatedSubTask.StartTime;
            existingSubTask.Deadline = updatedSubTask.Deadline;
            existingSubTask.AmountBudget = updatedSubTask.AmountBudget;
            _context.SubTasks.Update(existingSubTask);
            await _context.SaveChangesAsync();

            return existingSubTask;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while updating the subtask.", ex);
        }
    }
    /// <summary>
    /// Delete a subtask
    /// </summary>
    /// <param name="subTaskId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> DeleteSubTaskAsync(int subTaskId)
    {
        try
        {
            var subTask = await _context.SubTasks.FindAsync(subTaskId);
            if (subTask == null)
            {
                return false;
            }

            _context.SubTasks.Remove(subTask);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while deleting the subtask.", ex);
        }
    }
    /// <summary>
    /// Get a subtask by ID
    /// </summary>
    /// <param name="subTaskId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<SubTask?> GetSubTaskByIdAsync(int subTaskId)
    {
        try
        {
            return await _context.SubTasks.FindAsync(subTaskId);
        }
        catch (Exception ex)
        {
            throw new Exception("Error while retrieving sub-task.", ex);
        }
    }
    /// <summary>
    /// Get subtasks by task ID
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<List<SubTask>> GetSubTasksByTaskIdAsync(int taskId)
    {
        try
        {
            return await _context.SubTasks
                .Where(st => st.TaskId == taskId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving sub-tasks by taskId.", ex);
        }
    }


}


