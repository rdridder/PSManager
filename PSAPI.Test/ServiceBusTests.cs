using Azure.Messaging.ServiceBus;
using PSData.Context;
using PSDTO.Constants;
using PSDTO.Enums;
using PSDTO.Messaging;
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
                var psController = CreateController(context);
                var processId = await StartAndContinueProcess(context);
                var message = _serviceBus.GetMessage();
                message.CorrelationId.ShouldBe(processId.ToString());
                message.ContentType.ShouldBe(DTOConstants.MESS_CONTENT_TYPE);

                var actionResultProcess = await psController.GetProcess(processId);
                var startObjectProcess = actionResultProcess.GetObjectResult();
                var body = DeserializeBody(message);

                var taskId = body[DTOConstants.MESS_TASK_ID_KEY];
                taskId.ShouldBe(startObjectProcess.ProcessTasks.Select(x => x.Id).First().ToString());
            }
        }

        [Fact]
        async Task TestFinishAZTask()
        {
            using (var context = CreateContext())
            {
                var psController = CreateController(context);
                var processId = await StartAndContinueProcess(context);

                // Get the start process message from the bus
                var message = _serviceBus.GetMessage();
                var body = DeserializeBody(message);
                var taskId = long.Parse(body[DTOConstants.MESS_TASK_ID_KEY]);
                var taskStatus = StatusEnum.success.ToString();

                // Create a task finished message and send it to the bus
                var taskFinishedMessage = new TaskFinishedMessageDTO(processId, taskId, taskStatus);
                await _messageService.SendTaskFinishedMessage(taskFinishedMessage, "taskFinished");

                // Get the message from the bus and run the task finished function
                var taskFinishedBus = _serviceBus.GetMessage();
                //TaskFinished taskFinishedFunction = new TaskFinished(new MockLogger<TaskFinished>());

                //await taskFinishedFunction.Run(taskFinishedBus);

                if (true)
                {
                    //
                }

            }
        }

        private Dictionary<string, string> DeserializeBody(ServiceBusReceivedMessage message)
        {
            // TODO serializing/deserializing need to be generalized in a service.
            var body = JsonSerializer.Deserialize<Dictionary<string, string>>(message.Body.ToString());
            return body;
        }

        private async Task<long> StartAndContinueProcess(ProcessContext context)
        {
            var processName = "Process name 1";
            var psController = CreateController(context);
            var start = new StartProcessDTO(processName);
            var actionResult = await psController.StartProcess(start);
            var startObject = actionResult.GetObjectResult();
            var continueProcess = new ContinueProcessDTO(startObject.Id);
            await psController.ContinueProcess(continueProcess);
            return startObject.Id;
        }
    }
}