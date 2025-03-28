using Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Categories;
using Planify_BackEnd.Services.Campus;
using Planify_BackEnd.Services.Categories;
using Planify_BackEnd.Services.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Planify_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly ICategoryService _categoryService;
        private readonly ICampusService _campusService;

        public EventsController(IEventService eventService, ICampusService campusService, ICategoryService categoryService)
        {
            _eventService = eventService;
            _campusService = campusService;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Retrieves all events with related data.
        /// </summary>
        /// <returns>A list of all events.</returns>
        [HttpGet ("list")]
        //[Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> GetAllEvents(int page, int pageSize)
        {
            try
            {
                var campusId = int.Parse(User.FindFirst("campusId")?.Value);
                var response = await _eventService.GetAllEventAsync(campusId, page,  pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }
        /// <summary>
        /// Create a new event.
        /// </summary>
        /// <param name="eventDTO"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Event Organizer, Campus Manager")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateRequestDTO eventDTO)
        {
            var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var response = await _eventService.CreateEventAsync(eventDTO, organizerId);

            return StatusCode(response.Status, response);
        }
        /// <summary>
        /// Upload image for event.
        /// </summary>
        /// <param name="imageDTO"></param>
        /// <returns></returns>
        [HttpPost("upload-image")]
        [Authorize(Roles = "Event Organizer, Campus Manager")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestDTO imageDTO)
        {
            var response = await _eventService.UploadImageAsync(imageDTO);

            return StatusCode(response.Status, response);
        }
        /// <summary>
        ///     Get event detail by event id.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("get-event-detail")]
        [Authorize(Roles = "Event Organizer, Campus Manager")]
        public async Task<IActionResult> GetEventDetail(int eventId)
        {
            var response = await _eventService.GetEventDetailAsync(eventId);

            return StatusCode(response.Status, response);
        }
        /// <summary>
        /// Get event detail for implementer.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("get-event-detail-implementer")]
        [Authorize(Roles = "Implementer")]
        public async Task<IActionResult> GetEventDetailForImp(int eventId)
        {
            var response = await _eventService.GetEventDetailAsync(eventId);

            return StatusCode(response.Status, response);
        }
        /// <summary>
        /// Update event.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eventDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> UpdateEvent(int id,[FromBody] EventDTO eventDTO)
        {
            try
            {
                if (id != eventDTO.Id)
                {
                    return BadRequest("Not allow update id");
                }
                //var getDetail = await _eventService.GetEventDetailAsync(id);
                //Event eventDetails = getDetail.Result as Event;
                //var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                //if (!userId.Equals(eventDetails.ManagerId))
                //{
                //    return Unauthorized();
                //}
                var campus = await _campusService.GetCampusByName(eventDTO.CampusName);
                if (campus == null || campus.Id == 0) return NotFound("Cannot found any campus with name: " + eventDTO.CampusName);
                var category = await _categoryService.GetCategoryByName(eventDTO.CategoryEventName, campus.Id);
                if (category == null || category.Id == 0) return NotFound("Cannot found any category with name: " + eventDTO.CategoryEventName + ", campus: " + eventDTO.CampusName);
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
        /// <summary>
        /// Delete event.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("delete/{id}")]
        [Authorize(Roles = "Event Organizer")]
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
        /// <summary>
        /// Search event by title, start time, end time, min budget, max budget, is public, status, category event id, placed.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="title"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="minBudget"></param>
        /// <param name="maxBudget"></param>
        /// <param name="isPublic"></param>
        /// <param name="status"></param>
        /// <param name="CategoryEventId"></param>
        /// <param name="placed"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [Authorize(Roles = "Event Organizer, Implementer, Campus Manager")]
        public async Task<IActionResult> SearchEventAsync(int page, int pageSize, string? title, 
            DateTime? startTime, DateTime? endTime, decimal? minBudget, decimal? maxBudget, 
            int? isPublic, int? status, int? CategoryEventId, string? placed)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var campusClaim = User.Claims.FirstOrDefault(c => c.Type == "campusId");
                var response = await _eventService.SearchEventAsync(page, pageSize, title, startTime, endTime,
                minBudget, maxBudget, isPublic, status, CategoryEventId, placed,userId,int.Parse(campusClaim.Value));
                if (response.TotalCount == 0)
                    return NotFound("Not found any event");
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("create-save-draft")]
        //[Authorize(Roles = "Event Organizer, Campus Manager")]
        //public async Task<IActionResult> CreateSaveDraft([FromBody] EventCreateRequestDTO eventDTO)
        //{
        //    var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        //    var response = await _eventService.CreateSaveDraft(eventDTO, organizerId);

        //    return StatusCode(response.Status, response);
        //}

        //[HttpPut("update-save-draft")]
        //[Authorize(Roles = "Event Organizer, Campus Manager")]
        //public async Task<IActionResult> UpdateSaveDraft([FromBody] EventDTO eventDTO)
        //{
        //    var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        //    var response = await _eventService.UpdateSaveDraft(eventDTO);

        //    return StatusCode(response.Status, response);
        //}
        //[HttpGet("get-save-draft")]
        //[Authorize(Roles = "Event Organizer, Campus Manager")]
        //public async Task<IActionResult> GetSaveDraft()
        //{
        //    var organizerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        //    var response = await _eventService.GetSaveDraft(organizerId);

        //    return StatusCode(response.Status, response);
        //}
    }
}
