﻿using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventGetListResponseDTO>> GetAllEvent();
        Task<ResponseDTO> CreateEventAsync(EventCreateRequestDTO eventDTO, Guid organizerId);
    }
}
