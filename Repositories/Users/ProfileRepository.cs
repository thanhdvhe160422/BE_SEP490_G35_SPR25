using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs.User;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.User
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly PlanifyContext _context;
        public ProfileRepository(PlanifyContext context)
        {
            _context = context;
        }
        public Models.User GetUserProfileById(Guid id)
        {
            return _context.Users
                .Include(u=>u.Avatar)
                .Include(u=>u.Campus)
                .Include(u=>u.Address).ThenInclude(a=>a.Ward).ThenInclude(w=>w.District).ThenInclude(d=>d.Province)
                .Include(u=>u.UserRoles).ThenInclude(ur=>ur.Role)
                .FirstOrDefault(u => u.Id.Equals(id));
        }
    }
}
