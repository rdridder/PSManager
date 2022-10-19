using System.Collections.Generic;

namespace PSDTO.Messaging
{
    public class MessageDTO
    {
        public MessageDTO(string correlationId, Dictionary<string, string> messageBody)
        {
            CorrelationId = correlationId;
            MessageBody = messageBody;
        }

        public string CorrelationId { get; }

        public Dictionary<string, string> MessageBody { get; }
    }
}
