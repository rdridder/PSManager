using System.ComponentModel.DataAnnotations;

namespace PSDTO.Process
{
    public class ContinueProcessDTO
    {
        public ContinueProcessDTO(long processId)
        {
            ProcessId = processId;
        }

        [Required]
        public long ProcessId { get; set; }
    }
}
