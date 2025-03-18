using Planify_BackEnd.DTOs.Users;

namespace Planify_BackEnd.Services.Users
{
    public interface IUserservice
    {
        Task<IEnumerable<UserListDTO>> GetListImplementer(int eventId, int page, int pageSize);
        Task<List<Models.User>> GetUserByNameOrEmailAsync(string input, int campusId);
    }
}
