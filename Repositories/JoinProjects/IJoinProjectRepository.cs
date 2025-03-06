using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.JoinGroups
{
    public interface IJoinProjectRepository
    {
        Task<IEnumerable<JoinProject>> GetAllJoinedProjects(Guid userId, int page, int pageSize);
    }
}
