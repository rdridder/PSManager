namespace PSDTO.Process
{
    public class ProcessListDTO
    {
        public ProcessListDTO(long id, string name, bool isReplayable)
        {
            Id = id;
            Name = name;
            IsReplayable = isReplayable;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsReplayable { get; set; }

        public string Status { get; set; }
    }
}
