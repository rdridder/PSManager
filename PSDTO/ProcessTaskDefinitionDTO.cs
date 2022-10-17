namespace PSDTO
{
    public class ProcessTaskDefinitionDTO
    {
        public ProcessTaskDefinitionDTO(long id, string name, string description, string key, bool isEnabled, string processTaskType)
        {
            Id = id;
            Name = name;
            Description = description;
            Key = key;
            IsEnabled = isEnabled;
            ProcessTaskType = processTaskType;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public bool IsEnabled { get; set; }

        public string ProcessTaskType { get; set; }
    }
}
