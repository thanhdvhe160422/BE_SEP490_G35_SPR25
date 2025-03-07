using Azure.Core;
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
        [HttpGet("{id}")]
        public IActionResult GetUserProfileById(Guid id,string token)
        {
            try
            {
                //var handle = new JwtSecurityTokenHandler();
                //var jwtToken = handle.ReadJwtToken(token);
                //var uid = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
                //if (!uid.Equals(id))
                //{
                //    return Unauthorized();
                //}
                var response = _profileService.getUserProfileById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
