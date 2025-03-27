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
        [HttpGet("{campusName}")]
        public async Task<IActionResult> GetCampusIdByName(string campusName)
        {
            try
            {
                var response = await _campusService.GetCampusByName(campusName);
                if (response==null) return NotFound("Not found campus with name "+ campusName);
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
