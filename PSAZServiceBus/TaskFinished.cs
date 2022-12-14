using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PSDTO.Enums;
using PSDTO.Messaging;
using PSDTO.Process;
using PSInterfaces;
using System;
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
            using var stream = message.Body.ToStream();
            var body = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
            var taskFinishedMessageDTO = new TaskFinishedMessageDTO(message.CorrelationId, body);

            var status = Enum.Parse<SetTaskStatusEnum>(taskFinishedMessageDTO.TaskStatus);
            var finishDTO = new FinishProcessTaskDTO(long.Parse(message.CorrelationId), taskFinishedMessageDTO.TaskId, status);
            var result = await _pSAPIClient.FinishProcessTaskAsync(finishDTO);

            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {message.CorrelationId}");
        }
    }
}
