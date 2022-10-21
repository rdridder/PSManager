namespace PSDTO.Messaging
{
    public class StartTaskMessageDTO : MessageDTO
    {
        public StartTaskMessageDTO(long processId, long taskId) : base(processId.ToString())
        {
            MessageBody[Constants.DTOConstants.MESS_TASK_ID_KEY] = taskId.ToString();
        }

        public string TaskStatus
        {
            get
            {
                return MessageBody[Constants.DTOConstants.MESS_TASK_STATUS_KEY];
            }
        }
    }
}
