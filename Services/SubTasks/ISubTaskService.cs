﻿using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SubTasks;

namespace Planify_BackEnd.Services.SubTasks
{
    public interface ISubTaskService
    {
        Task<ResponseDTO> CreateSubTaskAsync(SubTaskCreateRequestDTO subTaskDTO, Guid implementerId);
        bool UpdateActualSubTaskAmount(int subTaskId, decimal amount);
        Task<ResponseDTO> UpdateSubTaskAsync(int subTaskId, SubTaskUpdateRequestDTO subTaskDTO);
        Task<ResponseDTO> UpdateSubTaskStatusAsync(int subTaskId, int newStatus);
        Task<ResponseDTO> DeleteSubTaskAsync(int subTaskId);
        Task<ResponseDTO> GetSubTaskByIdAsync(int subTaskId);
        Task<ResponseDTO> GetSubTasksByTaskIdAsync(int taskId);
    }
}
