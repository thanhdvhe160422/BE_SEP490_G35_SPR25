﻿using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories
{
    public interface ISubTaskRepository
    {
        Task<SubTask> CreateSubTaskAsync(SubTask newSubTask);
        Task<SubTask?> UpdateSubTaskAsync(int subTaskId, SubTask updatedSubTask);
        Task<bool> DeleteSubTaskAsync(int subTaskId);
        Task<SubTask?> GetSubTaskByIdAsync(int subTaskId);
        Task<List<SubTask>> GetSubTasksByTaskIdAsync(int taskId);
    }
}
