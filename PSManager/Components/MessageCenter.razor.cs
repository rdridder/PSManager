using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace PSManager.Components
{
    public partial class MessageCenter : IAsyncDisposable
    {
        private HubConnection? hubConnection;

        [Inject]
        private NavigationManager Navigation { get; set; }

        private Queue<string> messages = new Queue<string>();

        private KeyValue _keyValue = new KeyValue();

        protected override Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(Navigation.ToAbsoluteUri("/pshub"))
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (key, value) =>
            {
                var encodedMsg = $"{key}: {value}";
                if (messages.Count > 4)
                {
                    messages.Dequeue();
                }
                messages.Enqueue(encodedMsg);
                InvokeAsync(StateHasChanged);
            });

            return hubConnection.StartAsync();
        }

        private async Task Send()
        {
            if (hubConnection is not null)
            {
                await hubConnection.SendAsync("SendMessage", _keyValue.Key, _keyValue.Value);
            }
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }

    public class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
