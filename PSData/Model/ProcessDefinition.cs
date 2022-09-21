using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProcessDefinition
    {
        public long Id { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsReplayable { get; set; }

        public virtual List<ProcessDefinitionTaskDefinition> ProcessDefinitionTaskDefinitions { get; set; }
    }
}
