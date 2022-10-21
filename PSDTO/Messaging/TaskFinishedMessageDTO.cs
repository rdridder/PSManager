namespace PSDTO.Messaging
{
    public class TaskFinishedMessageDTO : MessageDTO
    {
        public TaskFinishedMessageDTO(long processId, long taskId, string taskStatus) : base(processId.ToString())
        {
            MessageBody[Constants.DTOConstants.MESS_TASK_ID_KEY] = taskId.ToString();
            MessageBody[Constants.DTOConstants.MESS_TASK_STATUS_KEY] = taskStatus;
        }

        public long TaskId
        {
            get
            {
                return long.Parse(MessageBody[Constants.DTOConstants.MESS_TASK_ID_KEY]);
            }
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
