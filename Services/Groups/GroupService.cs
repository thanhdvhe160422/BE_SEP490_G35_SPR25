using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Groups;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.Groups;

namespace Planify_BackEnd.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GroupService(IGroupRepository groupRepository, IHttpContextAccessor httpContextAccessor)
        {
            _groupRepository = groupRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDTO> CreateGroupAsync(GroupCreateRequestDTO groupDTO, Guid organizerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(groupDTO.GroupName))
                {
                    return new ResponseDTO(400, "Name of group is required.", null);
                }

                var newGroup = new Group
                {
                    GroupName = groupDTO.GroupName,
                    CreateBy = organizerId,
                    EventId = groupDTO.EventId,
                    AmountBudget = groupDTO.AmountBudget,
                };
              

                await _groupRepository.CreateGroupAsync(newGroup);

                return new ResponseDTO(201, "Group creates successfully!", newGroup);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error orcurs while creating group!", ex.Message);
            }
        }
    }
}
