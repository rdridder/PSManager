namespace Model
{
    public class ProcessDefinitionTaskDefinition
    {
        public long ProcessDefinitionId { get; set; }
        public ProcessDefinition ProcessDefinition { get; set; }
        public long ProcessTaskDefinitionId { get; set; }
        public ProcessTaskDefinition ProcessTaskDefinition { get; set; }
        public int Order { get; set; }
    }
}
