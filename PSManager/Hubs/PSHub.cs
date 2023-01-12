using Microsoft.AspNetCore.SignalR;

namespace PSManager.Hubs
{
    public class PSHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
