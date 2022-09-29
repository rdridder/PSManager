using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class ProcessTask
    {
        public long Id { get; set; }

        public int Order { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        [MaxLength(32)]
        public string Key { get; set; }
    }
}
