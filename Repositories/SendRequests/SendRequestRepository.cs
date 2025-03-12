using Microsoft.EntityFrameworkCore;
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

        public async Task<List<SendRequest>> GetRequestsAsync()
        {
            return await _context.SendRequests.ToListAsync();
        }

        public async Task<SendRequest> GetRequestByIdAsync(int id)
        {
            return await _context.SendRequests.FindAsync(id);
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
    }
}
