using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.Campus;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampusController : ControllerBase
    {
        private readonly ICampusService _campusService;
        public CampusController(ICampusService campusService)
        {
            _campusService = campusService;
        }
        [HttpGet("List")]
        public async Task<IActionResult> GetAllCampus()
        {
            try
            {
                var response = await _campusService.GetAllCampus();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }
    }
}
