using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Events;

namespace Planify_BackEnd.Services.Events
{
    public class EventSpectatorService
    {
        private IEventSpectatorRepository _repository;
        public EventSpectatorService(IEventSpectatorRepository repository)
        {
            _repository = repository;
        }
        public List<EventBasicVMSpectator> GetEventOrderByStartDate(int page, int pageSize)
        {
            List<Event> events = _repository.GetEventsOrderByStartDate(page, pageSize);
            List<EventBasicVMSpectator> eventBasicVMs = new List<EventBasicVMSpectator>();
            foreach(var item in events)
            {
                EventBasicVMSpectator eventBasicVM = new EventBasicVMSpectator
                {
                    Id = item.Id,
                    CampusId = item.CampusId,
                    CampusDTO = new DTOs.Campus.CampusDTO
                    {
                        Id = item.Campus.Id,
                        CampusName = item.Campus.CampusName,
                        Status = item.Campus.Status
                    },
                    CategoryEventId = item.CategoryEventId,
                    CategoryViewModel = new DTOs.Categories.CategoryViewModel
                    {
                        Id = item.CategoryEvent.Id,
                        CategoryEventName = item.CategoryEvent.CategoryEventName
                    },
                    EndOfEvent = item.EndOfEvent,
                    Status = item.Status,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    EventDescription = item.EventDescription,
                    EventTitle = item.EventTitle,
                    IsPublic = item.IsPublic,
                    Placed = item.Placed,
                    TimeOfEvent = item.TimeOfEvent,
                    EventMedias = item.EventMedia.Select(em => new DTOs.Medias.EventMediumViewMediaModel
                    {
                        Id = em.Id,
                        EventId = em.Id,
                        MediaId = em.Id,
                        Status = em.Status,
                        MediaDTO = new DTOs.Medias.MediaItemDTO
                        {
                            Id = em.Media.Id,
                            MediaUrl = em.Media.MediaUrl
                        },
                    }).ToList()
                    
                };
                eventBasicVMs.Add(eventBasicVM);
            }
            return eventBasicVMs;
        }
        public List<EventBasicVMSpectator> SearchEventOrderByStartDate(int page, int pageSize, string?name, DateTime startDate, DateTime endDate)
        {
            List<Event> events = _repository.SearchEventOrderByStartDate(page,pageSize,name,startDate,endDate);
            List<EventBasicVMSpectator> eventBasicVMs = new List<EventBasicVMSpectator>();
            foreach (var item in events)
            {
                EventBasicVMSpectator eventBasicVM = new EventBasicVMSpectator
                {
                    Id = item.Id,
                    CampusId = item.CampusId,
                    CampusDTO = new DTOs.Campus.CampusDTO
                    {
                        Id = item.Campus.Id,
                        CampusName = item.Campus.CampusName,
                        Status = item.Campus.Status
                    },
                    CategoryEventId = item.CategoryEventId,
                    CategoryViewModel = new DTOs.Categories.CategoryViewModel
                    {
                        Id = item.CategoryEvent.Id,
                        CategoryEventName = item.CategoryEvent.CategoryEventName
                    },
                    EndOfEvent = item.EndOfEvent,
                    Status = item.Status,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    EventDescription = item.EventDescription,
                    EventTitle = item.EventTitle,
                    IsPublic = item.IsPublic,
                    Placed = item.Placed,
                    TimeOfEvent = item.TimeOfEvent,
                    EventMedias = item.EventMedia.Select(em => new DTOs.Medias.EventMediumViewMediaModel
                    {
                        Id = em.Id,
                        EventId = em.Id,
                        MediaId = em.Id,
                        Status = em.Status,
                        MediaDTO = new DTOs.Medias.MediaItemDTO
                        {
                            Id = em.Media.Id,
                            MediaUrl = em.Media.MediaUrl
                        },
                    }).ToList()

                };
                eventBasicVMs.Add(eventBasicVM);
            }
            return eventBasicVMs;
        }
    }
}
