using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PSAZServiceBus.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSAZServiceBus
{
    public class TaskFinished
    {
        private readonly ILogger<TaskFinished> _logger;

        public TaskFinished(ILogger<TaskFinished> log)
        {
            _logger = log;
        }

        [FunctionName("TaskFinished")]
        public async Task Run([ServiceBusTrigger("%AzServiceBusTaskFinishedQueue%", Connection = "AzServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ExecutionContext executionContext)
        {
            var messageService = new MessageService(null);
            var dto = await messageService.ConvertMessageToDTO(message);



            var bodyText = System.Text.Encoding.UTF8.GetString(message.Body);
            var body = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyText);

            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {bodyText}");
        }
    }
}
