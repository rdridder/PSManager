using System.ComponentModel.DataAnnotations;

namespace PSDTO
{
    public class StartProcessDTO
    {
        public StartProcessDTO(string name)
        {
            Name = name;
        }

        [Required]
        [StringLength(32)]
        public string Name { get; set; }
    }
}
