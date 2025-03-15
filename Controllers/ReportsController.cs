using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        //[Authorize(Roles = "Event Organizer")]
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
    }
}
