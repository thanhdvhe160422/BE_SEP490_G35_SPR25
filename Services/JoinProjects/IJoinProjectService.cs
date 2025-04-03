using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.JoinedProjects;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.JoinProjects
{
    public interface IJoinProjectService
    {
        Task<IEnumerable<JoinedProjectDTO>> GetAllJoinedProjects(Guid userId, int page, int pageSize);
        Task<bool> DeleteImplementerFromEvent(Guid userId, int eventId);
        Task<ResponseDTO> AddImplementersToEventAsync(AddImplementersToEventDTO request);
        Task<PageResultDTO<JoinedProjectVM>> GetImplementerJoinedEvent(int page, int pageSize, int eventId);
    }
}
