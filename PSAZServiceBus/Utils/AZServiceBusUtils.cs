using Azure.Messaging.ServiceBus;
using PSDTO.Constants;
using PSDTO.Messaging;
using System.Text.Json;

namespace PSAZServiceBus.Utils
{
    public static class AZServiceBusUtils
    {
        public static ServiceBusMessage ConvertDTOToMessage<T>(T messageDTO) where T : MessageDTO
        {
            var body = JsonSerializer.Serialize(messageDTO.MessageBody);
            var message = new ServiceBusMessage(body)
            {
                CorrelationId = messageDTO.CorrelationId,
                ContentType = DTOConstants.MESS_CONTENT_TYPE
            };
            return message;
        }
    }
}
