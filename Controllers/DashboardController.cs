using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.Dashboards;
using Planify_BackEnd.Services.Events;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("monthly-stats")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMonthlyStats()
        {
            var stats = await _dashboardService.GetMonthlyStatsAsync();
            return Ok(stats);
        }
        [HttpGet("used-categories")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsedCategories()
        {
            var result = await _dashboardService.GetUsedCategoriesAsync();
            return Ok(result);
        }

    }
}
