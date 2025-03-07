using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.JoinedProjects;

namespace Planify_BackEnd.Services.JoinProjects
{
    public interface IJoinProjectService
    {
        Task<IEnumerable<JoinedProjectDTO>> GetAllJoinedProjects(Guid userId, int page, int pageSize);
    }
}
