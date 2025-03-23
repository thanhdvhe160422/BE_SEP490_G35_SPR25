using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Reports;

namespace Planify_BackEnd.Services.Reports
{
    public interface IReportService
    {
        Task<IEnumerable<ReportVM>> GetReportsByReceivedUser(Guid receivedUserId);
        Task<IEnumerable<ReportVM>> GetAllReportsAsync();
        Task<IEnumerable<ReportVM>> GetReportById(int reportId);
        Task<ReportCreateDTO> CreateReportAsync(ReportCreateDTO reportDTO);
        Task<ResponseDTO> UploadImageAsync(UploadReportImageRequestDTO imageDTO);
    }
}
