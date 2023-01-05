namespace ChatApp.Client.Infrastructure.KafkaConsumer;

using Contracts;
using Confluent.Kafka;

public class MessageKafkaConsumer : KafkaConsumer<Message>
{
    public MessageKafkaConsumer(
        KafkaConsumerConfig consumerConfig, 
        IDeserializer<string> keyDeserializer, 
        IDeserializer<Message> valueDeserializer) 
        : base(consumerConfig, keyDeserializer, valueDeserializer) {}
}