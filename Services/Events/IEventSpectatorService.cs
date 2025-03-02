using Planify_BackEnd.DTOs.Events;

namespace Planify_BackEnd.Services.Events
{
    public interface IEventSpectatorService
    {
        EventVMSpectator GetEventById(int id);
        List<EventBasicVMSpectator> GetEventsOrderByStartDate(int page, int pageSize);
        List<EventBasicVMSpectator> SearchEventOrderByStartDate(int page, int pageSize, string? name, DateTime startDate, DateTime endDate);
    }
}
