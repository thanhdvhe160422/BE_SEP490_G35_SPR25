﻿using Microsoft.EntityFrameworkCore;
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

    }
}
