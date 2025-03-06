using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs.JoinedProjects;
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
        public async Task<IEnumerable<JoinedProjectDTO>> GetAllJoinedProjects(Guid userId, int page, int pageSize)
        {
            var joinedProject = await _joinProjectRepository.GetAllJoinedProjects(userId, page, pageSize);
            var joinedProjectDTOs = joinedProject.Select(j => new JoinedProjectDTO
            {
                Id = j.Id,
                EventId = j.EventId,
                UserId = j.UserId,
                Role = j.Role,
            }).ToList();
            return joinedProjectDTOs;
            
        }
    }

   
}
