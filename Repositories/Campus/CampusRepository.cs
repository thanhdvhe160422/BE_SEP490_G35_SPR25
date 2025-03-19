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
        try
        {
            return await _context.Campuses
                .Where(c => c.Status == 1)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);

        }
    }

    public async Task<Campus> GetCampusByName(string campusName)
    {
        try
        {
            var campus = await _context.Campuses
                .FirstOrDefaultAsync(c => c.CampusName.Contains(campusName)
                && c.Status == 1);
            return campus;
        }
        catch
        {
            return new Campus();
        }
    }
}
