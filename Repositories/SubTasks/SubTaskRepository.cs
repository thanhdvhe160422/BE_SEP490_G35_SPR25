
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;


public class SubTaskRepository : ISubTaskRepository
    {
        private readonly PlanifyContext _context;
    public SubTaskRepository(PlanifyContext context)
    {
        _context = context;
    }

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
}


