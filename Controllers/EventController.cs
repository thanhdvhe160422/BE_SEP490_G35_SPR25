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
        [HttpGet ("List")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> GetAllEvents(int page, int pageSize)
        {
            try
            {
                var response = await _eventService.GetAllEvent( page,  pageSize);
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

        [HttpGet("get-event-detail")]
        public async Task<IActionResult> GetEventDetail(int eventId)
        {
            var response = await _eventService.GetEventDetailAsync(eventId);

            return StatusCode(response.Status, response);
        }
    }
}
