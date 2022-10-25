using PSDTO.Enums;
using System.ComponentModel.DataAnnotations;

namespace PSDTO.Process
{
    public class FinishProcessTaskDTO
    {
        public FinishProcessTaskDTO(long processId, long taskId, SetTaskStatusEnum status)
        {
            ProcessId = processId;
            TaskId = taskId;
            Status = status;
        }

        [Required]
        public long ProcessId { get; set; }

        [Required]
        public long TaskId { get; set; }

        [Required]
        public SetTaskStatusEnum Status { get; set; }

    }
}
