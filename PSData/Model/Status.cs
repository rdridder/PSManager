using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Status
    {
        public long Id { get; set; }

        [MaxLength(16)]
        public string Name { get; set; }
    }
}
