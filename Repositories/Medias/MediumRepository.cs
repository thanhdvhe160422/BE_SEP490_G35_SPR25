using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.Medias
{
    public class MediumRepository: IMediumRepository
    {
        private readonly PlanifyContext _context;
        public MediumRepository(PlanifyContext context)
        {
            _context = context;
        }
        public async Task<Medium> CreateMediaItemAsync(Medium mediaItem)
        {
            try
            {
                _context.Media.Add(mediaItem);
                await _context.SaveChangesAsync();
                return mediaItem;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> DeleteMediaEvent(int mediaEventId)
        {
            try
            {
                var mediaEvent = _context.EventMedia.FirstOrDefault(em => em.Id == mediaEventId);
                mediaEvent.Status = 0;
                _context.EventMedia.Update(mediaEvent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<EventMedium>> GetDeleteMediaEvent(int eventId, List<EventMedium> list)
        {
            try
            {
                var listMediaEvent = await _context.EventMedia.Where(em=>em.EventId==eventId).ToListAsync();
                var deletedMedia = listMediaEvent
                    .Where(media => !list.Any(l => l.Id == media.Id) && media.Status != 0)
                    .ToList();
                return deletedMedia;
            }
            catch
            {
                throw new Exception("Error while get deleted media");
            }
        }
    }
}
