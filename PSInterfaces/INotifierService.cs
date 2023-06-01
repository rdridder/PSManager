using PSDTO.Enums;

namespace PSInterfaces
{
    public interface INotifierService
    {
        public Task SendMessageAsync(NotifierEnum action, string message);
    }
}
