namespace PSInterfaces
{
    public interface IMessageBusFactory<T>
    {
        T GetClient(string senderName);
    }
}
