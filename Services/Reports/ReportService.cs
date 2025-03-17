using Planify_BackEnd.DTOs.Reports;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Reports;

namespace Planify_BackEnd.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<IEnumerable<ReportVM>> GetReportsByReceivedUser(Guid receivedUserId)
        {
            try
            {
                IEnumerable<Report> reports = await _reportRepository.GetReportsByReceivedUser(receivedUserId);
                IEnumerable<ReportVM> reportVMs = reports.Select(report => new ReportVM
                {
                    Id = report.Id,
                    Reason = report.Reason,
                    SendFrom = report.SendFrom,
                    SendFromNavigation = report.SendFromNavigation == null ? new UserNameVM() : new UserNameVM
                    {
                        Id = report.SendFromNavigation.Id,
                        Email = report.SendFromNavigation.Email,
                        FirstName = report.SendFromNavigation.FirstName,
                        LastName = report.SendFromNavigation.LastName,
                    },
                    SendTo = report.SendTo,
                    SendToNavigation = report.SendToNavigation == null ? new UserNameVM() : new UserNameVM
                    {
                        Id = report.SendToNavigation.Id,
                        Email = report.SendToNavigation.Email,
                        FirstName = report.SendToNavigation.FirstName,
                        LastName = report.SendToNavigation.LastName,
                    },
                    SendTime = report.SendTime,
                    Status = report.Status,
                    TaskId = report.TaskId,
                    TaskDTO = new DTOs.Tasks.TaskSearchResponeDTO
                    {
                        Id = report.Task.Id,
                        TaskName = report.Task.TaskName,
                        TaskDescription = report.Task.TaskDescription,
                        AmountBudget = report.Task.AmountBudget,
                        Deadline = report.Task.Deadline,
                        Progress = report.Task.Progress,
                        StartTime = report.Task.StartTime,
                        Status = report.Task.Status,
                        GroupId = report.Task.GroupId
                    },
                    ReportMedia = report.ReportMedia == null ? new List<ReportMediumVM>() : report.ReportMedia.Select(rm => new ReportMediumVM
                    {
                        Id = rm.Id,
                        MediumDTO = new DTOs.Medias.MediumDTO
                        {
                            Id = rm.Media.Id,
                            MediaUrl = rm.Media.MediaUrl,
                        }
                    }).ToList(),

                }).ToList();
                return reportVMs;
            }catch(Exception ex)
            {
                return new List<ReportVM>();
            }
        }
    }
}
