using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.Users;
using System.Security.Claims;

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
        [Authorize(Roles = "Event Organizer")]
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

        [HttpGet("search")]
        [Authorize(Roles = "Event Organizer, Campus Manager")]
        public async Task<IActionResult> SearchUsers([FromQuery] string input)
        {
            var campusId = int.Parse(User.FindFirst("campusId")?.Value);
            if (string.IsNullOrWhiteSpace(input) || campusId == null)
            {
                return BadRequest("Input và CampusName không được để trống.");
            }

            var users = await _userService.GetUserByNameOrEmailAsync(input, campusId);
            if (users == null || !users.Any())
            {
                return NotFound("Không tìm thấy người dùng phù hợp.");
            }

            return Ok(users);
        }
    }
}
