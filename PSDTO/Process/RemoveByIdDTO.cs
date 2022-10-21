using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSDTO.Process
{
    public class RemoveByIdDTO
    {
        public RemoveByIdDTO(List<long> ids)
        {
            Ids = ids;
        }

        [Required]
        public List<long> Ids { get; set; }
    }
}
