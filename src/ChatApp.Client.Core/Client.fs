namespace ChatApp.Client.Core

open System

open ChatApp.Client.Contracts

type ClientResult =
    | Update of Message seq

type Response = {
    Data: Object
}

[<CLIMutable>]
type PushClientConfiguration = {
    PublishInterval: TimeSpan
    MaxSize: int
}

module ResponseHelpers =

    let convertToResponseObject clientResult =
        match clientResult with
        | Update messages ->
            {
                Data = messages
            }