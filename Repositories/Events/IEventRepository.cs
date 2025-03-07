using Planify_BackEnd.Models;
using static Planify_BackEnd.DTOs.Events.EventDetailResponseDTO;

namespace Planify_BackEnd.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEvent(int page, int pageSize);
        Task<Event> CreateEventAsync(Event newEvent);

        System.Threading.Tasks.Task CreateMediaItemAsync(Medium mediaItem);
        System.Threading.Tasks.Task AddEventMediaAsync(EventMedium eventMedia);
        Task<CategoryEvent> GetCategoryEventAsync(int categoryId, int campusId);
        Task<EventDetailDto?> GetEventDetailAsync(int eventId);
    }
}
