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
        //[Authorize(Roles = "Event Organizer")]
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
        [HttpPut("{id}")]
        //[Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> UpdateEvent(int id,[FromBody] EventDTO eventDTO)
        {
            try
            {
                if (id != eventDTO.Id)
                {
                    return BadRequest("Not allow update id");
                }
                var response = await _eventService.UpdateEventAsync(eventDTO);
                if (response == null || response.Id == 0)
                {
                    return BadRequest("Cannot update event!");
                }
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("delete/{id}")]
        //[Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                var response = await _eventService.DeleteEventAsync(id);
                if (response)
                    return Ok();
                else
                    return BadRequest("Cannot delete event!");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("search")]
        //[Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> SearchEventAsync(int page, int pageSize, string? title, 
            DateTime? startTime, DateTime? endTime, decimal? minBudget, decimal? maxBudget, 
            int? isPublic, int? status, int? CategoryEventId, string? placed)
        {
            try
            {
                var response = await _eventService.SearchEventAsync(page, pageSize, title, startTime, endTime,
                minBudget, maxBudget, isPublic, status, CategoryEventId, placed);
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
