namespace ChatApp.Client.Handlers

open ChatApp.Client.Core
open ChatApp.Client.Contracts

open System
open System.Reactive.Linq
open System.Reactive.Subjects

type PushHandler(
        pushData: ClientResult -> unit,
        configuration: PushClientConfiguration) =

    let updateStream = new Subject<Message>()

    let publishStream =
        updateStream
            .Buffer(configuration.PublishInterval, configuration.MaxSize)
            .Subscribe(fun list ->
                if list <> null && list.Count > 0 then
                    list
                    :> seq<Message>
                    |> ClientResult.Update
                    |> pushData
            )

    let handleMessageUpdate message =
        message
        |> updateStream.OnNext

    static member Create
        (
            pushData: ClientResult -> unit,
            configuration: PushClientConfiguration
        )
        = new PushHandler(pushData, configuration)

    member _.HandleMessageUpdate = handleMessageUpdate

    interface IDisposable with
        member _.Dispose() =
            publishStream.Dispose()