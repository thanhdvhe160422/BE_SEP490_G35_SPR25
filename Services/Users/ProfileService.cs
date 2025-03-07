using Microsoft.AspNetCore.Http.HttpResults;
using Planify_BackEnd.DTOs.Medias;
using Planify_BackEnd.DTOs.User;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Repositories.User;
using System.IdentityModel.Tokens.Jwt;

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
                    AvatarId = user.AvatarId,
                    Avatar = user.Avatar==null? null: new MediumDTO
                    {
                        Id = user.Avatar.Id,
                        MediaUrl = user.Avatar.MediaUrl
                    },
                    CampusDTO = user.Campus == null ? null : new DTOs.Campus.CampusDTO
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
                    AddressId = user.AddressId,
                    AddressVM = user.Address==null? null: new DTOs.Andress.AddressVM
                    {
                        Id=user.Address.Id,
                        AddressDetail = user.Address.AddressDetail,
                        WardId = user.Address.WardId,
                        WardVM = new DTOs.Andress.WardVM
                        {
                            WardName = user.Address.Ward.WardName,
                            DistrictVM = new DTOs.Andress.DistrictVM
                            {
                                DistrictName = user.Address.Ward.District.DistrictName,
                                ProvinceVM = new DTOs.Andress.ProvinceVM
                                {
                                    ProvinceName = user.Address.Ward.District.Province.ProvinceName
                                }
                            },

                        }
                    },
                    UserRoleDTO = user.UserRoles==null?new List<UserRoleDTO>():user.UserRoles.Select(ur=>new UserRoleDTO
                    {
                        Id = ur.Id,
                        RoleId = ur.RoleId,
                        UserId = ur.UserId,
                        RoleDTO = new DTOs.Roles.RoleDTO
                        {
                            Id = ur.Role.Id,
                            RoleName = ur.Role.RoleName,
                        }
                    }).ToList(),
                    Status = user.Status
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
