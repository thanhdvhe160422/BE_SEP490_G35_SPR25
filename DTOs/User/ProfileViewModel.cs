using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Medias;
using Planify_BackEnd.DTOs.Roles;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.DTOs.User
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? FirstName { get; set; } = null!;

        public string? LastName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string? PhoneNumber { get; set; } = null!;

        public int? WardId { get; set; }

        public int? DistrictId { get; set; }

        public int? ProvinceId { get; set; }

        public int? Avatar { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? Role { get; set; }

        public int? CampusId { get; set; }

        public RoleDTO? RoleNavigation { get; set; }
        public CampusDTO? CampusDTO { get; set; }
        public MediaItemDTO? MediaItemDTO{ get; set; }
        public District? District { get; set; }
        public Province? Province { get; set; }
        public Ward? Ward { get; set; }
    }
}
