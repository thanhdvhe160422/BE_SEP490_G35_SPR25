using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Groups
{
    public interface IGroupRepository
    {
        public Task<Group> CreateGroupAsync(Group newGroup);
    }
}
