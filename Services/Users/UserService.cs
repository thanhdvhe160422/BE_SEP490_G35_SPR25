using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Roles;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Models;

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

        public async Task<List<Models.User>> GetUserByNameOrEmailAsync(string input, int campusId)
        {
            return await _userRepository.GetUserByNameOrEmail(input, campusId);
        }
        public async Task<UserRoleDTO> AddUserRole(UserRoleDTO roleDTO)
        {
            try
            {
                UserRole role = new UserRole
                {
                    Id = roleDTO.Id,
                    RoleId = roleDTO.RoleId,
                    UserId = roleDTO.UserId,
                };
                var r = await _userRepository.AddUserRole(role);
                roleDTO.Id = r.Id;
                roleDTO.UserId = r.UserId;
                roleDTO.RoleId = r.RoleId;
                return roleDTO;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserListDTO> CreateEventOrganizer(UserDTO userDTO)
        {
            try
            {
                Models.User user = new Models.User
                {
                    Id = Guid.NewGuid(),
                    CampusId = userDTO.CampusId,
                    DateOfBirth = userDTO.DateOfBirth,
                    Email = userDTO.Email,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    PhoneNumber = userDTO.PhoneNumber,
                    Gender = userDTO.Gender,
                    Status = 1,
                    CreatedAt = DateTime.Now,
                };
                var u = await _userRepository.CreateEventOrganizer(user);
                UserListDTO userListDTO = new UserListDTO
                {
                    Id = u.Id,
                    AddressId = u.AddressId,
                    AvatarId = u.AvatarId,
                    CampusId = u.CampusId,
                    CreatedAt = u.CreatedAt,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    Status = u.Status,
                    UserName = u.UserName,
                    Password = u.Password,
                };
                return userListDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserListDTO> UpdateEventOrganizer(UserDTO userDTO)
        {
            try
            {
                Models.User user = new Models.User
                {
                    Id = userDTO.Id,
                    CampusId = userDTO.CampusId,
                    DateOfBirth = userDTO.DateOfBirth,
                    Email = userDTO.Email,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    PhoneNumber = userDTO.PhoneNumber,
                    Gender = userDTO.Gender,
                };
                var u = await _userRepository.UpdateEventOrganizer(user);
                UserListDTO userListDTO = new UserListDTO
                {
                    Id = u.Id,
                    AddressId = u.AddressId,
                    AvatarId = u.AvatarId,
                    CampusId = u.CampusId,
                    CreatedAt = u.CreatedAt,
                    DateOfBirth = u.DateOfBirth,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Gender = u.Gender,
                    PhoneNumber = u.PhoneNumber,
                    Status = u.Status,
                    UserName = u.UserName,
                    Password = u.Password,
                };
                return userListDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
