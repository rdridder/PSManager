using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PSDTO.Messaging;
using PSDTO.Process;
using PSInterfaces;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSAZServiceBus
{
    public class TaskFinished
    {
        private IPSAPIClient _pSAPIClient;

        private ILogger<TaskFinished> _logger;

        public TaskFinished(IPSAPIClient pSAPIClient, ILogger<TaskFinished> logger)
        {
            _pSAPIClient = pSAPIClient;
            _logger = logger;

        }

        [FunctionName("TaskFinished")]
        public async Task RunAsync([ServiceBusTrigger("%AzServiceBusTaskFinishedQueue%", Connection = "AzServiceBusConnectionString")] ServiceBusReceivedMessage message)
        {
            //var finishTask = await AZServiceBusUtils.ConvertMessageToDTO<TaskFinishedMessageDTO>(message);

            using var stream = message.Body.ToStream();
            var body = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
            var taskFinishedMessageDTO = new TaskFinishedMessageDTO(message.CorrelationId, body);


            var finish = new FinishProcessTaskDTO(long.Parse(message.CorrelationId), taskFinishedMessageDTO.TaskId, PSDTO.Enums.SetTaskStatusEnum.success);


            var result = await _pSAPIClient.FinishProcessTaskAsync(finish);


            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {message.CorrelationId}");
        }
    }
}
