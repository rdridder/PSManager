namespace PSServices
{
    public interface IMessageBusFactory<T>
    {
        T GetClient(string senderName);
    }
}
