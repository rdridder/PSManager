using Azure.Messaging.ServiceBus;
using PSDTO.Messaging;
using PSInterfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSAZServiceBus.Services
{
    public class MessageService : IMessageService<ServiceBusReceivedMessage, ServiceBusMessage>
    {
        private ServiceBusClient _client;

        public MessageService(ServiceBusClient client)
        {
            _client = client;
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

        public Task SendMessage(MessageDTO message, string queueName)
        {
            throw new System.NotImplementedException();
        }
    }
}
