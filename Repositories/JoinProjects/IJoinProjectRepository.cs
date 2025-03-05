using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.JoinGroups
{
    public interface IJoinProjectRepository
    {

        public List<JoinProject> GetAllJoinedProjects(Guid userId, int page, int pageSize);
    }
}
