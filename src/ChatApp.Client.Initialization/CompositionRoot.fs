namespace ChatApp.Client.Initialization

open ChatApp.Client.Initialization.Configuration
open ChatApp.Client.Infrastructure.KafkaConsumer

open Microsoft.Extensions.Logging

type Services = {
    MessageKafkaConsumer: MessageKafkaConsumer
}

type CompositionRoot(dep: GlobalDependency) =
    
    let logger = dep.Logger.ForContext<CompositionRoot>()

    let createMessageKafkaConsumer() =
        let consumer = new MessageKafkaConsumer(dep.ClientConfiguration.kafkaConsumerConfig, dep.KeyDeserializer, dep.MessageValueDeserializer)

        consumer.add_OnMessageReceived(fun message ->
            logger.Information("Message Received Key: {key}", message.Key)
        )

        consumer.add_OnError(fun error ->
            logger.Error("Something went wrong reason: {error}", error)
        )

        consumer.add_OnKafkaError(fun _ error ->
            logger.Error("Kafka Error reason: {reason}", error.Reason)
        )
        consumer

    let _messageKafkaConsumer = createMessageKafkaConsumer()

    let services = {
        Services.MessageKafkaConsumer = _messageKafkaConsumer
    }

    let start (services: Services) =
        services.MessageKafkaConsumer.Start()

    let stop (services: Services) =
        services.MessageKafkaConsumer.Dispose()

    member _.Start() =
        start services
        
    member _.Stop() =
        stop services

    