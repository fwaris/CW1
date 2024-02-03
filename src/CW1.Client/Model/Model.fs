namespace CW1.Client
open System
open Bolero

//Very often need multiple 'pages' even in a single page app (eg. for authentication).
//Here we are retaining the paging mechanism, even though there is only one page in this app
type Page =
    | [<EndPoint "/">] Home
    //| [<EndPoint "/authentication/{action}">] Authentication of action:string 

//Application model or state for the UI
type Model = {
    count   : int
    page    : Page
}

//Type definition for data exchanged between client and server in a message (as JSON over the wire)
type ClientInfo = {
    Time: DateTime
}

///Messages sent by the server to the client
type ServerInitiatedMessages =
    | Srv_Count of int
    | Srv_Notification of string

///Messages sent by the client to the server
type ClientInitiatedMessages =
    | Clnt_Reset of string      //need message parameter for signalR serialization (it seems)
    | Clnt_Connected of ClientInfo  //

///Elmish messages handled by the update function
type Message =
    | Reset
    | Nop of unit 
    | Error of exn
    | SetPage of Page
    | Started 
    | FromServer of ServerInitiatedMessages
