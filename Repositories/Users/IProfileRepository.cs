

namespace Planify_BackEnd.Repositories.User
{
    public interface IProfileRepository
    {
        Models.User GetUserProfileById(Guid id);
    }
}
