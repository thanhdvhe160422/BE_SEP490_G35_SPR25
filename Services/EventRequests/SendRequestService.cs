﻿using Microsoft.AspNetCore.SignalR;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Hub;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories;
using Planify_BackEnd.Repositories.SendRequests;

namespace Planify_BackEnd.Services.EventRequests
{
    public class SendRequestService : ISendRequestService
    {
        private readonly ISendRequestRepository _requestRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IHubContext<EventRequestHub> _hubContext;

        public SendRequestService(
            ISendRequestRepository requestRepository,
            IEventRepository eventRepository,
            IHubContext<EventRequestHub> hubContext)
        {
            _requestRepository = requestRepository ?? throw new ArgumentNullException(nameof(requestRepository));
            _eventRepository = eventRepository ?? throw new ArgumentNullException(nameof(eventRepository));
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
        }

        public async Task<ResponseDTO> GetRequestsAsync()
        {
            var requests = await _requestRepository.GetRequestsAsync();
            return new ResponseDTO(200, "Lấy danh sách yêu cầu thành công", requests);
        }

        public async Task<ResponseDTO> CreateRequestAsync(SendRequestDTO requestDTO)
        {
            try
            {
                if (requestDTO == null)
                    return new ResponseDTO(400, "Dữ liệu không hợp lệ", null);

                if (requestDTO.EventId <= 0)
                    return new ResponseDTO(400, "Event ID không hợp lệ", null);

                var eventEntity = await _eventRepository.GetEventByIdAsync(requestDTO.EventId);
                if (eventEntity == null)
                    return new ResponseDTO(404, "Không tìm thấy sự kiện", null);

                if (eventEntity.Status != 0)
                    return new ResponseDTO(400, "Sự kiện không thể tạo yêu cầu mới do trạng thái hiện tại", null);

                var existingRequest = await _requestRepository.GetRequestByIdAsync(requestDTO.EventId);
                if (existingRequest != null && existingRequest.Status == 0)
                    return new ResponseDTO(400, "Sự kiện đã có yêu cầu đang chờ duyệt", null);

                var request = new SendRequest
                {
                    EventId = requestDTO.EventId,
                    Status = 0 // Chờ duyệt
                };

                request = await _requestRepository.CreateRequestAsync(request);
                if (request == null)
                    return new ResponseDTO(500, "Không thể tạo yêu cầu", null);

                eventEntity.Status = 1; // Chờ duyệt
                await _eventRepository.UpdateEventAsync(eventEntity);

                // Gửi thông báo SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveEventRequest", request);

                return new ResponseDTO(201, "Tạo yêu cầu thành công", request);
            }
            catch (Exception ex)
            {
                return new ResponseDTO(500, "Đã xảy ra lỗi khi tạo yêu cầu", null);
            }
        }

        public async Task<ResponseDTO> ApproveRequestAsync(int id, Guid managerId, string reason)
        {
            try
            {
                if (id <= 0) return new ResponseDTO(400, "ID không hợp lệ", null);
                if (managerId == Guid.Empty) return new ResponseDTO(400, "Manager ID không hợp lệ", null);

                var request = await _requestRepository.GetRequestByIdAsync(id);
                if (request == null) return new ResponseDTO(404, "Không tìm thấy yêu cầu", null);

                if (request.Status != 0)
                    return new ResponseDTO(400, "Yêu cầu không thể được duyệt vì không ở trạng thái chờ duyệt", null);

                var eventEntity = await _eventRepository.GetEventByIdAsync(request.EventId);
                if (eventEntity == null)
                    return new ResponseDTO(404, "Không tìm thấy sự kiện", null);

                if (eventEntity.Status != 1)
                    return new ResponseDTO(400, "Sự kiện không thể được duyệt do trạng thái hiện tại", null);

                request.Status = 1; // Đã duyệt
                request.ManagerId = managerId;
                request.Reason = reason;
                eventEntity.Status = 2; // Đã duyệt

                await _requestRepository.UpdateRequestAsync(request);
                await _eventRepository.UpdateEventAsync(eventEntity);
                await _hubContext.Clients.All.SendAsync("RequestApproved", request);

                return new ResponseDTO(200, "Yêu cầu đã được duyệt", request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error approving request: {ex.Message}");
                return new ResponseDTO(500, "Đã xảy ra lỗi khi duyệt yêu cầu", null);
            }
        }

        public async Task<ResponseDTO> RejectRequestAsync(int id, Guid managerId, string reason)
        {
            try
            {
                if (id <= 0) return new ResponseDTO(400, "ID không hợp lệ", null);
                if (managerId == Guid.Empty) return new ResponseDTO(400, "Manager ID không hợp lệ", null);

                var request = await _requestRepository.GetRequestByIdAsync(id);
                if (request == null) return new ResponseDTO(404, "Không tìm thấy yêu cầu", null);

                if (request.Status != 0)
                    return new ResponseDTO(400, "Yêu cầu không thể bị từ chối vì không ở trạng thái chờ duyệt", null);

                var eventEntity = await _eventRepository.GetEventByIdAsync(request.EventId);
                if (eventEntity == null)
                    return new ResponseDTO(404, "Không tìm thấy sự kiện", null);

                if (eventEntity.Status != 1)
                    return new ResponseDTO(400, "Sự kiện không thể bị từ chối do trạng thái hiện tại", null);

                request.Status = -1; // Bị từ chối
                request.ManagerId = managerId;
                request.Reason = reason;
                eventEntity.Status = -1; // Không được duyệt

                await _requestRepository.UpdateRequestAsync(request);
                await _eventRepository.UpdateEventAsync(eventEntity);
                await _hubContext.Clients.All.SendAsync("RequestRejected", request);

                return new ResponseDTO(200, "Yêu cầu đã bị từ chối", request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error rejecting request: {ex.Message}");
                return new ResponseDTO(500, "Đã xảy ra lỗi khi từ chối yêu cầu", null);
            }
        }
    }
}