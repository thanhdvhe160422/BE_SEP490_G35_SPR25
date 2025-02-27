using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Repositories.Events;

namespace Planify_BackEnd.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventForSpectatorsController : ControllerBase
    {
        private readonly IEventSpectatorRepository _repository;
        public EventForSpectatorsController(IEventSpectatorRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult GetEvents(int page, int pageSize)
        {
            try
            {
                var response = _repository.GetEventsOrderByStartDate(page, pageSize);
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
                var response = _repository.GetEventById(id);
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
                var response = _repository.SearchEventOrderByStartDate(page, pageSize, name, startDate, endDate);
                return Ok(response);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
