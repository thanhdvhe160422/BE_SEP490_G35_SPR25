using Planify_BackEnd.DTOs.Reports;

namespace Planify_BackEnd.Services.Reports
{
    public interface IReportService
    {
        Task<IEnumerable<ReportVM>> GetReportsByReceivedUser(Guid receivedUserId);
    }
}
