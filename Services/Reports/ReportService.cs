using Microsoft.Data.SqlClient;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Reports;
using Planify_BackEnd.DTOs.Users;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.Reports;
using Planify_BackEnd.Services.GoogleDrive;

namespace Planify_BackEnd.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly GoogleDriveService _googleDriveService;
        public ReportService(IReportRepository reportRepository, GoogleDriveService googleDriveService)
        {
            _reportRepository = reportRepository;
            _googleDriveService = googleDriveService;
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

        public async Task<ResponseDTO> UploadImageAsync(UploadReportImageRequestDTO imageDTO)
        {
            if (imageDTO.ReportMediaFiles != null && imageDTO.ReportMediaFiles.Any())
            {
                foreach (var file in imageDTO.ReportMediaFiles)
                {
                    using var stream = file.OpenReadStream();
                    string contentType = file.ContentType;
                    string driveUrl;
                    driveUrl = await _googleDriveService.UploadFileAsync(stream, file.FileName, contentType);
                    if (string.IsNullOrEmpty(driveUrl))
                    {
                        Console.WriteLine($"❌ Upload thất bại cho file {file.FileName}");
                        throw new Exception("Upload failed, MediaURL is null or empty.");
                    }
                    Console.WriteLine($"✅ File {file.FileName} đã upload thành công: {driveUrl}");
                    var mediaItem = new Medium { MediaUrl = driveUrl };
                    await _reportRepository.CreateMediaItemAsync(mediaItem);

                    var reportMedia = new ReportMedium
                    {
                        ReportId = imageDTO.ReportId,
                        MediaId = mediaItem.Id
                
                    };
                  
                    await _reportRepository.AddEventMediaAsync(reportMedia);
                }
            }

            return new ResponseDTO(201, "Upload Image successfully!", null);
        }
    }
}
