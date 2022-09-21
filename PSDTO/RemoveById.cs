using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PSDTO
{
    public class RemoveById
    {
        public RemoveById(List<long> ids)
        {
            Ids = ids;
        }

        [Required]
        public List<long> Ids { get; set; }
    }
}
