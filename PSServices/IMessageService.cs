using PSDTO.Messaging;
using System.Threading.Tasks;

namespace PSInterfaces
{
    public interface IMessageService<MessageToDTO, DTOToMessage>
    {
        public Task SendMessage(MessageDTO messageDTO, string queueName);

        public Task<MessageDTO> ConvertMessageToDTO(MessageToDTO message);

        public Task<DTOToMessage> ConvertDTOToMessage(MessageDTO messageDTO);
    }
}
