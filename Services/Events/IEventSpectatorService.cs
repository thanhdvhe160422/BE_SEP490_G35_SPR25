using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.DTOs;

namespace Planify_BackEnd.Services.Events
{
    public interface IEventSpectatorService
    {
        EventVMSpectator GetEventById(int id);
        PageResultDTO<EventBasicVMSpectator> GetEvents(int page, int pageSize, Guid userId);
        PageResultDTO<EventBasicVMSpectator> SearchEvent(int page, int pageSize, string? name, DateTime? startDate, DateTime? endDate, string? placed, Guid userId);
    }
}
