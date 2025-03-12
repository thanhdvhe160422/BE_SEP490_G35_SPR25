namespace Planify_BackEnd.Hub
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class EventRequestHub : Hub
    {
        public async Task SendRequest(string message)
        {
            await Clients.All.SendAsync("ReceiveEventRequest", message);
        }
    }
}
