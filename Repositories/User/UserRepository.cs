using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;

public class UserRepository : IUserRepository
{
    private readonly PlanifyContext _context;

    public UserRepository(PlanifyContext context)
    {
        _context = context;
    }

    public User GetUserByEmail(string email)
    {
        try
        {
            return _context.Users.Include(r => r.RoleNavigation).Include(c => c.Campus)
                                 .FirstOrDefault(u => u.Email == email && u.Status == 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserByEmail: {ex.Message}");
            return null;
        }
    }

    public User GetUserById(Guid id)
    {
        try
        {
            return _context.Users.Include(r => r.RoleNavigation).Include(c => c.Campus)
                                 .FirstOrDefault(u => u.Id == id && u.Status == 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserById: {ex.Message}");
            return null;
        }
    }
}