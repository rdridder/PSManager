namespace PSDTO
{
    public class ProcessTaskDTO
    {
        public ProcessTaskDTO(long id, int order, string name, string key, string processTaskType)
        {
            Id = id;
            Order = order;
            Name = name;
            Key = key;
            ProcessTaskType = processTaskType;
        }

        public long Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Status { get; set; }

        public string ProcessTaskType { get; set; }
    }
}
