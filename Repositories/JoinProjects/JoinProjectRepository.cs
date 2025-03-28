using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.JoinGroups
{
    public class JoinProjectRepository : IJoinProjectRepository
    {
        private readonly PlanifyContext _context;

        public JoinProjectRepository(PlanifyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JoinProject>> GetAllJoinedProjects(Guid userId, int page, int pageSize)
        {
            try
            {
                return await _context.JoinProjects
                    .Include(jp => jp.Event)
                    .Where(jp => jp.UserId == userId)
                    .Skip((page - 1) * pageSize).Take(pageSize)
                    .ToListAsync(); 
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteImplementerFromEvent(Guid userId, int eventId)
        {
            try
            {
                var joinProject = _context.JoinProjects.FirstOrDefault(jp => jp.UserId == userId && jp.EventId == eventId);
                joinProject.TimeOutProject = DateTime.Now;
                _context.JoinProjects.Update(joinProject);
                await _context.SaveChangesAsync();
                return true;
            }catch
            {
                return false;
            }
        }

        public async Task<bool> AddImplementersToProject(List<Guid> implementerIds, int eventId)
        {
            if (implementerIds == null || !implementerIds.Any())
            {
                return false;
            }

            try
            {
                var joinProjects = implementerIds.Select(implementerId => new JoinProject
                {
                    UserId = implementerId,
                    EventId = eventId,
                    TimeJoinProject = DateTime.UtcNow,
                }).ToList();

                _context.JoinProjects.AddRange(joinProjects);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddImplementerToProject(Guid implementerId, int eventId)
        {
            if (implementerId == Guid.Empty)
            {
                return false;
            }

            try
            {
                var joinProjects =  new JoinProject
                {
                    UserId = implementerId,
                    EventId = eventId,
                    TimeJoinProject = DateTime.UtcNow,
                };

                _context.JoinProjects.AddRange(joinProjects);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddRoleImplementer(Guid implementerId)
        {
            if (implementerId == Guid.Empty)
            {
                return false;
            }

            try
            {
                var userRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == implementerId && ur.RoleId == 4);

                if (userRole == null)
                {
                    _context.UserRoles.Add(new UserRole
                    {
                        UserId = implementerId,
                        RoleId = 4
                    });
                }
                else
                {
                    return true;
                }

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddRoleImplementers(List<Guid> implementerIds)
        {
            if (implementerIds == null || !implementerIds.Any())
            {
                return false;
            }

            try
            {
                foreach (var implementerId in implementerIds)
                {
                    var userRole = await _context.UserRoles
                    .FirstOrDefaultAsync(ur => ur.UserId == implementerId && ur.RoleId == 4);

                    if (userRole == null)
                    {
                        _context.UserRoles.Add(new UserRole
                        {
                            UserId = implementerId,
                            RoleId = 4
                        });
                    }
                }
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> EventExistsAsync(int eventId)
        {
            return await _context.Events.AnyAsync(e => e.Id == eventId);
        }

        public async Task<List<Guid>> GetInvalidUserIdsAsync(List<Guid> userIds)
        {
            return userIds.Where(id => !_context.Users.Any(u => u.Id == id)).ToList();
        }

        public async Task<List<Guid>> GetExistingImplementerIdsAsync(int eventId)
        {
            return await _context.JoinProjects
                .Where(jp => jp.EventId == eventId)
                .Select(jp => jp.UserId)
                .ToListAsync();
        }
    }
}
