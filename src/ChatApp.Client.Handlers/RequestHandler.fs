namespace ChatApp.Client.Handlers

open System
open System.Threading.Tasks

open ChatApp.Client.Core
open ChatApp.Client.Contracts
open System.Threading.Tasks.Dataflow

type RequestHandler(
        eventStream: IObservable<Message>,
        pushHandler: PushHandler,
        publishActor: ActionBlock<ClientResult>) =

    let clientActor = ActionBlock(pushHandler.HandleMessageUpdate)

    let handleUpdateMessage message =
        message
        |> clientActor.Post
        |> ignore

    let eventStreamSubscription =
        eventStream.Subscribe(handleUpdateMessage)

    static member Create
        (
            eventStream: IObservable<Message>,
            pushCallback: Func<ClientResult, Task>,
            configuration: PushClientConfiguration
        )
        =
        let pushActorFunc clientData : Task = task {
            try
                do! pushCallback.Invoke clientData
            with
            | ex -> Console.WriteLine($"Error: {ex.Message}")
        }

        let pushActor = ActionBlock(pushActorFunc)

        let pushHandler = new PushHandler(pushActor.Post >> ignore, configuration)
        new RequestHandler(eventStream, pushHandler, pushActor)

    interface IDisposable with
        member _.Dispose() =
            clientActor.Complete()
            publishActor.Complete()
            eventStreamSubscription.Dispose()
            (pushHandler :> IDisposable).Dispose()

