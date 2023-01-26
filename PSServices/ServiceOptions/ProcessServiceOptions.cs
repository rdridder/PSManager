namespace PSServices.ServiceOptions
{
    public class ProcessServiceOptions
    {
        public const string ProcessService = "ProcessService";
        public bool SignalREnabled { get; set; } = false;
        public int PageSize { get; set; } = 10;
    }
}
