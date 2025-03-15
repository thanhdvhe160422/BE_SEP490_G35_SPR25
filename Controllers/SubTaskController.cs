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
    public class SubTasksController : ControllerBase
    {
        private readonly ISubTaskService _subTaskService;
        public SubTasksController(ISubTaskService subTaskService)
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
        [HttpPut("{subTaskId}/amount")]
        //[Authorize(Roles = "Implementer")]
        public IActionResult UpdateActualSubTaskAmount(int subTaskId, [FromBody] decimal amount)
        {
            try
            {
                var response = _subTaskService.UpdateActualSubTaskAmount(subTaskId, amount);
                if (!response)
                {
                    return BadRequest("Cannot update subtask amount!");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
