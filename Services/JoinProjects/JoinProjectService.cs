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
                TimeJoinProject = j.TimeJoinProject,
                TimeOutProject = j.TimeOutProject,
                EventTitle = j.Event.EventTitle,
                EventDescription = j.Event.EventDescription,
                StartTime = j.Event.StartTime,
                EndTime = j.Event.EndTime,
                AmountBudget = j.Event.AmountBudget,
                IsPublic = j.Event.IsPublic,
                TimePublic = j.Event.TimePublic,
                Status = j.Event.Status,
                CampusId = j.Event.CampusId,
                CategoryEventId = j.Event.CategoryEventId,
                Placed = j.Event.Placed,
                CreatedAt = j.Event.CreatedAt,
                CreateBy = j.Event.CreateBy,
                UpdatedAt = j.Event.UpdatedAt,
                UpdateBy = j.Event.UpdateBy
            }).ToList();
            return joinedProjectDTOs;
            
        }
        public async Task<bool> DeleteImplementerFromEvent(Guid userId, int eventId)
        {
            try
            {
                return await _joinProjectRepository.DeleteImplementerFromEvent(userId, eventId);
            }catch (Exception ex)
            {
                Console.WriteLine("join project service - delete implementer from event: " + ex.Message);
                return false;
            }
        }

    }

   
}
