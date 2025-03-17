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
        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet("list/{groupId}")]
        [Authorize(Roles = "Implementer")]
        public async Task<IActionResult> GetAllTasks(int groupId)
        {
            try
            {
                var response = await _taskService.GetAllTasksAsync(groupId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Search tasks by name,orderby start date and end date
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="name"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> SearchTasksAsync(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = await _taskService.SearchTaskOrderByStartDateAsync(page, pageSize, name, startDate, endDate);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{taskId}/amount")]
        [Authorize(Roles = "Implementer")]
        public IActionResult UpdateActualTaskAmount(int taskId, [FromBody] decimal amount)
        {
            try
            {
                var response = _taskService.UpdateActualTaskAmount(taskId, amount);
                if (!response)
                {
                    return BadRequest("Cannot update task amount!");
                }
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="taskDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateRequestDTO taskDTO)
        {
            var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value); 
            var response = await _taskService.CreateTaskAsync(taskDTO, organizerId);

            return StatusCode(response.Status, response);
        } 
        /// <summary>
        /// Update a task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskDTO"></param>
        /// <returns></returns>
        [HttpPut("update/{taskId}")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskUpdateRequestDTO taskDTO)
        {
            var response = await _taskService.UpdateTaskAsync(taskId, taskDTO);
            return StatusCode(response.Status, response);
        }
        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{taskId}")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var response = await _taskService.DeleteTaskAsync(taskId);
            return StatusCode(response.Status, response);
        }

    }
}
