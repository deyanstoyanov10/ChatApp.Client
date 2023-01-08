namespace ChatApp.Client.Core

open ChatApp.Client.Contracts

type Command =
    | UpdateMessage of message: Message

