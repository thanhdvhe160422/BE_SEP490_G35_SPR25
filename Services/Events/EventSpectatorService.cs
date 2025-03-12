using Planify_BackEnd.DTOs.Events;
using Planify_BackEnd.Models;
using Planify_BackEnd.Repositories.Events;

namespace Planify_BackEnd.Services.Events
{
    public class EventSpectatorService : IEventSpectatorService
    {
        private IEventSpectatorRepository _repository;
        public EventSpectatorService(IEventSpectatorRepository repository)
        {
            _repository = repository;
        }
        public EventVMSpectator GetEventById(int id)
        {
            try
            {

                var e = _repository.GetEventById(id);
                EventVMSpectator eventVM = new EventVMSpectator
                {
                    Id = e.Id,
                    EventTitle = e.EventTitle,
                    EventDescription = e.EventDescription,
                    CampusId = e.CampusId,
                    CampusDTO = e.Campus == null ? null : new DTOs.Campus.CampusDTO
                    {
                        Id = e.Campus.Id,
                        CampusName = e.Campus.CampusName
                    },
                    Status = e.Status,
                    CategoryEventId = e.CategoryEventId,
                    CategoryViewModel = e.CategoryEvent == null ? null : new DTOs.Categories.CategoryViewModel
                    {
                        Id = e.CategoryEvent.Id,
                        CategoryEventName = e.CategoryEvent.CategoryEventName
                    },
                    CreatedAt = e.CreatedAt,
                    IsPublic = e.IsPublic,
                    Placed = e.Placed,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    TimePublic = e.TimePublic,
                    EventMedias = e.EventMedia == null ? null : e.EventMedia.Select(em => new DTOs.Medias.EventMediumViewMediaModel
                    {
                        Id = em.Id,
                        EventId = em.EventId,
                        MediaId = em.MediaId,
                        Status = em.Status,
                        MediaDTO = new DTOs.Medias.MediaItemDTO
                        {
                            Id = em.Media.Id,
                            MediaUrl = em.Media.MediaUrl
                        }
                    }).ToList()
                };
                return eventVM;
            }catch (Exception ex)
            {
                Console.WriteLine("event spectator - getEvent: "+ex.Message);
                return new EventVMSpectator();
            }
        }
        public List<EventBasicVMSpectator> GetEventsOrderByStartDate(int page, int pageSize)
        {
            try
            {

                List<Event> events = _repository.GetEventsOrderByStartDate(page, pageSize);
                List<EventBasicVMSpectator> eventBasicVMs = new List<EventBasicVMSpectator>();
                foreach (var item in events)
                {
                    EventBasicVMSpectator eventBasicVM = new EventBasicVMSpectator
                    {
                        Id = item.Id,
                        CampusId = item.CampusId,
                        CampusDTO = item.Campus == null ? null : new DTOs.Campus.CampusDTO
                        {
                            Id = item.Campus.Id,
                            CampusName = item.Campus.CampusName,
                            Status = item.Campus.Status
                        },
                        CategoryEventId = item.CategoryEventId,
                        CategoryViewModel = item.CategoryEvent == null ? null : new DTOs.Categories.CategoryViewModel
                        {
                            Id = item.CategoryEvent.Id,
                            CategoryEventName = item.CategoryEvent.CategoryEventName
                        },
                        Status = item.Status,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        EventDescription = item.EventDescription,
                        EventTitle = item.EventTitle,
                        IsPublic = item.IsPublic,
                        Placed = item.Placed,
                        EventMedias = item.EventMedia == null ? null : item.EventMedia.Select(em => new DTOs.Medias.EventMediumViewMediaModel
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
            }catch(Exception ex)
            {
                Console.WriteLine("event spectator - getEvents: "+ex.Message);
                return new List<EventBasicVMSpectator>();
            }
        }
        public List<EventBasicVMSpectator> SearchEventOrderByStartDate(int page, int pageSize, string?name, DateTime startDate, DateTime endDate)
        {
            try
            {

                List<Event> events = _repository.SearchEventOrderByStartDate(page, pageSize, name, startDate, endDate);
                List<EventBasicVMSpectator> eventBasicVMs = new List<EventBasicVMSpectator>();
                foreach (var item in events)
                {
                    EventBasicVMSpectator eventBasicVM = new EventBasicVMSpectator
                    {
                        Id = item.Id,
                        CampusId = item.CampusId,
                        CampusDTO = item.Campus == null ? null : new DTOs.Campus.CampusDTO
                        {
                            Id = item.Campus.Id,
                            CampusName = item.Campus.CampusName,
                            Status = item.Campus.Status
                        },
                        CategoryEventId = item.CategoryEventId,
                        CategoryViewModel = item.CategoryEvent == null ? null : new DTOs.Categories.CategoryViewModel
                        {
                            Id = item.CategoryEvent.Id,
                            CategoryEventName = item.CategoryEvent.CategoryEventName
                        },
                        Status = item.Status,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        EventDescription = item.EventDescription,
                        EventTitle = item.EventTitle,
                        IsPublic = item.IsPublic,
                        Placed = item.Placed,
                        EventMedias = item.EventMedia == null ? null : item.EventMedia.Select(em => new DTOs.Medias.EventMediumViewMediaModel
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
            }catch(Exception ex)
            {
                Console.WriteLine("event spectator - searchEvent: " + ex.Message);
                return new List<EventBasicVMSpectator>();
            }
        }
    }
}
