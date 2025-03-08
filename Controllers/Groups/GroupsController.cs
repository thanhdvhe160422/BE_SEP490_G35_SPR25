using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.Services.Groups;

namespace Planify_BackEnd.Controllers.Groups
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpPut("allocate-cost/{groupId}/{cost}")]
        public IActionResult AllocateCostToGroup(int groupId, decimal cost)
        {
            try
            {
                bool response = _groupService.AllocateCostToGroup(groupId, cost);
                if (response)
                    return Ok("");
                else
                    return BadRequest("Cannot allocate cost to group id " + groupId);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("add-lead-group/{groupId}/{implementerId}")]
        //[Authorize("Event Organizer")]
        public IActionResult AddLeadGroup(int groupId,Guid implementerId)
        {
            try
            {
                if (_groupService.AddLeadGroup(groupId, implementerId))
                    return Ok();
                else
                    return BadRequest("Cannot add lead group!");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("remove-lead-group/{groupId}/{implementerId}")]
        //[Authorize("Event Organizer")]
        public IActionResult RemoveLeadGroup(int groupId, Guid implementerId)
        {
            try
            {
                if (_groupService.RemoveLeadGroup(groupId, implementerId))
                    return Ok();
                else
                    return BadRequest("Cannot remove lead group!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
