using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.FavouriteEvents
{
    public class FavouriteEventRepository : IFavouriteEventRepository
    {
        private readonly PlanifyContext _context;
        public FavouriteEventRepository(PlanifyContext context)
        {
            _context = context;
        }

        public PageResultDTO<FavouriteEvent> GetAllFavouriteEventsAsync(int page, int pageSize, Guid spectatorId)
        {
            try
            {
                var count = _context.FavouriteEvents.Count();
                if (count == 0)
                    return new PageResultDTO<FavouriteEvent>(new List<FavouriteEvent>(), count, page, pageSize);
                var f = _context.FavouriteEvents
                    .Include(f => f.Event)
                    .Include(f => f.User)
                    .Where(f => f.UserId == spectatorId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                PageResultDTO<FavouriteEvent> result = new PageResultDTO<FavouriteEvent>(f, count, page, pageSize);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while retrieving favourite events.", ex);
            }
        }
        public async Task<FavouriteEvent> CreateFavouriteEventAsync(FavouriteEvent favouriteEvent)
        {
            try
            {
                await _context.FavouriteEvents.AddAsync(favouriteEvent);
                await _context.SaveChangesAsync();
                return favouriteEvent;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the favourite event.", ex);
            }

        }
        public async Task<bool> DeleteFavouriteEventAsync(int eventId, Guid spectatorId)
        {
            try
            {
                var fevent = await _context.FavouriteEvents.FirstOrDefaultAsync(f => f.EventId == eventId && f.UserId == spectatorId);

                if (fevent == null)
                {
                    return false;
                }
                _context.FavouriteEvents.Remove(fevent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}
