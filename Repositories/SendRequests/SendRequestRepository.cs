﻿using Microsoft.EntityFrameworkCore;
using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Models;
using System;

namespace Planify_BackEnd.Repositories.SendRequests
{
    public class SendRequestRepository : ISendRequestRepository
    {
        private readonly PlanifyContext _context;

        public SendRequestRepository(PlanifyContext context)
        {
            _context = context;
        }

        public async Task<List<SendRequestWithEventDTO>> GetRequestsAsync()
        {
            return await _context.SendRequests
                .Include(sr => sr.Event)
                .Select(sr => new SendRequestWithEventDTO
                {
                    Id = sr.Id,
                    EventId = sr.EventId,
                    Reason = sr.Reason,
                    Status = sr.Event.Status,
                    ManagerId = sr.ManagerId,
                    EventTitle = sr.Event.EventTitle,
                    EventStartTime = sr.Event.StartTime,
                    EventEndTime = sr.Event.EndTime
                })
                .ToListAsync();
        }

        public async Task<SendRequest> GetRequestByIdAsync(int id)
        {
            try
            {
                return await _context.SendRequests
                    .Include(sr => sr.Event).FirstOrDefaultAsync(sr => sr.Id == id);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SendRequest> CreateRequestAsync(SendRequest request)
        {
            _context.SendRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async System.Threading.Tasks.Task UpdateRequestAsync(SendRequest request)
        {
            _context.SendRequests.Update(request);
            await _context.SaveChangesAsync();
        }
        public async Task<List<SendRequest>> GetRequestsByUserIdAsync(Guid userId, int campusId)
        {
            return await _context.SendRequests
                .Include(sr => sr.Event)
                .Where(sr => sr.Event.CreateBy == userId &&
                    sr.Event.CampusId == campusId)
                .ToListAsync();
        }
    }
}
