﻿using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.EventRequests
{
    public interface ISendRequestService
    {
        Task<ResponseDTO> GetRequestsAsync();
        Task<ResponseDTO> CreateRequestAsync(SendRequestDTO requestDTO);
        Task<ResponseDTO> ApproveRequestAsync(int id, Guid managerId);
    }
}
