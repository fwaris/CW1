namespace CW1.Server
open System.Threading.Tasks
open Microsoft.AspNetCore.SignalR
open FSharp.Control
open CW1.Client
open System.Collections.Concurrent
open Microsoft.Extensions.Hosting

///manage client state
module Clients  =
    let clientMap = ConcurrentDictionary<string,int>()

    let updateClients (clients:IHubClients) = 
        async {
            for kv in clientMap do
                let cnnId = kv.Key
                let mutable count = kv.Value
                try 
                    let client = clients.Client(cnnId)
                    do! client.SendAsync(ClientHub.fromServer,Srv_Count count) |> Async.AwaitTask
                    clientMap.TryUpdate(cnnId,count+1,count) |> ignore
                with ex ->
                    printfn $"Error pinging client {cnnId}: {ex.Message}"
                    clientMap.TryRemove(cnnId,&count) |> ignore
        }

    let addClient cnnId = clientMap.TryAdd(cnnId,0) |> ignore

    let resetClient cnnId = 
        match clientMap.TryGetValue (cnnId) with
        | true, c -> clientMap.TryUpdate(cnnId,0,c) |> ignore
        | _ -> ()


//hubs are temporary objects that are created for each client invocation
type ServerHub() as this =
    inherit Hub()

    static member SendMessage(client:ISingleClientProxy, msg:ServerInitiatedMessages) =
        task {
            return! client.SendAsync(ClientHub.fromServer,msg)            
        }

    member this.FromClient(msg:ClientInitiatedMessages) : Task = 
        let cnnId = this.Context.ConnectionId
        let client = this.Clients.Client(cnnId)
        let dispatch msg = ServerHub.SendMessage(client,msg) |> ignore
        task{
            try 
                match msg with 

                | Clnt_Connected c ->
                    printfn $"Client connected: {c.Time}"
                    Clients.addClient cnnId

                | Clnt_Reset _ -> 
                    Clients.resetClient cnnId
                    dispatch (Srv_Notification "Counter reset on server")
            with ex ->
                printf $"Error in FromClient: {ex.Message}"
        }

//service to update connected clients on a periodic basis
type ClientService(hub:IHubContext<ServerHub>) =

    let mutable go = true
    let loop =
        async {
                while go do
                    do! Async.Sleep 1000
                    try
                        do! Clients.updateClients (hub.Clients)
                    with ex ->
                        printfn $"Error in ping loop: {ex.Message}"
            }

    interface IHostedService with

        member this.StartAsync(cts) = 
            Async.Start(loop)
            Task.CompletedTask

        member this.StopAsync(cts) = 
            go <- false
            Task.CompletedTask
