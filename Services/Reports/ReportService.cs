using Microsoft.Data.SqlClient;
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
        public async Task<IEnumerable<ReportVM>> GetAllReportsAsync()
        {
            try
            {
                var reports = await _reportRepository.GetAllReportsAsync();
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<ReportVM>> GetReportById(int reportId)
        {
            try
            {
                var reports = await _reportRepository.GetReportById(reportId);

                if (reports == null || !reports.Any())
                    return Enumerable.Empty<ReportVM>();

                return reports.Select(report => new ReportVM
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
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                throw; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw; 
            }
        }
        public async Task<ReportCreateDTO> CreateReportAsync(ReportCreateDTO reportDTO)
        {
            try
            {
                var report = new Report
                {
                    Reason = reportDTO.Reason,
                    SendFrom = reportDTO.SendFrom,
                    SendTo = reportDTO.SendTo,
                    SendTime = reportDTO.SendTime,
                    Status = 1,
                    TaskId = reportDTO.TaskId,
                };

                var createdReport = await _reportRepository.CreateReportAsync(report);

                return new ReportCreateDTO
                {
                    Id = createdReport.Id, 
                    Reason = createdReport.Reason,
                    SendFrom = createdReport.SendFrom,
                    SendTo = createdReport.SendTo,
                    SendTime = createdReport.SendTime,
                    Status = createdReport.Status,
                    TaskId = createdReport.TaskId,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while creating report: " + ex.Message);
                throw; 
            }
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
