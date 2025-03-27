using Planify_BackEnd.Models;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetListUserAsync(int page, int pageSize);
    Task<User> GetUserDetailAsync(Guid userId);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetListImplementer(int eventId, int page, int pageSize);
    Task<List<User>> GetUserByNameOrEmail(string input, int campusId);
    Task<User> CreateEventOrganizer(User user);
    Task<User> UpdateEventOrganizer(User user);
    Task<UserRole> AddUserRole(UserRole role);
}