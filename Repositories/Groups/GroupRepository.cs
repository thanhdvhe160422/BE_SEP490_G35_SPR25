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
        public async Task<Group> CreateGroupAsync(Group newGroup)
        {
            try
            {
                await _context.Groups.AddAsync(newGroup);
                await _context.SaveChangesAsync();
                return newGroup;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }

        public async System.Threading.Tasks.Task AddImplementerToGroupAsync(JoinGroup joinGroup)
        {
            _context.JoinGroups.Add(joinGroup);
            await _context.SaveChangesAsync();
        }

    }
}
