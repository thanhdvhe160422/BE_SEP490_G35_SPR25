using Planify_BackEnd.Models;
﻿namespace Planify_BackEnd.Repositories.Groups
{
    public interface IGroupRepository
    {
        bool AllocateCostToGroup(int groupId, decimal cost);
        Task<Group> CreateGroupAsync(Group newGroup);
        System.Threading.Tasks.Task AddImplementerToGroupAsync(JoinGroup joinGroup);
    }
}
