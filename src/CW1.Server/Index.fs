module CW1.Server.Index
open Bolero
open Bolero.Html
open Bolero.Server.Html
open CW1

let page = doctypeHtml {
    head {
        meta { attr.charset "UTF-8" }
        meta { attr.name "viewport"; attr.content "width=device-width, initial-scale=1.0" }
        title { "Bolero Application" }
        ``base`` { attr.href "/" }
        link { attr.rel "stylesheet"; attr.href "CW1.Client.styles.css" }
        link {attr.href "_content/MudBlazor/MudBlazor.min.css"; attr.rel "stylesheet"}
        script {attr.src "_content/MudBlazor/MudBlazor.min.js"}
    }
    body {
        div { attr.id "main"; comp<CW1.Client.App.MyApp> }
        boleroScript
    }
}
