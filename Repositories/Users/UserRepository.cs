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

    public async Task<List<User>> GetUserByNameOrEmail(string input, int campusId)
    {
        try
        {
            return await _context.Users
            .Where(c => (c.FirstName.Contains(input) || c.LastName.Contains(input) || c.Email.Contains(input))
                        && c.CampusId == campusId
                        && c.UserRoles.Any(ur => ur.RoleId == 4 || ur.RoleId == 5))
            .Include(c => c.Campus)
            .Include(c => c.UserRoles)
                .ThenInclude(ur => ur.Role)
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> CreateEventOrganizer(User user)
    {
        try
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> UpdateEventOrganizer(User user)
    {
        try
        {
            var updateUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            updateUser.CampusId = user.CampusId;
            updateUser.DateOfBirth = user.DateOfBirth;
            updateUser.Email = user.Email;
            updateUser.FirstName = user.FirstName;
            updateUser.LastName = user.LastName;
            updateUser.PhoneNumber = user.PhoneNumber;
            updateUser.Gender = user.Gender;
            _context.Update(updateUser);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserRole> AddUserRole(UserRole role)
    {
        try
        {
            _context.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}