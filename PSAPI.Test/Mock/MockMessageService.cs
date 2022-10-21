using Azure.Messaging.ServiceBus;
using PSDTO.Messaging;
using PSInterfaces;
using System.Text.Json;

namespace PSAPI.Test.Mock
{
    public class MockMessageService : IMessageService
    {
        private readonly MockAZServiceBus _serviceBus;

        public MockMessageService(MockAZServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public async Task SendMessage(MessageDTO messageDTO, string queueName)
        {
            var message = await ConvertDTOToMessage(messageDTO);
            _serviceBus.addMessage(message);
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
    }
}
