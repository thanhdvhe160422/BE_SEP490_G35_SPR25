using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Groups
{
    public class GroupRepository : IGroupRepository
    {
        private readonly PlanifyContext _context;
        public GroupRepository(PlanifyContext context)
        {
            _context = context;
        }
        public bool AllocateCostToGroup(int groupId, decimal cost)
        {
            try
            {
                var group = _context.Groups.FirstOrDefault(g => g.Id == groupId);
                group.AmountBudget = cost;
                _context.Groups.Update(group);
                _context.SaveChanges();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
