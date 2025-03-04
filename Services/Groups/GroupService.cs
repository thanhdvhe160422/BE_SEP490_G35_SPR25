using Planify_BackEnd.Repositories.Groups;

namespace Planify_BackEnd.Services.Groups
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
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
    }
}
