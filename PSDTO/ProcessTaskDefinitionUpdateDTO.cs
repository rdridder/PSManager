using System.ComponentModel.DataAnnotations;

namespace PSDTO
{
    public class ProcessTaskDefinitionUpdateDTO : ProcessTaskDefinitionCreateDTO
    {
        public ProcessTaskDefinitionUpdateDTO(long id, string name, string description, string key, bool isEnabled)
            : base(name, description, key, isEnabled)
        {
            Id = id;
        }

        [Required]
        public long Id { get; set; }
    }
}
