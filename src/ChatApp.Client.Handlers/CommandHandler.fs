namespace ChatApp.Client.Handlers

open ChatApp.Client.Core
open ChatApp.Client.Database
open ChatApp.Client.Contracts

open System
open System.Threading.Tasks
open System.Reactive.Subjects
open System.Threading.Tasks.Dataflow

type CommandHandler(database: Database) =
    
    let stream = new Subject<Message>()

    let processCommand command (tcs: TaskCompletionSource) : Task = task {
        match command with
        | UpdateMessage message -> 
            message 
            |> MessageCreate 
            |> database.UpdateMessage

        tcs.SetResult()
    }

    let messageActor = ActionBlock(fun (command, tcs) -> processCommand command tcs)

    do
        database.EntityUpdate.Add(fun update -> stream.OnNext(update))

    let handleCommand command =
        let tcs = TaskCompletionSource()

        messageActor.Post((command, tcs)) |> ignore

    member _.HandleCommand = handleCommand

    member _.Stream = stream :> IObservable<Message>