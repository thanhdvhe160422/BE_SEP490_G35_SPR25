using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Users;

namespace Planify_BackEnd.Services.Users
{
    public interface IUserservice
    {
        Task<IEnumerable<UserListDTO>> GetListUserAsync(int page, int pageSize);
        Task<UserDetailDTO> GetUserDetailAsync(Guid id);
        Task<ResponseDTO> UpdateUserStatusAsync(Guid id, int newStatus);
        Task<IEnumerable<UserListDTO>> GetListImplementer(int eventId, int page, int pageSize);
        Task<List<Models.User>> GetUserByNameOrEmailAsync(string input, int campusId);
        Task<UserListDTO> CreateEventOrganizer(UserDTO userDTO);
        Task<UserListDTO> UpdateEventOrganizer(UserDTO userDTO);
        Task<UserRoleDTO> AddUserRole(UserRoleDTO roleDTO);
        Task<ResponseDTO> CreateManagerAsync(UserCreateDTO user);
    }
}
