using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Process
    {
        public long Id { get; set; }

        [MaxLength(32)]
        public string Name { get; set; }

        public bool IsReplayable { get; set; }

        public virtual List<ProcessTask> ProcessTasks { get; set; }
    }
}
