using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs.Campus;
using Planify_BackEnd.Models;

    public class CampusRepository : ICampusRepository
{
        private readonly PlanifyContext _context;
        public CampusRepository(PlanifyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Campus>> getAllCampus()
        {
            return await _context.Campuses.ToListAsync();
        }
    }

