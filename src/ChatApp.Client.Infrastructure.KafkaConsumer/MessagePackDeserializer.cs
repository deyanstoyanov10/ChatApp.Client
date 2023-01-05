namespace ChatApp.Client.Infrastructure.KafkaConsumer;

using MessagePack;
using Confluent.Kafka;

public class MessagePackDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        return data == null ? default : MessagePackSerializer.Deserialize<T>(data.ToArray());
    }
}
