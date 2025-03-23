using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
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
                    .Where(r => r.SendTo == receivedUserId)
                    .ToList();
                return list;
            } catch (Exception ex)
            {
                return new List<Report>();
            }
        }
        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            try
            {
                var list = await _context.Reports
                    .Include(r => r.SendFromNavigation)
                    .Include(r => r.SendToNavigation)
                    .Include(r => r.Task)
                    .Include(r => r.ReportMedia).ThenInclude(rm => rm.Media)
                    .ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        public async Task<IEnumerable<Report>> GetReportById(int reportId)
        {
            try
            {
                var list = await _context.Reports
                    .Include(r => r.SendFromNavigation)
                    .Include(r => r.SendToNavigation)
                    .Include(r => r.Task)
                    .Include(r => r.ReportMedia).ThenInclude(rm => rm.Media)
                    .Where(r => r.Id == reportId)
                    .ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        public async Task<Report> CreateReportAsync(Report report)
        {
            try
            {
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return report;
            }
            catch (DbUpdateException dbEx)
            {
                // Lỗi liên quan đến EF hoặc database (ví dụ: ràng buộc, khóa ngoại...)
                Console.WriteLine($"Database update error: {dbEx.Message}");
                throw;
            }
            catch (SqlException sqlEx)
            {
                // Lỗi truy vấn SQL
                Console.WriteLine($"SQL error: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                // Lỗi tổng quát khác
                Console.WriteLine($"Unexpected error: {ex.Message}");
                throw;
            }
        }
        public async System.Threading.Tasks.Task CreateMediaItemAsync(Medium mediaItem)
        {
            _context.Media.Add(mediaItem);
            await _context.SaveChangesAsync();
        }
        public async System.Threading.Tasks.Task AddEventMediaAsync(ReportMedium reportMedia)
        {
            _context.ReportMedia.Add(reportMedia);
            await _context.SaveChangesAsync();
        }

    }
}
