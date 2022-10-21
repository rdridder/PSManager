using System.ComponentModel.DataAnnotations;

namespace PSDTO.ProcessDefinition
{
    public class ProcessDefinitionCreateDTO
    {
        public ProcessDefinitionCreateDTO(string name, string description, bool isEnabled, bool isReplayable)
        {
            Name = name;
            Description = description;
            IsEnabled = isEnabled;
            IsReplayable = isReplayable;
        }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string Description { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public bool IsReplayable { get; set; }
    }
}
