using Planify_BackEnd.DTOs.SendRequests;
using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.SendRequests
{
    public interface ISendRequestRepository
    {
        Task<List<SendRequestWithEventDTO>> GetRequestsAsync();
        Task<SendRequest> GetRequestByIdAsync(int id);
        Task<SendRequest> CreateRequestAsync(SendRequest request);
        System.Threading.Tasks.Task UpdateRequestAsync(SendRequest request);
        Task<List<SendRequest>> GetRequestsByUserIdAsync(Guid userId);
    }
}
