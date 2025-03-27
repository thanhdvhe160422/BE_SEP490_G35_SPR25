using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.Users;
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
        [HttpGet("get-list-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetListUserAsync(int page, int pageSize)
        {
            try
            {
                var response = await _userService.GetListUserAsync(page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("get-user-detail/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserDetailAsync(Guid id)
        {
            try
            {
                var response = await _userService.GetUserDetailAsync(id);
                if (response == null)
                {
                    return NotFound(new { message = "User not found!" });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPut("ban/unban-users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserStatusAsync(Guid id, int newStatus)
        {
            try
            {
                var response = await _userService.UpdateUserStatusAsync(id, newStatus);
                if (response == null)
                {
                    return NotFound(new { message = "User not found!" });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
        [HttpPost("event-organizer")]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> CreateEventOrganizer(UserDTO userDTO)
        {
            try
            {
                var user = await _userService.CreateEventOrganizer(userDTO);
                if (user == null || user.FirstName == null)
                {
                    return BadRequest("Cannot create event organizer!");
                }
                UserRoleDTO roleDTO = new UserRoleDTO
                {
                    Id = 0,
                    RoleId = 3,
                    UserId = user.Id
                };
                var role = await _userService.AddUserRole(roleDTO);
                if (role == null || role.Id == 0)
                {
                    return BadRequest("Cannot add user role!");
                }
                return Ok(user);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("event-organizer")]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> UpdateEventOrganizer(UserDTO userDTO)
        {
            try
            {
                var user = await _userService.UpdateEventOrganizer(userDTO);
                if (user == null || user.FirstName == null)
                {
                    return BadRequest("Cannot update event organizer!");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
