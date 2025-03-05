using Planify_BackEnd.DTOs;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.JoinGroups;

namespace Planify_BackEnd.Services.JoinProjects
{
    public class JoinProjectService : IJoinProjectService
    {
        private readonly IJoinProjectRepository _joinProjectRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public JoinProjectService(IJoinProjectRepository joinProjectRepository, IHttpContextAccessor httpContextAccessor)
        {
            _joinProjectRepository = joinProjectRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public ResponseDTO GetAllJoinedProjects(Guid userId, int page, int pageSize)
        {
            var joinedProject = _joinProjectRepository.GetAllJoinedProjects(userId, page, pageSize);
            return new ResponseDTO(200, "Joined project retrieved successfully", joinedProject);
        }
    }

   
}
