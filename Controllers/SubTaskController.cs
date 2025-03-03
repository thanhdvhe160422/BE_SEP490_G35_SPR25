using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.SubTasks;
using Planify_BackEnd.Services.SubTasks;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTaskController : ControllerBase
    {
        private readonly ISubTaskService _subTaskService;
        public SubTaskController(ISubTaskService subTaskService)
        {
            _subTaskService = subTaskService;
        }
        [HttpPost("create")]
        [Authorize(Roles = "Implementer")]
        public async Task<IActionResult> CreateSubTask([FromBody] SubTaskCreateRequestDTO subTaskDTO)
        {
            var implementerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = await _subTaskService.CreateSubTaskAsync(subTaskDTO, implementerId);
            return StatusCode(response.Status, response);
        }
    }
}
