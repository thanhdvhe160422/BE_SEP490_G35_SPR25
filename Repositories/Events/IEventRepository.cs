﻿using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEvent();
        Task<Event> CreateEventAsync(Event newEvent);

        System.Threading.Tasks.Task CreateMediaItemAsync(MediaItem mediaItem);
        System.Threading.Tasks.Task AddEventMediaAsync(EventMedium eventMedia);
    }
}
