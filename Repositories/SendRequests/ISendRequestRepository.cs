using Planify_BackEnd.Models;

namespace Planify_BackEnd.Repositories.SendRequests
{
    public interface ISendRequestRepository
    {
        Task<List<SendRequest>> GetRequestsAsync();
        Task<SendRequest> GetRequestByIdAsync(int id);
        Task<SendRequest> CreateRequestAsync(SendRequest request);
        System.Threading.Tasks.Task UpdateRequestAsync(SendRequest request);
    }
}
