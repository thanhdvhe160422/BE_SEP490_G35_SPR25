using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Services.Events;
using System.Security.Claims;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Retrieves all events with related data.
        /// </summary>
        /// <returns>A list of all events.</returns>
        [HttpGet]
        public ActionResult<ResponseDTO> GetAllEvents()
        {
            try
            {
                var response = _eventService.GetAllEvent();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequestDTO eventDTO)
        {
            var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _eventService.CreateEventAsync(eventDTO, organizerId);

            return StatusCode(response.Status, response);
        }
    }
}
