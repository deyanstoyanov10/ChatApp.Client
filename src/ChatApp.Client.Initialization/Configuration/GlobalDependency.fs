namespace ChatApp.Client.Initialization.Configuration

open Serilog
open ChatApp.Client
open Confluent.Kafka

type GlobalDependency = {
    Logger: ILogger
    ClientConfiguration: ClientConfiguration
    KeyDeserializer: IDeserializer<string>
    MessageValueDeserializer: IDeserializer<Contracts.Message>
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
    }

