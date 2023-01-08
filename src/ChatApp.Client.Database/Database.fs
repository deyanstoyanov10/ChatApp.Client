namespace ChatApp.Client.Database

open ChatApp.Client.Core
open ChatApp.Client.Contracts

type Database() =
    let callback = new Event<Message>()

    let updateMessages entityChange =
        match entityChange with
        | MessageCreate message -> callback.Trigger(message)
        | MessageUpdate message -> callback.Trigger(message)
        | MessageDelete message -> callback.Trigger(message)

    member _.UpdateMessage = updateMessages

    member _.EntityUpdate = callback.Publish