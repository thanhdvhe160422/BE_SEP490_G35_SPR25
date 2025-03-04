using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Tasks;

namespace Planify_BackEnd.Services.Tasks
{
    public interface ITaskService
    {
        Task<ResponseDTO> CreateTaskAsync(TaskCreateRequestDTO taskDTO, Guid organizerId);
    }
}
