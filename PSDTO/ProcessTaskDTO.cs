namespace PSDTO
{
    public class ProcessTaskDTO
    {
        public ProcessTaskDTO(long id, int order, string name, string key)
        {
            Id = id;
            Order = order;
            Name = name;
            Key = key;
        }

        public long Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Status { get; set; }
    }
}
