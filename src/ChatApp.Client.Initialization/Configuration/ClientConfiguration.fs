namespace ChatApp.Client.Initialization.Configuration

open ChatApp.Client.Infrastructure.KafkaConsumer

[<CLIMutable>]
type ClientConfiguration = {
    kafkaConsumerConfig: KafkaConsumerConfig
}
