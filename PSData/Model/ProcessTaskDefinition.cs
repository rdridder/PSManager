using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    [Index(nameof(Key), IsUnique = true)]
    public class ProcessTaskDefinition
    {
        public long Id { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Description { get; set; }

        [MaxLength(32)]
        public string Key { get; set; }

        public bool IsEnabled { get; set; }

        public virtual List<ProcessDefinitionTaskDefinition> ProcessDefinitionTaskDefinitions { get; set; }

        public ProcessTaskType ProcessTaskType { get; set; }
    }
}
