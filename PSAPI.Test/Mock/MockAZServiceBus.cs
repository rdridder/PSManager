using Azure.Messaging.ServiceBus;

namespace PSAPI.Test.Mock
{
    public class MockAZServiceBus
    {
        public Queue<ServiceBusMessage> _queue;

        public MockAZServiceBus()
        {
            _queue = new Queue<ServiceBusMessage>();
        }

        public void addMessage(ServiceBusMessage message)
        {
            _queue.Enqueue(message);
        }

        public ServiceBusReceivedMessage GetMessage()
        {
            var message = _queue.Dequeue();
            var receivedMesssage = ServiceBusModelFactory.ServiceBusReceivedMessage(message.Body, message.MessageId, message.PartitionKey, null, message.SessionId, message.ReplyToSessionId, message.TimeToLive, message.CorrelationId, message.Subject, message.To, message.ContentType, message.ReplyTo, message.ScheduledEnqueueTime, message.ApplicationProperties, Guid.Empty, 0, default, -1, null, 0, default, ServiceBusMessageState.Active); ;
            return receivedMesssage;
        }
    }
}
