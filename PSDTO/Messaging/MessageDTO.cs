using System.Collections.Generic;

namespace PSDTO.Messaging
{
    public abstract class MessageDTO
    {
        public MessageDTO(string correlationId)
        {
            CorrelationId = correlationId;
            MessageBody = new Dictionary<string, string>();
        }

        public string CorrelationId { get; }

        public Dictionary<string, string> MessageBody { get; }
    }
}
