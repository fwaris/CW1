﻿namespace CW1.Client
open Microsoft.AspNetCore.Components
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.SignalR.Client
open FSharp.Control
open System.Text.Json
open System.Text.Json.Serialization
open Microsoft.Extensions.DependencyInjection

module ClientHub =
    open System.Threading.Channels
    let fromServer = "FromServer"
    let fromClient = "FromClient"
    let urlPath = "/cw1hub"

    let configureSer (o:JsonSerializerOptions)= 
        JsonFSharpOptions.Default()
            .WithAllowNullFields(true)
            .WithAllowOverride(true)
            .AddToJsonSerializerOptions(o)                
        o
        
    //signalr hub connection that can send/receive messages to/from server
    let connection (loggerProvider: ILoggerProvider) (navMgr:NavigationManager)  =
        let hubConnection =
            HubConnectionBuilder()               
                .AddJsonProtocol(fun o -> configureSer o.PayloadSerializerOptions |> ignore)
                .WithUrl(navMgr.ToAbsoluteUri(urlPath))
                .WithAutomaticReconnect()           
                .ConfigureLogging(fun logging ->
                    logging.AddProvider(loggerProvider) |> ignore
                )           
                .Build()
        (hubConnection.StartAsync()) |> Async.AwaitTask |> Async.Start
        hubConnection

    let send (conn:HubConnection) (msg:ClientInitiatedMessages) = 
        conn.SendAsync(fromClient,msg) 

