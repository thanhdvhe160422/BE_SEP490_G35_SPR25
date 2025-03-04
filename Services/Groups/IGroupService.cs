namespace Planify_BackEnd.Services.Groups
{
    public interface IGroupService
    {
        bool AllocateCostToGroup(int groupId, decimal cost);
    }
}
