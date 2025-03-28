using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Services.Events
{
    public interface IEventService
    {
        Task<IEnumerable<EventGetListResponseDTO>> GetAllEventAsync(int campusId, int page, int pageSize);
        Task<ResponseDTO> CreateEventAsync(EventCreateRequestDTO eventDTO, Guid organizerId);
        Task<ResponseDTO> UploadImageAsync(UploadImageRequestDTO imageDTO);
        Task<ResponseDTO> GetEventDetailAsync(int eventId);
        Task<EventDetailDto> UpdateEventAsync(EventDTO e);
        Task<bool> DeleteEventAsync(int eventId);
        Task<PageResultDTO<EventGetListResponseDTO>> SearchEventAsync(int page, int pageSize,
            string? title,
            DateTime? startTime, DateTime? endTime,
            decimal? minBudget, decimal? maxBudget,
            int? isPublic,
            int? status,
            int? CategoryEventId,
            string? placed,
            Guid userId,
            int campusId);

        Task<ResponseDTO> CreateSaveDraft(EventCreateRequestDTO eventDTO, Guid organizerId);
        Task<ResponseDTO> UpdateSaveDraft(EventDTO eventDTO);
        Task<ResponseDTO> GetSaveDraft(Guid createBy);
    }
}
