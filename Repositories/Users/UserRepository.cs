using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Medias;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Models;

public class UserRepository : IUserRepository
{
    private readonly PlanifyContext _context;

    public UserRepository(PlanifyContext context)
    {
        _context = context;
    }
    public async Task<User> UpdateManagerAsync(Guid id, UserUpdateDTO updateUser)
    {
        try
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.AddressId = updateUser.AddressId;
            existingUser.FirstName = updateUser.FirstName;
            existingUser.LastName = updateUser.LastName;
            existingUser.UserName = updateUser.UserName;
            existingUser.Status = updateUser.Status;
            existingUser.DateOfBirth = updateUser.DateOfBirth;
            existingUser.Gender = updateUser.Gender;
            existingUser.CampusId = updateUser.CampusId;
            existingUser.Email = updateUser.Email;
            existingUser.PhoneNumber = updateUser.PhoneNumber;
            existingUser.AvatarId = updateUser.AvatarId;
            existingUser.Password = updateUser.Password;
            _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
                return existingUser;
            }
            catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred.", ex);
        }
    }
    public async Task<User> CreateManagerAsync(User user)
    {
        try
        {
            await _context.UserRoles.AddAsync(new UserRole
            {
                UserId = user.Id,
                RoleId = 2
            });
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public PageResultDTO<User> GetListUser(int page, int pageSize)
    {
        try
        {
            var count = _context.Users.Count();
            if (count == 0)
            {
                return new PageResultDTO<User>(new List<User>(), count, page, pageSize);
            }
            var data = _context.Users
                .Include(r => r.UserRoles)
                .ThenInclude(u => u.Role)
                .Include(c => c.Campus)
                .Include(a => a.Address)
                .Include(a=>a.Avatar)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();
            return new PageResultDTO<User>(data, count, page, pageSize);

        } catch (Exception ex)
        {
            Console.WriteLine($"Error in GetListUser: {ex.Message}");
            return null;
        }
    }
    public async Task<User> GetUserDetailAsync(Guid userId)
    {
        try
        {
            return await _context.Users
                .Include(r => r.UserRoles)
                .ThenInclude(u => u.Role)
                .Include(c => c.Campus)
                .Include(a => a.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GetUserDetailAsync: {ex.Message}");
            return null;
        }
    }
    public async Task<bool> UpdateUserStatusAsync(Guid id, int newStatus)
    {
        try
        {
            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return false; // Không tìm thấy User
            }

            existingUser.Status = newStatus;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return true; // Cập nhật thành công
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while updating the user status.", ex);
        }
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        try
        {
            var user = await _context.Users
                .Include(r => r.UserRoles)
                .ThenInclude(u => u.Role)
                .Include(c => c.Campus)
                .FirstOrDefaultAsync(u => u.Email == email && u.Status == 1);

            if (user != null && user.UserRoles != null && user.UserRoles.Any())
            {
                user.UserRoles = user.UserRoles
                    .OrderBy(ur => ur.Role.Id)
                    .Take(1)
                    .ToList();
            }

            return user;
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
    public  PageResultDTO<User> GetListImplementer(int eventId, int page, int pageSize)
    {
        try
        {
            var count = _context.JoinProjects
                .Include(jp => jp.User)
                .Include(jp => jp.Event)
                .Where(jg => jg.EventId == eventId)
                .Select(jg => jg.User)
                .Distinct()
                .Count();
            if (count == 0)
                return new PageResultDTO<User>(new List<User>(), count, page, pageSize);
            var data = _context.JoinProjects
                .Include(jp => jp.User)
                .Include(jp => jp.Event)
                .Where(jg => jg.EventId == eventId)
                .Select(jg => jg.User)
                .Distinct()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            PageResultDTO<User> result = new PageResultDTO<User>(data, count, page, pageSize);
            return result;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PageResultDTO<User>> GetUserByNameOrEmail(string input, int campusId)
    {
        try
        {
            var count = _context.Users
            .Where(c => (c.FirstName.Contains(input) || c.LastName.Contains(input) || c.Email.Contains(input))
                        && c.CampusId == campusId
                        && c.UserRoles.Any(ur => ur.RoleId == 4 || ur.RoleId == 5))
            .Include(c => c.Campus)
            .Include(c => c.UserRoles)
                .ThenInclude(ur => ur.Role)
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .Count();
            if (count == 0) return new PageResultDTO<User>(new List<User>(), 0, 0, 0);
            var result = await _context.Users
            .Where(c => (c.FirstName.Contains(input) || c.LastName.Contains(input) || c.Email.Contains(input))
                        && c.CampusId == campusId
                        && c.UserRoles.Any(ur => ur.RoleId == 4 || ur.RoleId == 5))
            .Include(c => c.Campus)
            .Include(c => c.UserRoles)
                .ThenInclude(ur => ur.Role)
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .ToListAsync();
            return new PageResultDTO<User>(result, count, 0, 0);
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

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
            .Include(u => u.Campus)
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<PageResultDTO<EventOrganizerVM>> GetEventOrganizer(int page, int pageSize, int campusId)
    {
        try
        {
            var count = _context.UserRoles
                .Include(ur => ur.User)
                .Where(ur => ur.RoleId == 3&&ur.User.CampusId==campusId)
                .Count();
            var result = await _context.UserRoles
                .Include(ur => ur.User).ThenInclude(u => u.Avatar)
                .Where(ur => ur.RoleId == 3 && ur.User.CampusId == campusId)
                .Select(ur => new EventOrganizerVM
                {
                    Id = ur.User.Id,
                    DateOfBirth = ur.User.DateOfBirth,
                    Email = ur.User.Email,
                    FirstName = ur.User.FirstName,
                    LastName = ur.User.LastName,
                    Gender = ur.User.Gender,
                    PhoneNumber = ur.User.PhoneNumber,
                    Avatar = ur.User.Avatar==null?
                    new MediumDTO():
                    new MediumDTO
                    {
                        Id = ur.User.Avatar.Id,
                        MediaUrl = ur.User.Avatar.MediaUrl
                    }
                }).ToListAsync();
            return new PageResultDTO<EventOrganizerVM>(result, count, page, pageSize);
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UpdateRoleEOG(Guid userId, int roleId)
    {
        try
        {
            var role = _context.UserRoles.FirstOrDefault(u => u.UserId.Equals(userId) && u.RoleId == 3);
            if (role == null) return false;
            role.RoleId = roleId;
            _context.Update(role);
            await _context.SaveChangesAsync();
            return true;
        }catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<bool> UpdateRoleCampusManager(Guid userId, int roleId)
    {
        try
        {
            var role = _context.UserRoles.FirstOrDefault(u => u.UserId.Equals(userId) && u.RoleId == 2);
            if (role == null) return false;
            role.RoleId = roleId;
            _context.Update(role);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PageResultDTO<EventOrganizerVM>> GetCampusManager(int page, int pageSize, int campusId)
    {
        try
        {
            var count = _context.UserRoles
                .Include(ur => ur.User)
                .Where(ur => ur.RoleId == 2 && ur.User.CampusId == campusId)
                .Count();
            var result = await _context.UserRoles
                .Include(ur => ur.User).ThenInclude(u => u.Avatar)
                .Where(ur => ur.RoleId == 2 && ur.User.CampusId == campusId)
                .Select(ur => new EventOrganizerVM
                {
                    Id = ur.User.Id,
                    DateOfBirth = ur.User.DateOfBirth,
                    Email = ur.User.Email,
                    FirstName = ur.User.FirstName,
                    LastName = ur.User.LastName,
                    Gender = ur.User.Gender,
                    PhoneNumber = ur.User.PhoneNumber,
                    Avatar = ur.User.Avatar == null ?
                    new MediumDTO() :
                    new MediumDTO
                    {
                        Id = ur.User.Avatar.Id,
                        MediaUrl = ur.User.Avatar.MediaUrl
                    }
                }).ToListAsync();
            return new PageResultDTO<EventOrganizerVM>(result, count, page, pageSize);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PageResultDTO<User>> SearchUser(int page, int pageSize, string input, int campusId)
    {
        try
        {
            var count = _context.Users
            .Where(c => (c.FirstName.Contains(input) || c.LastName.Contains(input) || c.Email.Contains(input))
                        && c.CampusId == campusId
                        && c.UserRoles.Any(ur => ur.RoleId != 1))
            .Include(c => c.Campus)
            .Include(c => c.UserRoles)
                .ThenInclude(ur => ur.Role)
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .Count();
            if (count == 0) return new PageResultDTO<User>(new List<User>(), 0, page, pageSize);
            var result = await _context.Users
            .Where(c => (c.FirstName.Contains(input) || c.LastName.Contains(input) || c.Email.Contains(input))
                        && c.CampusId == campusId
                        && c.UserRoles.Any(ur => ur.RoleId != 1))
            .Include(c => c.Campus)
            .Include(c => c.UserRoles)
                .ThenInclude(ur => ur.Role)
            .GroupBy(c => c.Id)
            .Select(g => g.First())
            .Skip((page - 1) * pageSize).Take(pageSize)
            .ToListAsync();
            return new PageResultDTO<User>(result, count, page, pageSize);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}