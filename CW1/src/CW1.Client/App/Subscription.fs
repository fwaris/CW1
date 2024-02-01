namespace CW1.Client
open System
open Elmish
open System.Threading.Channels
open FSharp.Control

//pump background-generated messages into the main program message loop via an Elmish subscription
module Subscription =

    let asyncMsgQueue = 
            let ops = BoundedChannelOptions(1000,
                        SingleReader = true,
                        FullMode = BoundedChannelFullMode.DropNewest,
                        SingleWriter = true)
            Channel.CreateBounded<Message>(ops)

    let queueReader() =
        asyncSeq{
            while true do 
                let! msg = asyncMsgQueue.Reader.ReadAsync().AsTask() |> Async.AwaitTask
                yield msg
        }

    //this can be plugged into Elmish's subscription mechanism
    let asyncMessages (model:Model) : (SubId * Subscribe<Message>) list =
        let sub dispatch : IDisposable =
            queueReader()
            |> AsyncSeq.iter(fun msg -> 
                try dispatch msg with ex -> printfn "%A" ex.Message)
            |> Async.Start
            {new IDisposable with member _.Dispose() = ()}
        [["asyncMessages"],sub]

    //post background-generated into the subscription queue
    let post msg = asyncMsgQueue.Writer.TryWrite(msg) |> ignore
