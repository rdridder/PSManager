using PSDTO.Messaging;

namespace PSInterfaces
{
    public interface IMessageService
    {
        public Task SendStartTaskMessage(StartTaskMessageDTO messageDTO, string queueName);

        public Task SendTaskFinishedMessage(TaskFinishedMessageDTO messageDTO, string queueName);

        //public Task<MessageDTO> ConvertMessageToDTO(MessageToDTO message);

        //public Task<DTOToMessage> ConvertDTOToMessage(MessageDTO messageDTO);
    }
}
