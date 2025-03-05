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
        [HttpGet]
        public IActionResult GetAllJoinedProjects(int page, int pageSize)
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var response = _joinProjectService.GetAllJoinedProjects(Guid.Parse(userId), page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
