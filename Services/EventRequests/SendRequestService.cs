using Microsoft.AspNetCore.SignalR;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Hub;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.SendRequests;

namespace Planify_BackEnd.Services.EventRequests
{
    public class SendRequestService : ISendRequestService
    {
        private readonly ISendRequestRepository _repository;
        private readonly IHubContext<EventRequestHub> _hubContext;

        public SendRequestService(ISendRequestRepository repository, IHubContext<EventRequestHub> hubContext)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task<ResponseDTO> GetRequestsAsync()
        {
            var requests = await _repository.GetRequestsAsync();
            return new ResponseDTO(200, "Lấy danh sách yêu cầu thành công", requests ?? new List<SendRequest>());
        }

        public async Task<ResponseDTO> CreateRequestAsync(SendRequestDTO requestDTO)
        {
            if (requestDTO == null)
                return new ResponseDTO(400, "Dữ liệu không hợp lệ", null);

            var request = new SendRequest
            {
                EventId = requestDTO.EventId,
                Reason = requestDTO.Reason ?? throw new ArgumentNullException(nameof(requestDTO.Reason)),
                Status = 0 // Chờ duyệt
            };

            request = await _repository.CreateRequestAsync(request);
            if (request == null)
                return new ResponseDTO(500, "Không thể tạo yêu cầu", null);

            // Gửi real-time đến tất cả Campus Manager
            await _hubContext.Clients.All.SendAsync("ReceiveEventRequest", request);

            return new ResponseDTO(201, "Tạo yêu cầu thành công", request);
        }

        public async Task<ResponseDTO> ApproveRequestAsync(int id, Guid managerId)
        {
            if (id <= 0) return new ResponseDTO(400, "ID không hợp lệ", null);
            if (managerId == Guid.Empty) return new ResponseDTO(400, "Manager ID không hợp lệ", null);

            var request = await _repository.GetRequestByIdAsync(id);
            if (request == null) return new ResponseDTO(404, "Không tìm thấy yêu cầu", null);

            request.Status = 1; // Đã duyệt
            request.ManagerId = managerId;

            await _repository.UpdateRequestAsync(request);

            // Gửi thông báo real-time đến Event Organizer
            await _hubContext.Clients.All.SendAsync("RequestApproved", request);

            return new ResponseDTO(200, "Yêu cầu đã được duyệt", request);
        }
    }
}
