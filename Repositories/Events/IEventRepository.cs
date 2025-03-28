using Planify_BackEnd.DTOs;
using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventAsync(int page, int pageSize);
        Task<Event> CreateEventAsync(Event newEvent);

        System.Threading.Tasks.Task CreateMediaItemAsync(Medium mediaItem);
        System.Threading.Tasks.Task AddEventMediaAsync(EventMedium eventMedia);
        Task<CategoryEvent> GetCategoryEventAsync(int categoryId, int campusId);
        Task<EventDetailDto> GetEventDetailAsync(int eventId);
        Task<Event> UpdateEventAsync(Event e);
        Task<bool> DeleteEventAsync(int eventId);
        Task<IEnumerable<Event>>SearchEventAsync(int page,int pageSize, 
            string? title, 
            DateTime? startTime, DateTime? endTime,
            decimal? minBudget, decimal? maxBudget,
            int? isPublic,
            int? status,
            int? CategoryEventId,
            string? placed);
        System.Threading.Tasks.Task CreateRiskAsync(Risk risk);
    }
}
