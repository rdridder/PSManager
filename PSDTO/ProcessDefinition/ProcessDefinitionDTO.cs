using System.Collections.Generic;

namespace PSDTO.ProcessDefinition
{
    public class ProcessDefinitionDTO
    {
        public ProcessDefinitionDTO(long id, string name, string description, bool isEnabled, bool isReplayable, List<ProcessTaskDefinitionDTO> processTaskDefinitions)
        {
            Id = id;
            Name = name;
            Description = description;
            IsEnabled = isEnabled;
            IsReplayable = isReplayable;
            ProcessTaskDefinitions = processTaskDefinitions;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsReplayable { get; set; }

        public List<ProcessTaskDefinitionDTO> ProcessTaskDefinitions { get; set; }
    }
}
