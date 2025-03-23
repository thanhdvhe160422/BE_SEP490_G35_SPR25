using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs.Reports;
using Planify_BackEnd.Services.Events;
using Planify_BackEnd.Services.Reports;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        [HttpGet("{receviedUserId}")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> GetReportsByReceviedUserId(Guid receviedUserId)
        {
            try
            {
                var response = _reportService.GetReportsByReceivedUser(receviedUserId);
                return Ok(response.Result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("View")]
        //[Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> GetAllReportsAsync()
        {
            try
            {
                var response = _reportService.GetAllReportsAsync();
                return Ok(response.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("View/{reportId}")]
        //[Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> GetReportById(int reportId)
        {
            try
            {
                var response = _reportService.GetReportById(reportId);
                return Ok(response.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Create")]
        [Authorize(Roles = "Implementer")]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateDTO reportDTO)
        {
            try
            {
                var response = _reportService.CreateReportAsync(reportDTO);
                return Ok(response.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("upload-image")]
        //[Authorize(Roles = "Event Organizer, Campus Manager")]
        public async Task<IActionResult> UploadImage([FromForm] UploadReportImageRequestDTO imageDTO)
        {
            var response = await _reportService.UploadImageAsync(imageDTO);

            return StatusCode(response.Status, response);
        }
    }
}
