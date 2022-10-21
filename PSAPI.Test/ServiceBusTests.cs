using PSDTO.Constants;
using PSDTO.Process;
using System.Text.Json;

namespace PSAPI.Test
{
    public class ServiceBusTests : DatabaseTestBase
    {
        public ServiceBusTests() : base()
        {
        }

        [Fact]
        async Task TestStartAZTask()
        {
            using (var context = CreateContext())
            {
                var processName = "Process name 1";
                var psController = CreateController(context);
                var start = new StartProcessDTO(processName);
                var actionResult = await psController.StartProcess(start);
                var startObject = actionResult.GetObjectResult();
                var continueProcess = new ContinueProcessDTO(startObject.Id);
                await psController.ContinueProcess(continueProcess);
                var message = _serviceBus.GetMessage();
                message.CorrelationId.ShouldBe(startObject.Id.ToString());
                message.ContentType.ShouldBe(DTOConstants.MESS_CONTENT_TYPE);

                var actionResultProcess = await psController.GetProcess(startObject.Id);
                var startObjectProcess = actionResultProcess.GetObjectResult();

                var a = message.Body.ToString();
                var body = JsonSerializer.Deserialize<Dictionary<string, string>>(message.Body.ToString());
                body[DTOConstants.MESS_TASK_ID_KEY].ShouldBe(startObjectProcess.ProcessTasks.Select(x => x.Id).First().ToString());
            }
        }
    }
}