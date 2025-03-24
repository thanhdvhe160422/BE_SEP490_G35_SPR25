using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Reports
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsByReceivedUser(Guid receivedUserId);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task<IEnumerable<Report>> GetReportById(int reportId);
        Task<Report> CreateReportAsync(Report report);
        System.Threading.Tasks.Task CreateMediaItemAsync(Medium mediaItem);
        System.Threading.Tasks.Task AddEventMediaAsync(ReportMedium reportMedia);
    }
}
