using PSAZServiceBus.Utils;
using PSDTO.Messaging;
using PSInterfaces;

namespace PSAPI.Test.Mock
{
    public class MockAZMessageService : IMessageService
    {
        private readonly MockAZServiceBus _serviceBus;

        public MockAZMessageService(MockAZServiceBus serviceBus)
        {
            _serviceBus = serviceBus;
        }

        public Task SendStartTaskMessage(StartTaskMessageDTO messageDTO, string queueName)
        {
            var message = AZServiceBusUtils.ConvertDTOToMessage(messageDTO);
            _serviceBus.addMessage(message);
            return Task.CompletedTask;
        }

        public Task SendTaskFinishedMessage(TaskFinishedMessageDTO messageDTO, string queueName)
        {
            var message = AZServiceBusUtils.ConvertDTOToMessage(messageDTO);
            _serviceBus.addMessage(message);
            return Task.CompletedTask;
        }
    }
}
