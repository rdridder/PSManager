using System.ComponentModel.DataAnnotations;

namespace PSDTO.ProcessDefinition
{
    public class ProcessDefinitionUpdateDTO : ProcessDefinitionCreateDTO
    {
        public ProcessDefinitionUpdateDTO(long id, string name, string description, bool isEnabled, bool isReplayable)
            : base(name, description, isEnabled, isReplayable)
        {
            Id = id;
        }

        [Required]
        public long Id { get; set; }

    }
}
