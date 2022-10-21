using System.ComponentModel.DataAnnotations;

namespace PSDTO.ProcessDefinition
{
    public class ProcessTaskDefinitionUpdateDTO : ProcessTaskDefinitionCreateDTO
    {
        public ProcessTaskDefinitionUpdateDTO(long id, string name, string description, string key, bool isEnabled, string processTaskType)
            : base(name, description, key, isEnabled, processTaskType)
        {
            Id = id;
        }

        [Required]
        public long Id { get; set; }
    }
}
