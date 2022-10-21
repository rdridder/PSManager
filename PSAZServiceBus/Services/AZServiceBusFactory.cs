using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using PSInterfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace PSAZServiceBus.Services
{
    public class AZServiceBusFactory : IMessageBusFactory<ServiceBusSender>, IAsyncDisposable
    {
        private readonly object _lockObject;

        private readonly ServiceBusClient _serviceBusClient;

        private readonly ILogger<AZServiceBusFactory> _log;

        private readonly ConcurrentDictionary<string, ServiceBusSender> _senders;

        public AZServiceBusFactory(ServiceBusClient serviceBusClient, ILogger<AZServiceBusFactory> log)
        {
            _serviceBusClient = serviceBusClient;
            _log = log;
            _senders = new ConcurrentDictionary<string, ServiceBusSender>();
            _lockObject = new object();
        }

        public ServiceBusSender GetClient(string senderName)
        {
            if (_senders.ContainsKey(senderName) && !_serviceBusClient.IsClosed)
            {
                return _senders[senderName];
            }

            lock (_lockObject)
            {
                // The lock is now applied, all thread access is blocked
                // Check again to make sure the sender was not created by another thread
                if (_senders.ContainsKey(senderName) && !_serviceBusClient.IsClosed)
                {
                    return _senders[senderName];
                }
                if (_serviceBusClient.IsClosed)
                {
                    // TODO Check what the best approach is.
                    // _serviceBusClient is injected so if closed not sure what to do.
                    _log.LogWarning("Service bus connection is closed.");
                }
                var sender = _serviceBusClient.CreateSender(senderName);
                _senders[senderName] = sender;
            }

            return _senders[senderName];
        }

        public async ValueTask DisposeAsync()
        {
            // Dispose all senders
            // The _serviceBusClient is injected so not managed by this instance
            foreach (var keyValue in _senders)
            {
                await keyValue.Value.DisposeAsync();
            }
        }
    }
}
