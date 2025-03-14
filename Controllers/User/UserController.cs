using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.Users;

namespace Planify_BackEnd.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserservice _userService;
        public UsersController(IUserservice userService)
        {
            _userService = userService;
        }
        [HttpGet("getListImplementer/{eventId}")]
        //[Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> GetListImplementer(int eventId, int page, int pageSize)
        {
          
            try
            {
                var response = await _userService.GetListImplementer(eventId, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }
    }
}
