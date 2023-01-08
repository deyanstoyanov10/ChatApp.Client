namespace ChatApp.Client.Initialization

open ChatApp.Client.Database
open ChatApp.Client.Handlers

open ChatApp.Client.Initialization.Configuration
open ChatApp.Client.Infrastructure.KafkaConsumer

type Services = {
    MessageKafkaConsumer: MessageKafkaConsumer
    CommandHandler: CommandHandler
}

type CompositionRoot(dep: GlobalDependency) =
    
    let logger = dep.Logger.ForContext<CompositionRoot>()

    let createDatabase() =
        Database()

    let createCommandHandler database =
        CommandHandler(database)

    let createKafkaMessageHandler commandHandler =
        KafkaMessageHandler(commandHandler)

    let createMessageKafkaConsumer(kafkaMessageHandler: KafkaMessageHandler) =
        let consumer = new MessageKafkaConsumer(dep.ClientConfiguration.kafkaConsumerConfig, dep.KeyDeserializer, dep.MessageValueDeserializer)

        consumer.add_OnMessageReceived(fun message ->
            message.Value
            |> MessageReceived
            |> kafkaMessageHandler.HandleKafkaEvent
        )

        consumer.add_OnError(fun error ->
            logger.Error("Something went wrong reason: {error}", error)
        )

        consumer.add_OnKafkaError(fun _ error ->
            logger.Error("Kafka Error reason: {reason}", error.Reason)
        )
        consumer

    let _database = createDatabase()
    let _commandHandler = createCommandHandler(_database)
    let _kafkaMessageHandler = createKafkaMessageHandler(_commandHandler)
    let _messageKafkaConsumer = createMessageKafkaConsumer(_kafkaMessageHandler)

    let services = {
        Services.MessageKafkaConsumer = _messageKafkaConsumer
        Services.CommandHandler = _commandHandler
    }

    let start (services: Services) =
        services.MessageKafkaConsumer.Start()

    let stop (services: Services) =
        services.MessageKafkaConsumer.Dispose()

    member _.Services = services

    member _.Start() =
        start services
        
    member _.Stop() =
        stop services

    