using Planify_BackEnd.DTOs.User;

namespace Planify_BackEnd.Services.User
{
    public interface IProfileService
    {
        ProfileViewModel getUserProfileById(Guid id);
    }
}
