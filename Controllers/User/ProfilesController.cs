using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.User;

namespace Planify_BackEnd.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfileService _profileService;
        public ProfilesController(ProfileService profileService)
        {
            _profileService = profileService;
        }
        [HttpGet("{id}")]
        public IActionResult GetUserProfileById(Guid id)
        {
            try
            {
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
