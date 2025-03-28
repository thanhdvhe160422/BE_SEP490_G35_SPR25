using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Events
{
    public interface IEventSpectatorRepository
    {
        PageResultDTO<Event> GetEvents(int page, int pageSize, Guid userId);
        Event GetEventById(int id);
        PageResultDTO<Event> SearchEvent(int page, int pageSize, string? name, DateTime? startDate, DateTime? endDate, string? placed, Guid userId);
    }
}
