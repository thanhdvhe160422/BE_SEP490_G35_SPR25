using Planify_BackEnd.DTOs.User;
using Planify_BackEnd.Repositories.User;

namespace Planify_BackEnd.Services.User
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }
        public ProfileViewModel getUserProfileById(Guid id)
        {
            try
            {
                Models.User user= _profileRepository.GetUserProfileById(id);
                ProfileViewModel profileViewModel = new ProfileViewModel
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    MediaItemDTO = new DTOs.Medias.MediaItemDTO
                    {
                        Id = user.AvatarNavigation.Id,
                        MediaUrl = user.AvatarNavigation.MediaUrl
                    },
                    CampusDTO = new DTOs.Campus.CampusDTO
                    {
                        Id = user.Campus.Id,
                        CampusName = user.Campus.CampusName,
                        Status = user.Campus.Status
                    },
                    CampusId = user.CampusId,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Password = user.Password,
                    Role = user.Role,
                    RoleNavigation = user.RoleNavigation,
                    Province = user.Province,
                    ProvinceId = user.ProvinceId,
                    District = user.District,
                    DistrictId = user.DistrictId,
                    Ward = user.Ward,
                    WardId = user.WardId
                };
                return profileViewModel;
            }catch(Exception ex)
            {
                Console.WriteLine("profile service - getUserProfileById: "+ex.Message);
                return new ProfileViewModel();
            }
        }
    }
}
