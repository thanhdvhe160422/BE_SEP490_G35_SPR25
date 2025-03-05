using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Services.Events;
using Planify_BackEnd.Services.Tasks;
using Planify_BackEnd.DTOs.Tasks;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost("create")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateRequestDTO taskDTO)
        {
            var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _taskService.CreateTaskAsync(taskDTO, organizerId);

            return StatusCode(response.Status, response);
        }
        [HttpGet("search")]
        [Authorize(Roles = "Event Organizer")]
        public IActionResult SearchTasks(int page, int pageSize, string? name, DateTime startDate, DateTime deadline)
        {
            try
            {
                var response = _taskService.SearchTaskOrderByStartDate(page, pageSize, name, startDate, deadline);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
