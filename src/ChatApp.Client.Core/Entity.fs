namespace ChatApp.Client.Core

open ChatApp.Client.Contracts

type EntityChanged =
    | MessageCreate of Message
    | MessageUpdate of Message
    | MessageDelete of Message

