using Microsoft.AspNetCore.SignalR.Client;
using PSDTO.Enums;
using PSInterfaces;
using System;
using System.Threading.Tasks;

namespace PSServices
{
    public class SignalRNotifierService : INotifierService
    {
        private readonly HubConnection _hubConnection;

        public SignalRNotifierService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(new Uri("https://localhost:7230/pshub"))
                .WithAutomaticReconnect()
                .Build();
            _hubConnection.StartAsync().Wait();
        }

        public Task SendMessageAsync(NotifierEnum action, string message)
        {
            return _hubConnection.InvokeAsync("SendMessage", "Ron", message);
        }
    }
}
