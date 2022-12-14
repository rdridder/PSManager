using PSAPI.Controllers;
using PSDTO.Process;
using PSInterfaces;

namespace PSAPI.Test.Mock
{
    public class MockPSAPIClient : IPSAPIClient
    {
        private PSController _controller;

        public MockPSAPIClient(PSController controller)
        {
            _controller = controller;
        }

        public async Task<ProcessStatusDTO> FinishProcessTaskAsync(FinishProcessTaskDTO finishProcessTask)
        {
            var output = await _controller.FinishProcessTask(finishProcessTask);
            var outputObject = output.GetObjectResult();
            return outputObject;
        }

        public async Task<ProcessDTO> GetProcessAsync(long processId)
        {
            var output = await _controller.GetProcess(processId);
            var outputObject = output.GetObjectResult();
            return outputObject;
        }
    }
}
