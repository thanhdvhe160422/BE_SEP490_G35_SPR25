using Planify_BackEnd.DTOs;

namespace Planify_BackEnd.Services.JoinProjects
{
    public interface IJoinProjectService
    {
        ResponseDTO GetAllJoinedProjects(Guid userId, int page, int pageSize);
    }
}
