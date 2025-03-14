﻿using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;

public class UserRepository : IUserRepository
{
    private readonly PlanifyContext _context;

    public UserRepository(PlanifyContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        try
        {
            return await _context.Users
                .Include(r => r.UserRoles)
                .ThenInclude(u => u.Role)
                .Include(c => c.Campus)
                .FirstOrDefaultAsync(u => u.Email == email && u.Status == 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserByEmailAsync: {ex.Message}");
            return null;
        }
    }
    public async Task<User> GetUserByIdAsync(Guid id)
    {
        try
        {
            return await _context.Users
                //.Include(r => r.RoleNavigation)
                .Include(c => c.Campus)
                .FirstOrDefaultAsync(u => u.Id == id && u.Status == 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserByIdAsync: {ex.Message}");
            return null;
        }
    }
    /// <summary>
    /// get list implementer of a project by EventId
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<IEnumerable<User>> GetListImplementer(int eventId, int page, int pageSize)
    {
        try
        {
            return await _context.JoinProjects
                      .Where(jg => jg.EventId == eventId)
                      .Select(jg => jg.User)
                      .Distinct()
                      .Skip((page - 1) * pageSize).Take(pageSize)
                      .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
}