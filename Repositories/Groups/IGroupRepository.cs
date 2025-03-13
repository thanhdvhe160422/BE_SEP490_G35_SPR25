using Planify_BackEnd.Models;
﻿namespace Planify_BackEnd.Repositories.Groups
{
    public interface IGroupRepository
    {
        bool AllocateCostToGroup(int groupId, decimal cost);
        Task<Group> CreateGroupAsync(Group newGroup);
        bool IsGroupExists(int groupId);
        System.Threading.Tasks.Task AddImplementerToGroupAsync(JoinGroup joinGroup);
        //Because there is no leadgroup column, leader will have status -1
        bool AddLeadGroup(int GroupId, Guid ImplementerId);
        bool RemoveLeadGroup(int GroupId, Guid ImplementerId);
        Task<Group> UpdateGroupAsync(Group group);
        Task<bool> DeleteGroupAsync(int GroupId);
        Task<Group> GetGroupByIdAsync(int GroupId);
    }
}
