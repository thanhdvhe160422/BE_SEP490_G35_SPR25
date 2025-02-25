using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;

public class UserRepository : IUserRepository
{
    private readonly PlanifyDbContext _context;

    public UserRepository(PlanifyDbContext context)
    {
        _context = context;
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.Include(r => r.RoleNavigation).Include(c => c.Campus)
                         .FirstOrDefault(u => u.Email == email && u.Status == 1);
    }

    public User GetUserById(Guid id)
    {
        return _context.Users.Include(r => r.RoleNavigation).Include(c => c.Campus)
            .FirstOrDefault(u => u.Id == id && u.Status == 1);
    }
}