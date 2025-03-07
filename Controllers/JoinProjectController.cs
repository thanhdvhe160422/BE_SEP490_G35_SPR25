using System.Security.Claims;
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
        [HttpGet("List")]
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
    }
}
