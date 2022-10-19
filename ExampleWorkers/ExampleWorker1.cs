using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ExampleWorkers
{
    public class ExampleWorker1
    {
        private readonly ILogger<ExampleWorker1> _logger;

        public ExampleWorker1(ILogger<ExampleWorker1> log)
        {
            _logger = log;
        }

        [FunctionName("ExampleWorker1")]
        [return: ServiceBus("%AzServiceBusTaskFinishedQueue%", Connection = "AzServiceBusConnectionString")]
        public ServiceBusMessage RunAsync([ServiceBusTrigger("%AzServiceBusExampleWorker1Queue%", Connection = "AzServiceBusConnectionString")] ServiceBusReceivedMessage message,
            ExecutionContext executionContext)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function processed message: {message.Body.ToString()}");

            return new ServiceBusMessage("Hello");

        }
    }
}
