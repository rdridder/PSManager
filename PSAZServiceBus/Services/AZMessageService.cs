using Azure.Messaging.ServiceBus;
using PSAZServiceBus.Utils;
using PSDTO.Messaging;
using PSInterfaces;
using System.Threading.Tasks;

namespace PSAZServiceBus.Services
{
    public class AZMessageService : IMessageService
    {
        private readonly IMessageBusFactory<ServiceBusSender> _factory;

        public AZMessageService(IMessageBusFactory<ServiceBusSender> factory)
        {
            _factory = factory;
        }

        public Task SendMessage(MessageDTO messageDTO, string queueName)
        {
            return SendTaskFinishedMessage(messageDTO, queueName);
        }

        public Task SendStartTaskMessage(StartTaskMessageDTO messageDTO, string queueName)
        {
            return SendTaskFinishedMessage(messageDTO, queueName);
        }

        public Task SendTaskFinishedMessage(TaskFinishedMessageDTO messageDTO, string queueName)
        {
            return SendTaskFinishedMessage(messageDTO, queueName);
        }

        private Task SendTaskFinishedMessage<T>(T messageDTO, string queueName) where T : MessageDTO
        {
            var sender = _factory.GetClient(queueName);
            var serviceBusMessage = AZServiceBusUtils.ConvertDTOToMessage(messageDTO);
            return sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
