namespace Planify_BackEnd.Repositories.Groups
{
    public interface IGroupRepository
    {
        bool AllocateCostToGroup(int groupId, decimal cost);
    }
}
