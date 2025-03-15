using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Reports
{
    public class ReportRepository : IReportRepository
    {
        private readonly PlanifyContext _context;
        public ReportRepository(PlanifyContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Report>> GetReportsByReceivedUser(Guid receivedUserId)
        {
            try
            {
                var list = _context.Reports
                    .Include(r => r.SendFromNavigation)
                    .Include(r => r.SendToNavigation)
                    .Include(r => r.Task)
                    .Include(r => r.ReportMedia).ThenInclude(rm => rm.Media)
                    .Where(r=>r.SendTo==receivedUserId)
                    .ToList();
                return list;
            }catch(Exception ex)
            {
                return new List<Report>();
            }
        }
    }
}
