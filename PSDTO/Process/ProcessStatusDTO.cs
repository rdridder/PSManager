using PSDTO.Enums;

namespace PSDTO.Process
{
    public class ProcessStatusDTO
    {
        public ProcessStatusDTO(long processId, StatusEnum status)
        {
            ProcessId = processId;
            Status = status;
        }

        public long ProcessId { get; set; }

        public StatusEnum Status { get; }
    }
}
