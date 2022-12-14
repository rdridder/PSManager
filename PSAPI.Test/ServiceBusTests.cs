using Azure.Messaging.ServiceBus;
using PSAPI.Controllers;
using PSAPI.Test.Mock;
using PSAZServiceBus;
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

                // Get the start process message from the bus
                var message = _serviceBus.GetMessage();
                message.CorrelationId.ShouldBe(processId.ToString());
                message.ContentType.ShouldBe(DTOConstants.MESS_CONTENT_TYPE);

                // Get the process from the controller
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
                // Create a task finished message and send it to the bus
                var taskStatus = StatusEnum.success;
                var tuple = await SetupBusTaskCommunication(context, taskStatus);
                var processObject = tuple.processDTO;
                var openStatus = StatusEnum.open.ToString();
                var runningStatus = StatusEnum.running.ToString();
                processObject.ProcessTasks[0].Status.ShouldBe(taskStatus.ToString());
                processObject.ProcessTasks[1].Status.ShouldBe(openStatus);
                processObject.Status.ShouldBe(runningStatus);
            }
        }

        [Fact]
        async Task TestFinishProcess()
        {
            using (var context = CreateContext())
            {
                // Create a task finished message and send it to the bus
                var taskStatus = StatusEnum.success;
                var tuple = await SetupBusTaskCommunication(context, taskStatus);
                var processObject = tuple.processDTO;
                var psController = tuple.psController;

                // Continue the process
                var continueProcess = new ContinueProcessDTO(processObject.Id);
                await psController.ContinueProcess(continueProcess);

                // Get the start task message from the bus
                var message = _serviceBus.GetMessage();

                // Finish the started task
                await RunTaskFinishedFunction(processObject.Id, processObject.ProcessTasks[1].Id, taskStatus, psController);

                var process = await psController.GetProcess(processObject.Id);
                processObject = process.GetObjectResult();

                var openStatus = StatusEnum.open.ToString();
                var runningStatus = StatusEnum.running.ToString();
                processObject.ProcessTasks[0].Status.ShouldBe(taskStatus.ToString());
                processObject.ProcessTasks[1].Status.ShouldBe(taskStatus.ToString());
                processObject.Status.ShouldBe(taskStatus.ToString());
            }
        }

        [Fact]
        async Task TestFailAZTask()
        {
            using (var context = CreateContext())
            {
                // Create a task failed message and send it to the bus
                var taskStatus = StatusEnum.failed;
                var tuple = await SetupBusTaskCommunication(context, taskStatus);
                var processObject = tuple.processDTO;
                var openStatus = StatusEnum.open.ToString();
                processObject.ProcessTasks[0].Status.ShouldBe(taskStatus.ToString());
                processObject.ProcessTasks[1].Status.ShouldBe(openStatus);
                processObject.Status.ShouldBe(taskStatus.ToString());
            }
        }

        private async Task<(ProcessDTO processDTO, PSController psController)> SetupBusTaskCommunication(ProcessContext context, StatusEnum taskStatus)
        {
            var psController = CreateController(context);
            var processId = await StartAndContinueProcess(context);

            // Get the start process message from the bus
            var message = _serviceBus.GetMessage();
            var body = DeserializeBody(message);
            var taskId = long.Parse(body[DTOConstants.MESS_TASK_ID_KEY]);

            // Create a task finished message and send it to the bus
            await RunTaskFinishedFunction(processId, taskId, taskStatus, psController);

            var process = await psController.GetProcess(processId);
            var processObject = process.GetObjectResult();
            return (processObject, psController);
        }

        private async Task RunTaskFinishedFunction(long processId, long taskId, StatusEnum taskStatus, PSController psController)
        {
            var apiClient = new MockPSAPIClient(psController);

            // Create a task finished message and send it to the bus
            var taskFinishedMessage = new TaskFinishedMessageDTO(processId, taskId, taskStatus.ToString());
            await _messageService.SendTaskFinishedMessage(taskFinishedMessage, "taskFinished");

            // Get the message from the bus and run the task finished function
            var taskFinishedBus = _serviceBus.GetMessage();
            TaskFinished taskFinishedFunction = new TaskFinished(apiClient, new MockLogger<TaskFinished>());

            await taskFinishedFunction.RunAsync(taskFinishedBus);
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