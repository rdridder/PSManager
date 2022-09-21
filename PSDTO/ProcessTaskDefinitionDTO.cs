namespace PSDTO
{
    public class ProcessTaskDefinitionDTO
    {
        public ProcessTaskDefinitionDTO(long id, string name, string description, string key, bool isEnabled)
        {
            Id = id;
            Name = name;
            Description = description;
            Key = key;
            IsEnabled = isEnabled;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public bool IsEnabled { get; set; }
    }
}
