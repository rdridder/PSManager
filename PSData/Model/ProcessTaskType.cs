using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProcessTaskType
    {
        public long Id { get; set; }

        [MaxLength(16)]
        public string Name { get; set; }
    }
}
