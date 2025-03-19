using Planify_BackEnd.DTOs.User;
using Planify_BackEnd.DTOs.Users;

namespace Planify_BackEnd.Services.User
{
    public interface IProfileService
    {
        ProfileViewModel getUserProfileById(Guid id);
        ProfileUpdateModel UpdateProfile(ProfileUpdateModel updateProfile);
        bool UpdateAvatar(Guid id, int avatarId);
    }
}
