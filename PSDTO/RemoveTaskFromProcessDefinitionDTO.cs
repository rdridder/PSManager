using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSDTO
{
    public class RemoveTaskFromProcessDefinitionDTO
    {
        public RemoveTaskFromProcessDefinitionDTO(long processId, List<long> taskIds)
        {
            ProcessId = processId;
            TaskIds = taskIds;
        }

        [Required]
        public long ProcessId { get; set; }

        [Required]
        public List<long> TaskIds { get; set; }
    }
}
