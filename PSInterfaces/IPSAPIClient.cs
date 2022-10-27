using PSDTO.Process;

namespace PSInterfaces
{
    public interface IPSAPIClient
    {
        Task<ProcessStatusDTO> FinishProcessTaskAsync(FinishProcessTaskDTO finishProcessTask);

        Task<ProcessDTO> GetProcessAsync(long processId);
    }
}
