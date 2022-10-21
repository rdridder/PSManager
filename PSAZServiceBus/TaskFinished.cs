using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PSAZServiceBus.Services;
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
            var messageService = new AZMessageService(null);
            //var dto = await messageService.ConvertMessageToDTO(message);




            _logger.LogInformation($"C# ServiceBus topic trigger function processed message:");
        }
    }
}
