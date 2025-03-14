using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Models;
using static Planify_BackEnd.DTOs.Events.EventDetailResponseDTO;

namespace Planify_BackEnd.Services.Users
{
    public class UserService : IUserservice
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserListDTO>> GetListImplementer(int eventId, int page, int pageSize)
        {
            var users = await _userRepository.GetListImplementer(eventId, page, pageSize);

            var userDTOs = users.Select(c => new UserListDTO
            {
                Id = c.Id,
                UserName = c.UserName,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Password = c.Password,
                DateOfBirth = c.DateOfBirth,
                PhoneNumber = c.PhoneNumber,
                AddressId = c.AddressId,
                AvatarId = c.AvatarId,
                CreatedAt = c.CreatedAt,
                CampusId = c.CampusId,
                Status = c.Status,
                Gender = c.Gender
            }).ToList();
            return userDTOs;
        }
    }
}
