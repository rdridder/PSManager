using Microsoft.AspNetCore.SignalR;

namespace PSAPI.Hubs
{
    public class ProcessMessageHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
