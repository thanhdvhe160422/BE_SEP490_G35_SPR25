using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Reports
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetReportsByReceivedUser(Guid receivedUserId);

    }
}
