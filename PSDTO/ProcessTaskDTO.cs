namespace PSDTO
{
    public class ProcessTaskDTO
    {
        public ProcessTaskDTO(long id, string name, string key)
        {
            Id = id;
            Name = name;
            Key = key;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }
    }
}
