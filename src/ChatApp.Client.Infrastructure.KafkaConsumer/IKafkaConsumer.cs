namespace ChatApp.Client.Infrastructure.KafkaConsumer;

public interface IKafkaConsumer<TKey, TContract> : IDisposable
{
    void Start();
}