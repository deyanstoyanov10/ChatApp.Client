namespace ChatApp.Client.Handlers

open ChatApp.Client.Core
open ChatApp.Client.Contracts

type KafkaEvent =
    | MessageReceived of message: Message

type KafkaMessageHandler(commandHandler: CommandHandler) =
    
    let handleKafkaEvent event =
        match event with
        | MessageReceived message ->
            message
            |> UpdateMessage
            |> commandHandler.HandleCommand

    member _.HandleKafkaEvent = handleKafkaEvent