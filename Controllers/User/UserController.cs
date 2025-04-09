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
        [HttpPost("create-campus-manager")]
        [Authorize(Roles = "Campus Manager, Admin")]
        public async Task<IActionResult> CreateManagerAsync(UserDTO userDTO)
        {
            try
            {
                var user = await _userService.CreateManagerAsync(userDTO);
                if (user == null)
                {
                    return BadRequest("Cannot create user!");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("update-campus-manager/{id}")]
        [Authorize(Roles = "Campus Manager, Admin")]
        public async Task<IActionResult> UpdateManagerAsync(UserUpdateDTO userDTO, Guid id)
        {
            try
            {
                var user = await _userService.UpdateManagerAsync(userDTO, id);
                if (user == null)
                {
                    return BadRequest("Cannot update user!");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-list-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetListUserAsync(int page, int pageSize)
        {
            try
            {
                var response =  _userService.GetListUser(page, pageSize);
                if (response.TotalCount == 0)
                {
                    return NotFound("Cannot found any user");
                }
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
        [HttpGet("get-list-implementer/{eventId}")]
        [Authorize(Roles = "Event Organizer, Campus Manager, Implementer")]
        public async Task<IActionResult> GetListImplementer(int eventId, int page, int pageSize)
        {
          
            try
            {
                var response =  _userService.GetListImplementer(eventId, page, pageSize);
                if (response.TotalCount == 0)
                {
                    return NotFound("Cannot found any implementer");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Event Organizer, Campus Manager")]
        public async Task<IActionResult> SearchUsers(int page, int pageSize, string input)
        {
            var campusId = int.Parse(User.FindFirst("campusId")?.Value);
            if (string.IsNullOrWhiteSpace(input) || campusId == null)
            {
                return BadRequest("Input và CampusName không được để trống.");
            }

            var users = await _userService.GetUserByNameOrEmailAsync(page, pageSize, input, campusId);
            if (users.TotalCount==0)
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
        [HttpPut("update-eog-role")]
        [Authorize(Roles = "Campus Manager, Admin")]
        public async Task<IActionResult> UpdateEventOrganizerRole(Guid userId, int roleId)
        {
            try
            {
                var response = await _userService.UpdateEOGRole(userId,roleId);
                if (!response)
                {
                    return NotFound("Not found any Event Organizer with id " + userId);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("event-organizer")]
        [Authorize(Roles = "Campus Manager, Admin")]
        public async Task<IActionResult> GetEventOrganizer(int page,int pageSize)
        {
            try
            {
                var campusId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "campusId").Value);
                var response = await _userService.GetEventOrganizer(page,pageSize,campusId);
                if (response.Items.Count() == 0)
                {
                    return NotFound("Cannot found any event organizer!");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-manager-role")]
        [Authorize(Roles = "Campus Manager, Admin")]
        public async Task<IActionResult> UpdateCampusManagerRole(Guid userId, int roleId)
        {
            try
            {
                var response = await _userService.UpdateCampusManagerRole(userId, roleId);
                if (!response)
                {
                    return NotFound("Not found any Campus Manager with id " + userId);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("campus-manager")]
        [Authorize(Roles = "Campus Manager, Admin")]
        public async Task<IActionResult> GetCampusManager(int page, int pageSize)
        {
            try
            {
                var campusId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "campusId").Value);
                var response = await _userService.GetCampusManager(page, pageSize, campusId);
                if (response.Items.Count() == 0)
                {
                    return NotFound("Cannot found any campus manager!");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search/v2")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchUsersForAdmin(int page, int pageSize, string input)
        {
            var campusId = int.Parse(User.FindFirst("campusId")?.Value);
            var users = await _userService.SearchUser(page, pageSize, input, campusId);
            if (users.TotalCount == 0)
            {
                return NotFound("Không tìm thấy người dùng phù hợp.");
            }

            return Ok(users);
        }
    }
}
