using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PSInterfaces;
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
            var result = await _pSAPIClient.GetProcessAsync(1);


            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {message.CorrelationId}");
        }
    }
}
