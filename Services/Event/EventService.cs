
using Planify_BackEnd.DTOs;
using Planify_BackEnd.Services.Event;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }
    public ResponseDTO GetAllEvent()
    {
        var events = _eventRepository.GetAllEvent();
        return new ResponseDTO
        {
            Result = events,
            IsSuccess = true,
            Message = "Events retrieved successfully"
        };

    }
}

