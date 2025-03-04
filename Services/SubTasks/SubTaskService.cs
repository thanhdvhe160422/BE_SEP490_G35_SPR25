using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;

namespace Planify_BackEnd.Services.SubTasks
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SubTaskService(ISubTaskRepository subTaskRepository, IHttpContextAccessor httpContextAccessor)
        {
            _subTaskRepository = subTaskRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDTO> CreateSubTaskAsync(SubTaskCreateRequestDTO subTaskDTO, Guid implementerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(subTaskDTO.SubTaskName))
                {
                    return new ResponseDTO(400, "Sub-task name is required.", null);
                }

                if (subTaskDTO.StartTime >= subTaskDTO.Deadline)
                {
                    return new ResponseDTO(400, "Start time must be earlier than deadline.", null);
                }
                var newSubTask = new SubTask
                {
                    TaskId = subTaskDTO.TaskId,
                    SubTaskName = subTaskDTO.SubTaskName,
                    SubTaskDescription = subTaskDTO.SubTaskName,
                    StartTime = subTaskDTO.StartTime,
                    Deadline = subTaskDTO.Deadline,
                    AmountBudget = subTaskDTO.AmountBudget,
                    Status = 0,
                    CreateBy = implementerId,
               
                };

                await _subTaskRepository.CreateSubTaskAsync(newSubTask);

                return new ResponseDTO(201, "Sub-task creates successfully!", newSubTask);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error orcurs while creating sub-task!", ex.Message);
            }
        }
    }
}
