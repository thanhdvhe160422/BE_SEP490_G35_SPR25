using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.JoinProjects;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JoinProjectController : ControllerBase
    {
        private readonly IJoinProjectService _joinProjectService;
        public JoinProjectController(IJoinProjectService joinProjectService)
        {
            _joinProjectService = joinProjectService;
        }
        [HttpGet("get-list-event-joined")]
        [Authorize(Roles = "Implementer")]
        public async Task<IActionResult> GetAllJoinedProjects(int page, int pageSize)
        {
            try
            {
                var userId =Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) ;
                var response = await _joinProjectService.GetAllJoinedProjects(userId, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("delete-from-project/{eventId}/{userId}")]
        //[Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> DeleteImplementerFromEvent(int eventId, Guid userId)
        {
            try
            {
                if (await _joinProjectService.DeleteImplementerFromEvent(userId, eventId))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Cannot remove implementer from event!");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
