using System.ComponentModel.DataAnnotations;

namespace PSDTO
{
    public class StatusDTO
    {
        public long Id { get; set; }

        [MaxLength(16)]
        public string Name { get; set; }
    }
}
