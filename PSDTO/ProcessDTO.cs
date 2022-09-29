using System.Collections.Generic;

namespace PSDTO
{
    public class ProcessDTO : ProcessListDTO
    {
        public ProcessDTO(long id, string name, bool isReplayable, List<ProcessTaskDTO> processTasks) : base(id, name, isReplayable)
        {
            ProcessTasks = processTasks;
        }
        public List<ProcessTaskDTO> ProcessTasks { get; set; }
    }
}
