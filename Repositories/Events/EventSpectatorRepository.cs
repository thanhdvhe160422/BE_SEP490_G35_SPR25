﻿using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Events
{
    public class EventSpectatorRepository : IEventSpectatorRepository
    {
        private readonly PlanifyContext _context;
        public EventSpectatorRepository(PlanifyContext context)
        {
            _context = context;
        }
        public Event GetEventById(int id)
        {
            return _context.Events
                .Include(e=>e.Campus)
                .Include(e=>e.CategoryEvent)
                .Include(e=>e.EventMedia)
                .ThenInclude(e=>e.Media)
                .FirstOrDefault(em => em.Id == id);
        }

        public PageResultDTO<Event> GetEvents(int page, int pageSize, Guid userId)
        {
            try
            {
                var count = _context.Events
                    .Include(e => e.Campus)
                    .Include(e => e.CategoryEvent)
                    .Include(e => e.EventMedia).ThenInclude(em => em.Media)
                    .Include(e => e.FavouriteEvents)
                    .Where(e => e.Status != -1 && e.IsPublic == 1)
                    .Where(e => e.FavouriteEvents.Any(fe => fe.UserId == userId) || !e.FavouriteEvents.Any())
                    .AsEnumerable()
                    .OrderBy(e =>
                        e.StartTime <= DateTime.Now && DateTime.Now <= e.EndTime ? 0 : 1)
                    .ThenBy(e =>
                        e.StartTime > DateTime.Now ? 0 : 1)
                    .ThenBy(e =>
                        e.EndTime < DateTime.Now ? 0 : 1)
                    .Count();
                if (count == 0) new PageResultDTO<Event>(new List<Event>(), count, page, pageSize);
                var events = _context.Events
                    .Include(e => e.Campus)
                    .Include(e => e.CategoryEvent)
                    .Include(e => e.EventMedia).ThenInclude(em => em.Media)
                    .Include(e => e.FavouriteEvents)
                    .Where(e => e.Status != -1 && e.IsPublic == 1)
                    .Where(e => e.FavouriteEvents.Any(fe => fe.UserId == userId) || !e.FavouriteEvents.Any())
                    .AsEnumerable()
                    .OrderBy(e =>
                        e.StartTime <= DateTime.Now && DateTime.Now <= e.EndTime ? 0 : 1)
                    .ThenBy(e =>
                        e.StartTime > DateTime.Now ? 0 : 1)
                    .ThenBy(e =>
                        e.EndTime < DateTime.Now ? 0 : 1)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                PageResultDTO<Event> result = new PageResultDTO<Event>(events, count, page, pageSize);
                return result;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PageResultDTO<Event> SearchEvent(int page, int pageSize, string? name, DateTime? startDate, DateTime? endDate, string? placed,Guid userId)
        {
            try
            {
                var count = _context.Events
                    .Include(e => e.Campus)
                    .Include(e => e.CategoryEvent)
                    .Include(e => e.EventMedia).ThenInclude(em => em.Media)
                    .Include(e => e.FavouriteEvents)
                    .Where(e => e.Status != -1 && e.IsPublic == 1)
                    .Where(e => string.IsNullOrEmpty(name) || e.EventTitle.Contains(name))
                    .Where(e => !startDate.HasValue || e.StartTime >= startDate.Value)
                    .Where(e => !endDate.HasValue || e.EndTime <= endDate.Value)
                    .Where(e => string.IsNullOrEmpty(placed) || e.Placed.Contains(placed))
                    .Where(e => e.FavouriteEvents.Any(fe => fe.UserId == userId) || !e.FavouriteEvents.Any())
                    .AsEnumerable()
                    .OrderBy(e =>
                        e.StartTime <= DateTime.Now && DateTime.Now <= e.EndTime ? 0 : 1)
                    .ThenBy(e =>
                        e.StartTime > DateTime.Now ? 0 : 1)
                    .ThenBy(e =>
                        e.EndTime < DateTime.Now ? 0 : 1).Count();
                if (count == 0) return new PageResultDTO<Event>(new List<Event>(),0, page, pageSize);
                var events = _context.Events
                    .Include(e => e.Campus)
                    .Include(e => e.CategoryEvent)
                    .Include(e => e.EventMedia).ThenInclude(em => em.Media)
                    .Include(e => e.FavouriteEvents)
                    .Where(e => e.Status != -1 && e.IsPublic == 1)
                    .Where(e => string.IsNullOrEmpty(name) || e.EventTitle.Contains(name))
                    .Where(e => !startDate.HasValue || e.StartTime >= startDate.Value)
                    .Where(e => !endDate.HasValue || e.EndTime <= endDate.Value)
                    .Where(e => string.IsNullOrEmpty(placed) || e.Placed.Contains(placed))
                    .Where(e => e.FavouriteEvents.Any(fe => fe.UserId == userId) || !e.FavouriteEvents.Any())
                    .AsEnumerable()
                    .OrderBy(e =>
                        e.StartTime <= DateTime.Now && DateTime.Now <= e.EndTime ? 0 : 1)
                    .ThenBy(e =>
                        e.StartTime > DateTime.Now ? 0 : 1)
                    .ThenBy(e =>
                        e.EndTime < DateTime.Now ? 0 : 1)
                    .Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return new PageResultDTO<Event>(events, count, page, pageSize);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
