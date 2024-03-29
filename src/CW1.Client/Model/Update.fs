﻿namespace CW1.Client
open System
open System.Threading.Tasks
open Elmish
open MudBlazor

type UpdateParms = 
    {
        snkbar              : ISnackbar
        serverDispatch      : ClientInitiatedMessages -> Task        
    }

module Update =
        
    let initModel =    
        {
            count = 0
            page = Home
        }

    let notify (snkbar:ISnackbar) (msg:string) = snkbar.Add msg |> ignore
    let notifyError (snkbar:ISnackbar) (msg:string) =  snkbar.Add(msg,severity = Severity.Error) |> ignore

    // wrapper for sending messages to the server that meets the type signature of the Elmish Cmd.ofTask
    let send (serverDispatch:ClientInitiatedMessages -> Task) (msg:ClientInitiatedMessages) = 
        task{
            do! serverDispatch msg            
        }

    //Elmish update function
    let update (updParms:UpdateParms) (msg:Message) (model:Model) =
        match msg with
        | Started  -> model, Cmd.OfTask.either (send updParms.serverDispatch) (Clnt_Connected {Time=DateTime.Now}) Nop Error
        | Reset -> model,  Cmd.OfTask.either (send updParms.serverDispatch) (Clnt_Reset "") Nop Error
        | Error ex -> notifyError updParms.snkbar (ex.Message); model, Cmd.none
        | Nop _ -> model, Cmd.none
        | SetPage page -> {model with page = page}, Cmd.none

        //from server
        | FromServer (Srv_Count i) -> {model with count = i}, Cmd.none
        | FromServer (Srv_Notification msg) -> notify updParms.snkbar msg; model, Cmd.none


