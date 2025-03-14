﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
       
    }
}
