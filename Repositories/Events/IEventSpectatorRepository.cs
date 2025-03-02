using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Events
{
    public interface IEventSpectatorRepository
    {
        List<Event> GetEventsOrderByStartDate(int page, int pageSize);
        Event GetEventById(int id);
        List<Event> SearchEventOrderByStartDate(int page, int pageSize, string? name, DateTime startDate, DateTime endDate);
    }
}
