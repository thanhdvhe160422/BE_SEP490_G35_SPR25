﻿using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.DTOs.Roles;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;

namespace Planify_BackEnd.Services.Users
{
    public class UserService : IUserservice
    {
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<ResponseDTO> UpdateManagerAsync(UserUpdateDTO user, Guid id)
        {
            try
            {
                var updateUser = await _userRepository.UpdateManagerAsync( id, user);

                if (updateUser == null)
                {
                    return new ResponseDTO(400, "User not found!", null);
                }
                return new ResponseDTO(200, "User updated successfully!", updateUser);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while updating user!", ex.Message);
            }
        }
        public async Task<ResponseDTO> CreateManagerAsync(UserCreateDTO user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.FirstName))
                {
                    return new ResponseDTO(400, "First name is required.", null);
                }
                var newUser = new Models.User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    DateOfBirth = user.DateOfBirth,
                    PhoneNumber = user.PhoneNumber,
                    AddressId = user.AddressId,
                    AvatarId = user.AvatarId,
                    CreatedAt = user.CreatedAt,
                    CampusId = user.CampusId,
                    Status = 1,
                    Gender = user.Gender,
                };
                try
                {
                    await _userRepository.CreateManagerAsync(newUser);
                }
                catch (Exception dbEx)
                {
                    return new ResponseDTO(500, "Database error while creating user!", dbEx.Message);
                }
                return new ResponseDTO(201, "Campus Manager creates successfully!", newUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<UserListDTO>> GetListUserAsync(int page, int pageSize)
        {
            var users = await _userRepository.GetListUserAsync(page, pageSize);

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
        public async Task<UserDetailDTO> GetUserDetailAsync(Guid id)
        {
            var c = await _userRepository.GetUserDetailAsync(id);
            if (c == null)
            {
                return null; 
            }

            var userDTO = new UserDetailDTO
            {
                Id = c.Id,
                UserName = c.UserName,
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Password = c.Password,
                DateOfBirth = c.DateOfBirth,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address.AddressDetail,
                AvatarId = c.AvatarId,
                CreatedAt = c.CreatedAt,
                CampusName = c.Campus.CampusName,
                Status = c.Status,
                Gender = c.Gender ? "Male" : "Female"
              
            };

            return userDTO;
        }
        public async Task<ResponseDTO> UpdateUserStatusAsync(Guid id, int newStatus)
        {
            try
            {
                var updateUser = await _userRepository.UpdateUserStatusAsync(id, newStatus);

                return new ResponseDTO(200, "User status updated successfully!", updateUser);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Error occurs while updating user atus!", ex.Message);
            }
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
