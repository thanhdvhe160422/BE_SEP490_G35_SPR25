using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Services.EventRequests;
using System.Security.Claims;

namespace Planify_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendRequestController : ControllerBase
    {
        private readonly ISendRequestService _sendRequestService;

        public SendRequestController(ISendRequestService sendRequestService)
        {
            _sendRequestService = sendRequestService ?? throw new ArgumentNullException(nameof(sendRequestService));
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var response = await _sendRequestService.GetRequestsAsync();
            return StatusCode(response.Status, response);
        }

        [HttpPost]
        [Authorize(Roles = "Event Organizer")]
        public async Task<IActionResult> SendRequest([FromBody] SendRequestDTO requestDTO)
        {
            var response = await _sendRequestService.CreateRequestAsync(requestDTO);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> ApproveRequest([FromBody] ApproveRequestDTO request)
        {
            var managerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(managerIdString))
            {
                return BadRequest(new ResponseDTO(400, "Không tìm thấy thông tin người dùng", null));
            }

            if (!Guid.TryParse(managerIdString, out var managerId))
            {
                return BadRequest(new ResponseDTO(400, "Manager ID không hợp lệ", null));
            }
            if (request.Reason == null) request.Reason = "";
            var response = await _sendRequestService.ApproveRequestAsync(request.Id, managerId, request.Reason);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Campus Manager")]
        public async Task<IActionResult> RejectRequest([FromBody] ApproveRequestDTO request)
        {
            var managerIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(managerIdString))
            {
                return BadRequest(new ResponseDTO(400, "Không tìm thấy thông tin người dùng", null));
            }

            if (!Guid.TryParse(managerIdString, out var managerId))
            {
                return BadRequest(new ResponseDTO(400, "Manager ID không hợp lệ", null));
            }

            var response = await _sendRequestService.RejectRequestAsync(request.Id, managerId, request.Reason);
            return StatusCode(response.Status, response);
        }
        [HttpGet("MyRequests/{userId}")]
        public async Task<IActionResult> GetMyRequests(Guid userId)
        {
            var campusClaim = User.Claims.FirstOrDefault(c => c.Type == "campusId");
            var response = await _sendRequestService.GetMyRequestsAsync(userId,campusClaim);
            return StatusCode(response.Status, response);
        }
    }
}