
using Planify_BackEnd.DTOs;

public class EventService
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

