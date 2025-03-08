using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.User;
using System.IdentityModel.Tokens.Jwt;

namespace Planify_BackEnd.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet("{userId}")]
        public IActionResult GetUserProfileById(Guid userId)
        {
            try
            {
                //var userId = User.Claims.FirstOrDefault(c=>c.Type== JwtRegisteredClaimNames.Sub)?.Value;
                //if (userId == null)
                //{
                //    return Unauthorized();
                //}
                var response = _profileService.getUserProfileById(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
