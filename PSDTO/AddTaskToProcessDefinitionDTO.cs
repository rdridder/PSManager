using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSDTO
{
    public class AddTaskToProcessDefinitionDTO
    {
        public AddTaskToProcessDefinitionDTO(long processId, List<AddTaskIdAndOrderToProcessDefinitionDTO> taskIds)
        {
            ProcessId = processId;
            TaskIds = taskIds;
        }

        [Required]
        public long ProcessId { get; set; }

        [Required]
        public List<AddTaskIdAndOrderToProcessDefinitionDTO> TaskIds { get; set; }
    }
}
