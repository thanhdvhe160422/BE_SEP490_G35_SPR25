﻿using Microsoft.EntityFrameworkCore;
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
                .Include(u=>u.AvatarNavigation)
                .Include(u=>u.Campus)
                .Include(u=>u.Province)
                .Include(u=>u.District)
                .Include(u=>u.Ward)
                .Include(u=>u.RoleNavigation)
                .FirstOrDefault(u => u.Id.Equals(id));
        }
    }
}
