using PSDTO.Messaging;
using System.Threading.Tasks;
// TODO fix namespace
namespace PSInterfaces
{
    public interface IMessageService
    {
        public Task SendMessage(MessageDTO messageDTO, string queueName);

        //public Task<MessageDTO> ConvertMessageToDTO(MessageToDTO message);

        //public Task<DTOToMessage> ConvertDTOToMessage(MessageDTO messageDTO);
    }
}
