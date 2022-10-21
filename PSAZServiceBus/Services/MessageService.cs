using Azure.Messaging.ServiceBus;
using PSDTO.Messaging;
using PSInterfaces;
using PSServices;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSAZServiceBus.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageBusFactory<ServiceBusSender> _factory;

        public MessageService(IMessageBusFactory<ServiceBusSender> factory)
        {
            _factory = factory;
        }

        public async Task<MessageDTO> ConvertMessageToDTO(ServiceBusReceivedMessage message)
        {
            using var stream = message.Body.ToStream();
            var body = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
            return new MessageDTO(message.CorrelationId, body);
        }

        public async Task<ServiceBusMessage> ConvertDTOToMessage(MessageDTO messageDTO)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, messageDTO.MessageBody);
            using var reader = new StreamReader(stream);
            var message = new ServiceBusMessage(await reader.ReadToEndAsync())
            {
                CorrelationId = messageDTO.CorrelationId,
                ContentType = "application/json;charset=utf-8"
            };
            return message;
        }

        public async Task SendMessage(MessageDTO message, string queueName)
        {
            var sender = _factory.GetClient(queueName);
            var serviceBusMessage = await ConvertDTOToMessage(message);
            await sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
