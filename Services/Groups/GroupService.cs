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

        public bool AllocateCostToGroup(int groupId, decimal cost)
        {
            try
            {
                _groupRepository.AllocateCostToGroup(groupId, cost);
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine("group service - allocate cost to group: "+ex.Message);
                return false;
            }
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
                try
                {
                    await _groupRepository.CreateGroupAsync(newGroup);
                }
                catch (Exception ex)
                {
                    return new ResponseDTO(500, "Error orcurs while creating group!", ex.Message);
                }

                return new ResponseDTO(201, "Group creates successfully!", newGroup);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error orcurs while creating group!", ex.Message);
            }
        }

        public bool AddLeadGroup(int GroupId, Guid ImplementerId)
        {
            try
            {
                return _groupRepository.AddLeadGroup(GroupId, ImplementerId);
            }catch
            {
                return false;
            }
        }

        public bool RemoveLeadGroup(int GroupId, Guid ImplementerId)
        {
            try
            {
                return _groupRepository.RemoveLeadGroup(GroupId, ImplementerId);
            }
            catch
            {
                return false;
            }
        }
    }
}
