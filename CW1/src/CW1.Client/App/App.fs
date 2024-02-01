namespace CW1.Client
open System
open System.Net.Http
open Microsoft.AspNetCore.Components
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.SignalR.Client
open Elmish
open Bolero
open Bolero.Html
open Bolero.Remoting.Client
open MudBlazor
open CW1.Client.Views

module App =
    let router = Router.infer SetPage (fun model -> model.page)

    let view model dispatch =
        ecomp<MainLayout,_,_> model dispatch {attr.empty()}

    type MyApp() =
        inherit ProgramComponent<Model, Message>()

        [<Inject>]
        member val Snackbar : ISnackbar = Unchecked.defaultof<_> with get, set

        [<Inject>]
        member val logger:ILoggerProvider = Unchecked.defaultof<_> with get, set

        member val hubConn : HubConnection = Unchecked.defaultof<_> with get, set

        override this.Program =

            //hub connection
            this.hubConn <- ClientHub.connection this.logger this.NavigationManager 
            let clientDispatch msg = this.Dispatch (FromServer msg) 
            let serverDispatch = ClientHub.send this.hubConn            
            this.hubConn.On<ServerInitiatedMessages>(ClientHub.fromServer,clientDispatch) |> ignore

            let uparms =
                {                    
                    snkbar = this.Snackbar
                    serverDispatch = serverDispatch
                }

            let update = Update.update uparms

            Program.mkProgram (fun _ -> Update.initModel, Cmd.ofMsg Started) update view 
            |> Program.withSubscription Subscription.asyncMessages
            |> Program.withRouter router
