using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SubTasks;

namespace Planify_BackEnd.Services.SubTasks
{
    public interface ISubTaskService
    {
        Task<ResponseDTO> CreateSubTaskAsync(SubTaskCreateRequestDTO subTaskDTO, Guid implementerId);
    }
}
