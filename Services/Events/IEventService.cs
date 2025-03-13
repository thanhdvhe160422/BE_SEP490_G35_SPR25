using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using static Planify_BackEnd.DTOs.Events.EventDetailResponseDTO;

namespace Planify_BackEnd.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventGetListResponseDTO>> GetAllEvent(int page, int pageSize);
        Task<ResponseDTO> CreateEventAsync(EventCreateRequestDTO eventDTO, Guid organizerId);

        Task<ResponseDTO> GetEventDetailAsync(int eventId);
        Task<EventDetailDto> UpdateEventAsync(EventDTO e);
        Task<bool> DeleteEventAsync(int eventId);
        Task<IEnumerable<EventGetListResponseDTO>> SearchEventAsync(int page, int pageSize,
            string? title,
            DateTime? startTime, DateTime? endTime,
            decimal? minBudget, decimal? maxBudget,
            int? isPublic,
            int? status,
            int? CategoryEventId,
            string? placed);
    }
}
