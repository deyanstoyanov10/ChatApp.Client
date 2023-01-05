namespace ChatApp.Client.Infrastructure.KafkaConsumer;

using Confluent.Kafka;

public class KafkaConsumerConfig
{
    public string GroupId { get; set; }

    public ConsumerConfig ConsumerConfig { get; set; }

    public string MessageTopic { get; set; }
}
