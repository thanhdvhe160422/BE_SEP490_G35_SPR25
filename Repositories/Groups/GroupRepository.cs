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

     
    }
}
