using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Repositories.Events;
using Planify_BackEnd.Services.Events;

namespace Planify_BackEnd.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventForSpectatorsController : ControllerBase
    {
        private readonly IEventSpectatorService _service;
        public EventForSpectatorsController(IEventSpectatorService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetEvents(int page, int pageSize)
        {
            try
            {
                var response = _service.GetEventsOrderByStartDate(page, pageSize);
                if (response == null || response.Count==0)
                {
                    return NotFound("Cannot found any event");
                }
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            try
            {
                var response = _service.GetEventById(id);
                if (response== null || response.Id == null)
                {
                    return NotFound("Cannot fount event with id: "+id);
                }
                return Ok(response); 
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("search")]
        public IActionResult SearchEvents(int page, int pageSize, string? name, DateTime startDate, DateTime endDate)
        {
            try
            {
                var response = _service.SearchEventOrderByStartDate(page, pageSize, name, startDate, endDate);
                if (response == null || response.Count == 0)
                {
                    return NotFound("Cannot found any event");
                }
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
