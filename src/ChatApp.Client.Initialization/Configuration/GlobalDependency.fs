namespace ChatApp.Client.Initialization.Configuration

open ChatApp.Client
open ChatApp.Client.Core

open Serilog
open Confluent.Kafka

type GlobalDependency = {
    Logger: ILogger
    ClientConfiguration: ClientConfiguration
    KeyDeserializer: IDeserializer<string>
    MessageValueDeserializer: IDeserializer<Contracts.Message>
    PushClientConfiguration : PushClientConfiguration
} with
  static member Create
   (logger: ILogger,
    clientConfiguration: ClientConfiguration,
    keyDeserializer: IDeserializer<string>,
    messageValueDeserializer: IDeserializer<Contracts.Message>) = {
    Logger = logger
    ClientConfiguration = clientConfiguration
    KeyDeserializer = keyDeserializer
    MessageValueDeserializer = messageValueDeserializer
    PushClientConfiguration = clientConfiguration.pushClientConfiguration
    }

