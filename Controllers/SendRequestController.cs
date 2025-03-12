using Microsoft.AspNetCore.Mvc;
using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Services.EventRequests;

namespace Planify_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendRequestController : ControllerBase
    {
        private readonly ISendRequestService _sendRequestService;

        public SendRequestController(ISendRequestService sendRequestService)
        {
            _sendRequestService = sendRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] SendRequestDTO requestDTO)
        {
            var request = await _sendRequestService.CreateRequestAsync(requestDTO);
            return Ok(request);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRequest(int id, [FromQuery] Guid managerId)
        {
            var request = await _sendRequestService.ApproveRequestAsync(id, managerId);
            return Ok(request);
        }
    }
}
