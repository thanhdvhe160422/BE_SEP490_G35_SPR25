using Planify_BackEnd.Models;

public interface IUserRepository
{
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetListImplementer(int eventId, int page, int pageSize);
}