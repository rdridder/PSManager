namespace PSDTO
{
    public class AddTaskIdAndOrderToProcessDefinitionDTO
    {
        public AddTaskIdAndOrderToProcessDefinitionDTO(long id, int order)
        {
            Id = id;
            Order = order;
        }

        public long Id { get; set; }

        public int Order { get; set; }
    }
}
